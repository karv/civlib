using System;
using Civ.Combate;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Argumento de evento de combate
	/// </summary>
	[Serializable]
	public class CombateEventArgs : EventArgs
	{
		/// <summary>
		/// Devuelve el análisis de combate
		/// </summary>
		/// <value>The análisis.</value>
		public IAnálisisCombate Análisis { get; }

		/// <param name="anal">Anal.</param>
		public CombateEventArgs (IAnálisisCombate anal)
		{
			Análisis = anal;
		}
	}
}