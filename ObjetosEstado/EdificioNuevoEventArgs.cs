using System;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Argumentos de edificio nuevo
	/// </summary>
	[Serializable]
	public class EdificioNuevoEventArgs : EventArgs
	{
		/// <summary>
		/// The edificio.
		/// </summary>
		public readonly Edificio Edificio;

		/// <param name="edificio">Edificio.</param>
		public EdificioNuevoEventArgs (Edificio edificio)
		{
			Edificio = edificio;
		}
	}
}