using System;
using System.Collections.Generic;

namespace Civ
{
	public partial class Ciudad
	{
		public readonly Armada Defensa;

		// Todo para crear unidades
		/// <summary>
		/// Entrena una cantidad de unidades de una clase fija.
		/// Incluye la unidad en la armada de la ciudad.
		/// </summary>
		/// <param name="uRAW">Clase de unidades a entrenar</param>
		/// <param name="Cantidad">Cantidad</param>
		/// <returns>Devuelve un arreglo con las unidades que se pudieron entrenar.</returns>
		public Unidad[] EntrenarUnidades (UnidadRAW uRAW, ulong Cantidad)
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
			} while (i < Cantidad && U != null );

			return ret.ToArray();
		}
		/// <summary>
		/// Entrena una unidad de una clase específica.
		/// Incluye la unidad en la armada de la ciudad.
		/// </summary>
		/// <param name="uRAW">Tipo de unidad.</param>
		/// <returns>Devuelve la instancia de la unidad creada</returns>
		public Unidad EntrenarUnidades (UnidadRAW uRAW)
		{
			Unidad ret = null;
			if (uRAW.CostePoblación <= getTrabajadoresDesocupados && uRAW.Reqs <= Almacén)	//Si puede pagar
			{
				ret = new Unidad(uRAW, this);
				Defensa.AgregaUnidad(ret);						// Agregar la unidad a la defensa de la ciudad.
				_PoblaciónProductiva -= uRAW.CostePoblación;	// Recluta desde la población productiva.
				foreach (var x in uRAW.Reqs.Keys)				// Quita los recursos que requiere.
				{
					Almacén[x] -= uRAW.Reqs[x];
				}
			}
			return ret;											// Devuelve la unidad creada.
		}
	}
}