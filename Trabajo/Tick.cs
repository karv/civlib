using System;

namespace Civ
{
	public partial class Trabajo: ITickable
	{
		/// <summary>
		/// Ejecuta un tick de tiempo
		/// </summary>
		public void Tick(float t = 1)
		{
			if (Trabajadores > 0)
			{
				// Obtener eficiencia (generada por la disponibilidad de recursos)
				float PctProd = 1;
				foreach (var x in RAW.EntradaBase.Keys)
				{
					PctProd = Math.Min(PctProd, Almacen[x] / (RAW.EntradaBase[x] * Trabajadores * t));
				}

				// Consumir recursos
				foreach (var x in RAW.EntradaBase.Keys)
				{
					Almacen.changeRecurso(x, -RAW.EntradaBase[x] * Trabajadores * PctProd * t);
				}


				// Producir recursos
				foreach (var x in RAW.SalidaBase.Keys)
				{
					Almacen.changeRecurso(x, RAW.SalidaBase[x] * Trabajadores * PctProd * t);
				}
			}
		}
	}
}