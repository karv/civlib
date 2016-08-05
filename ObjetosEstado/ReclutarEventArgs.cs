using Civ.RAW;
using System;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Argumentos de evento de reclutamiento
	/// </summary>
	[Serializable]
	public class ReclutarEventArgs : EventArgs
	{
		/// <summary>
		/// Tipo de unidad
		/// </summary>
		public readonly IUnidadRAW Tipo;

		/// <summary>
		/// Cantidad reclutada
		/// </summary>
		public readonly long Cantidad;

		/// <param name="tipo">Tipo.</param>
		/// <param name="cantidad">Cantidad.</param>
		public ReclutarEventArgs (IUnidadRAW tipo, long cantidad)
		{
			Tipo = tipo;
			Cantidad = cantidad;
		}
	}
	
}