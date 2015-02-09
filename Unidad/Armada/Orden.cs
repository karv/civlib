using System;

namespace Civ
{
	/// <summary>
	/// Representa una orden de una armada
	/// </summary>
	public class Orden
	{
		public enumTipoOrden TipoOrden;

		public enum enumTipoOrden
		{
			/// <summary>
			/// Ir a la IPosicion Destino
			/// </summary>
			Ir
		}

		public Pseudoposicion Destino;
	}

	public partial class Armada
	{
		public Orden Orden;
	}
}

