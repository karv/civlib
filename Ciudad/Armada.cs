using System;
using System.Collections.Generic;

namespace Civ
{
	public partial class Ciudad
	{
		public readonly Armada Defensa;

		public List<Armada> armadasEnCiudad()
		{
			Pseudoposicion posCiudad = (Pseudoposicion)Terr;
			//List<Armada> ret = CivDueno.Armadas.FindAll(x => x.Posicion.Equals(posCiudad));
			List<Armada> rat = new List<Armada>();
			foreach (var x in CivDueno.Armadas)
			{
				if (!x.esDefensa && x.Posicion.Equals(posCiudad))
					rat.Add(x);
			}
			return rat;
		}

		// Todo para crear unidades
		/// <summary>
		/// Entrena una cantidad de unidades de una clase fija.
		/// Incluye la unidad en la armada de la ciudad.
		/// </summary>
		/// <param name="uRAW">Clase de unidades a entrenar</param>
		/// <param name="Cantidad">Cantidad</param>
		/// <returns>Devuelve un arreglo con las unidades que se pudieron entrenar.</returns>
		public Unidad[] EntrenarUnidades(UnidadRAW uRAW, ulong Cantidad)
		{
			List<Unidad> ret = new List<Unidad>();
			Unidad U;
			ulong i = 0;

			do
			{
				U = EntrenarUnidades(uRAW);
				if (U != null)
					ret.Add(U);
				i++;
			} while (i < Cantidad && U != null);

			return ret.ToArray();
		}

		/// <summary>
		/// Entrena una unidad de una clase específica.
		/// Incluye la unidad en la armada de la ciudad.
		/// </summary>
		/// <param name="uRAW">Tipo de unidad.</param>
		/// <returns>Devuelve la instancia de la unidad creada</returns>
		public Unidad EntrenarUnidades(UnidadRAW uRAW)
		{
			Unidad ret = null;
			if (uRAW.CostePoblacion <= getTrabajadoresDesocupados && Almacen.PoseeRecursos((ListasExtra.ListaPeso<Recurso>)uRAW.Reqs))	//Si puede pagar
			{
				ret = new Unidad(uRAW, this);
				Defensa.AgregaUnidad(ret);						// Agregar la unidad a la defensa de la ciudad.
				_PoblacionProductiva -= uRAW.CostePoblacion;	// Recluta desde la población productiva.
				foreach (var x in uRAW.Reqs.Keys)				// Quita los recursos que requiere.
				{
					Almacen[x] -= uRAW.Reqs[x];
				}
			}
			return ret;											// Devuelve la unidad creada.
		}
	}
}