using System;
using Civ.ObjetosEstado;

namespace Civ.Orden
{
	/// <summary>
	/// Representa una orden de una armada
	/// </summary>
	public interface IOrden
	{
		/// <summary>
		/// Devuelve la armada de esta orden
		/// </summary>
		/// <value>The armada.</value>
		Armada ArmadaEjecutante { get; }

		/// <summary>
		/// Ejecuta la orden
		/// Devuelve true si la orden ha sido terminada.
		/// </summary>
		/// <param name="t">Tiempo de ejecución</param>
		bool Ejecutar (TimeSpan t);
	}
}