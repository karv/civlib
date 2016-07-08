using System;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Argumento de eventos de ciudad.
	/// </summary>
	[Serializable]
	public sealed class CiudadEventArgs : EventArgs
	{
		/// <summary>
		/// La ciudad
		/// </summary>
		public readonly ICiudad Ciudad;

		/// <param name="ciudad">Ciudad.</param>
		public CiudadEventArgs (ICiudad ciudad)
		{
			Ciudad = ciudad;
		}
		
	}
}