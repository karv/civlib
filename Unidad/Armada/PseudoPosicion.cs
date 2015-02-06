using System;

namespace Civ
{
	/// <summary>
	/// Representa un lugar que no es terreno, más bien es un punto en una arista de la Topología del mundo.
	/// </summary>
	public class PseudoPosicion :IPosicion
	{
		/// <summary>
		/// Punto A de esta posición.
		/// </summary>
		public IPosicion Origen;
		/// <summary>
		/// Punto B de esta posición
		/// </summary>
		public IPosicion Destino;
		/// <summary>
		/// Distancia entre Origen y "esta posición"
		/// </summary>
		public float Avance;

		public PseudoPosicion()
		{
		}
	}
}

