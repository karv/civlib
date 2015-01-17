using System;

namespace Civ
{
	public partial class Trabajo
	{
		/// <summary>
		/// Ejecuta un tick de tiempo
		/// </summary>
		public void Tick ()
		{
			float PctProd = 1;
			foreach (var x in RAW.EntradaBase.Keys) {
				PctProd = Math.Min (PctProd, Almacén [x] / (RAW.EntradaBase[x] * Trabajadores));
			}

			// Consumir recursos
			foreach (var x in RAW.EntradaBase.Keys) {
				Almacén[x] -= RAW.EntradaBase[x] * Trabajadores * PctProd;
			}


			// Producir recursos
			foreach (var x in RAW.SalidaBase.Keys) {
				Almacén[x] += RAW.SalidaBase[x] * Trabajadores * PctProd;
			}
		}

	}
}

