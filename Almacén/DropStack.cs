using System;
using Civ.Topología;

namespace Civ.Almacén
{
	/// <summary>
	/// Representa un conjunto de recursos que no están (necesariamente) en una ciudad.
	/// </summary>
	[Serializable]
	public class DropStack : AlmacénGenérico, IPosicionable
	{
		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Almacén.DropStack"/> class.
		/// </summary>
		/// <param name="pos">Position.</param>
		public DropStack (Pseudoposición pos)
		{
			Posición = pos;
		}

		#endregion

		#region Posición

		/// <summary>
		/// Devuelve la posición de este drop
		/// </summary>
		/// <value>The posición.</value>
		public Pseudoposición Posición { get; }

		Pseudoposición IPosicionable.Posición ()
		{
			return Posición;
		}

		#endregion
	}
}