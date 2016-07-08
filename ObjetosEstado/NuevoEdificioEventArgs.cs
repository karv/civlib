using System;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Argumento de evento de nuevos edificios
	/// </summary>
	[Serializable]
	public class NuevoEdificioEventArgs : EventArgs
	{
		/// <summary>
		/// Edificio nuevo
		/// </summary>
		public readonly Edificio Edificio;

		/// <param name="edificio">Edificio.</param>
		public NuevoEdificioEventArgs (Edificio edificio)
		{
			Edificio = edificio;
		}
	}
}