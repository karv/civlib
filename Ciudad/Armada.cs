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
		public Stack EntrenarUnidades(UnidadRAW uRAW, ulong cantidad = 1)
		{
			//Stack ret = null;
			if (uRAW.CostePoblacion <= getTrabajadoresDesocupados && Almacen.PoseeRecursos((ListasExtra.ListaPeso<Recurso>)uRAW.Reqs))	//Si puede pagar
			{
				_PoblacionProductiva -= uRAW.CostePoblacion;	// Recluta desde la poblaci�n productiva.
				foreach (var x in uRAW.Reqs.Keys)				// Quita los recursos que requiere.
				{
					Almacen[x] -= uRAW.Reqs[x];
				}

				Defensa.AgregaUnidad(uRAW, cantidad);
				//ret = new Stack(uRAW, cantidad, this);
				//Defensa.AgregaUnidad(ret);						// Agregar la unidad a la defensa de la ciudad.
			}
			return Defensa.UnidadesAgrupadas(uRAW);											// Devuelve la unidad creada.
		}
	}
}