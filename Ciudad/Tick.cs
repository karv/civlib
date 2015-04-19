using System;
using ListasExtra;
using System.Collections.Generic;

namespace Civ
{
	public partial class Ciudad: ITickable
	{
		// Tick
		/// <summary>
		/// Da un tick poblacional
		/// </summary>
		/// <param name="t">Diración del tick</param>
		public void PopTick(float t = 1)
		{
			// Se está suponiendo crecimiento constante entre ticks!!!

			//Crecimiento prometido por sector de edad.
			float[] Crecimiento = new float[3];
			float Consumo = getPoblacion * ConsumoAlimentoPorCiudadanoBase * t;
			//Que coman
			//Si tienen qué comer
			if (Consumo <= AlimentoAlmacen)
			{
				AlimentoAlmacen = AlimentoAlmacen - Consumo;
			}
			else
			{
				//El porcentage de muertes
				float pctMuerte = (1 - (AlimentoAlmacen / (getPoblacion * ConsumoAlimentoPorCiudadanoBase))) * _TasaMortalidadHambruna;
				AlimentoAlmacen = 0;
				//Promesas de muerte por sector.
				Crecimiento[0] -= getPoblacionPreProductiva * pctMuerte;
				Crecimiento[1] -= PoblacionProductiva * pctMuerte;
				Crecimiento[2] -= getPoblacionPostProductiva * pctMuerte;
			}

			//Crecimiento poblacional

			//Infantil a productivo.
			float Desarrollo = TasaDesarrolloBase * getPoblacionPreProductiva * t;
			Crecimiento[0] -= Desarrollo;
			Crecimiento[1] += Desarrollo;
			//Productivo a viejo
			float Envejecer = TasaVejezBase * PoblacionProductiva * t;
			Crecimiento[1] -= Envejecer;
			Crecimiento[2] += Envejecer;
			//Nuevos infantes
			float Natalidad = TasaNatalidadBase * PoblacionProductiva * t;
			Crecimiento[0] += Natalidad;
			//Mortalidad
			Crecimiento[0] -= getPoblacionPreProductiva * TasaMortalidadInfantilBase * t;
			Crecimiento[1] -= PoblacionProductiva * TasaMortalidadProductivaBase * t;
			Crecimiento[2] -= getPoblacionPostProductiva * TasaMortalidadVejezBase * t;

			// Aplicar cambios.

			if (Crecimiento[1] < -(long)getTrabajadoresDesocupados)
			{
				CivDueno.AgregaMensaje("La ciudad {0} ha perdido trabajadores productivos ocupados.", this);
				LiberarTrabajadores(PoblacionProductiva - (ulong)Crecimiento[1]);

			}

			_PoblacionPreProductiva = Math.Max(_PoblacionPreProductiva + Crecimiento[0], 0);
			_PoblacionProductiva = Math.Max(_PoblacionProductiva + Crecimiento[1], 0);
			_PoblacionPostProductiva = Math.Max(_PoblacionPostProductiva + Crecimiento[2], 0);

			if (AutoReclutar)
			{
				// Autoacomodar trabajadores desocupados
				List<Trabajo> Lst = ObtenerListaTrabajos;

				Lst.Sort(((x, y) => x.Prioridad < y.Prioridad ? -1 : 1));

				for (int i = 0; i < Lst.Count && getTrabajadoresDesocupados > 0; i++)
				{
					Lst[i].Trabajadores = Lst[i].MaxTrabajadores;
				}
			}
		}

		/// <summary>
		/// Da un tick hereditario.
		/// </summary>
		public void Tick(float t = 1)
		{
			foreach (ITickable x in Edificios)
			{
				x.Tick(t);
			}
			foreach (var x in Propiedades)
			{
				x.Tick(this, t);
			}
			// Construir edificio.
			if (EdifConstruyendo != null)
			{
				EdifConstruyendo.AbsorbeRecursos();
				if (EdifConstruyendo.EstaCompletado())
				{
					EdifConstruyendo.Completar();
					EdifConstruyendo = null;    //  Ya no se contruye edificio. Para evitar error de duplicidad.
				}
			}

			// Autocontruible
			List<EdificioRAW> PosiblesEdif = Global.g_.Data.EdificiosAutoconstruibles().FindAll(x => !ExisteEdificio(x)); 	// Obtener lista de edificios autocontruibles no construidos.
			foreach (var x in PosiblesEdif)
			{
				if (SatisfaceReq(x.Reqs()))
				{	// Si satisface requerimientos de construcción:
					AgregaEdificio(x);
				}
			}

			// Recursos no almacenados
			foreach (var x in Almacen.Keys)
			{
				if (x.Desaparece)
					Almacen[x] = 0;
			}
		}

		/// <summary>
		/// Destruye los recursos con el flag <c>.desaparecen</c>.
		/// </summary>
		[Obsolete]
		public void DestruirRecursosTemporales()
		{
			// Desaparecen algunos recursos
			List<Recurso> Alm = new List<Recurso>(Almacen.Keys);
			foreach (Recurso x in Alm)
			{
				if (x.Desaparece)
				{
					Almacen[x] = 0;
				}
			}

		}

		/// <summary>
		/// Ejecuta ambos: Tick () y PopTick ().
		/// En ese orden.
		/// </summary>
		public void FullTick(float t = 1)
		{
			PopTick(t);
			Tick(t);

			if (CivDueno != null && getPoblacion == 0)
			{		// Si la población de una ciudad llega a cero, se hacen ruinas (ciudad sin civilización)
				CivDueno.removeCiudad(this);
			}
		}
	}
}