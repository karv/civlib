using System;
using ListasExtra;
using System.Collections.Generic;


namespace Civ
{
	public partial class Ciudad
	{
		// Tick
		public void PopTick()
		{


			//Crecimiento prometido por sector de edad.
			float[] Crecimiento = new float[3];
			float Consumo = getPoblación * ConsumoAlimentoPorCiudadanoBase;
			//Que coman
			//Si tienen qué comer
			if (Consumo <= AlimentoAlmacén) {
				AlimentoAlmacén = AlimentoAlmacén - Consumo;
			} else {
				//El porcentage de muertes
				float pctMuerte = 1 - (AlimentoAlmacén / getPoblación);
				AlimentoAlmacén = 0;
				//Promesas de muerte por sector.
				Crecimiento [0] -= getPoblaciónPreProductiva * pctMuerte;
				Crecimiento [1] -= PoblaciónProductiva * pctMuerte;
				Crecimiento [2] -= getPoblaciónPostProductiva * pctMuerte;
			}

			//Crecimiento poblacional

			//Infantil a productivo.
			float Desarrollo = TasaDesarrolloBase * getPoblaciónPreProductiva;
			Crecimiento [0] -= Desarrollo;
			Crecimiento [1] += Desarrollo;
			//Productivo a viejo
			float Envejecer = TasaVejezBase * PoblaciónProductiva;
			Crecimiento [1] -= Envejecer;
			Crecimiento [2] += Envejecer;
			//Nuevos infantes
			float Natalidad = TasaNatalidadBase * PoblaciónProductiva;
			Crecimiento [0] += Natalidad;
			//Mortalidad
			Crecimiento [0] -= getPoblaciónPreProductiva * TasaMortalidadInfantilBase;
			Crecimiento [1] -= PoblaciónProductiva * TasaMortalidadProductivaBase;
			Crecimiento [2] -= getPoblaciónPostProductiva * TasaMortalidadVejezBase;

			// Aplicar cambios.

            if (Crecimiento[1] < -(long)getTrabajadoresDesocupados)
            {                
                CivDueño.Msj.Add(string.Format("La ciudad {0} ha perdido trabajadores productivos ocupados.", this.Nombre));
                LiberarTrabajadores(PoblaciónProductiva - (ulong)Crecimiento[1]);

            }

			// TODO: Los de mayor prioridad reclutan trabajadores en descanso. (¿opcional?)

			_PoblaciónPreProductiva = Math.Max (_PoblaciónPreProductiva + Crecimiento [0], 0);
			_PoblaciónProductiva = Math.Max (_PoblaciónProductiva + Crecimiento [1], 0);
			_PoblaciónPostProductiva = Math.Max (_PoblaciónPostProductiva + Crecimiento [2], 0);		

		}

		/// <summary>
		/// Da un tick hereditario.
		/// </summary>
		public void Tick (){
			foreach (var x in Edificios) {
				x.Tick ();
			}

			// Construir edificio.
			if (EdifConstruyendo != null)
			{
				EdifConstruyendo.AbsorbeRecursos();
				if (EdifConstruyendo.EstáCompletado()) EdifConstruyendo.Completar();                
			}
		}

		
		/// <summary>
		/// Destruye los recursos con el flag <c>.desaparecen</c>.
		/// </summary>
		public void DestruirRecursosTemporales()
		{
			// Desaparecen algunos recursos
			List<Recurso> Alm = new List<Recurso>(Almacén.Keys);
			foreach (Recurso x in Alm)
			{
				if (x.Desaparece)
				{
					Almacén[x] = 0;
				}
			}

		}

		/// <summary>
		/// Ejecuta ambos: Tick () y PopTick ().
		/// En ese orden.
		/// </summary>
		public void FullTick (){
			PopTick();
			Tick();
		}

	}
}

