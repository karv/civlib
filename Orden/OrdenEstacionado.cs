using System;
using Civ.ObjetosEstado;


namespace Civ.Orden
{
	/// <summary>
	/// Representa la orden <c>nula</c>; no hace nada.
	/// </summary>
	[Serializable]
	public class OrdenEstacionado : IOrden
	{
		/// <summary>
		/// Ejecuta la orden
		/// Devuelve true si la orden ha sido terminada.
		/// </summary>
		/// <param name="t">Tiempo de ejecución</param>
		public bool Ejecutar (TimeSpan t)
		{
			return false;
		}

		/// <summary>
		/// Devuelve la armada de esta orden
		/// </summary>
		/// <value>The armada.</value>
		public Armada ArmadaEjecutante
		{
			get
			{
				return null;
			}
		}
	}
}