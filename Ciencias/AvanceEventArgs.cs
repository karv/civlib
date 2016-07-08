using System;
using Civ.ObjetosEstado;

namespace Civ.Ciencias
{
	/// <summary>
	/// Argument genérico de eventos científicos
	/// </summary>
	[Serializable]
	public sealed class AvanceEventArgs : EventArgs
	{
		/// <summary>
		/// Ciencia
		/// </summary>
		public readonly Ciencia Ciencia;
		/// <summary>
		/// Civilización
		/// </summary>
		public readonly ICivilización Civil;

		/// <param name="ciencia">Ciencia.</param>
		/// <param name="civil">Civil.</param>
		public AvanceEventArgs (Ciencia ciencia, ICivilización civil)
		{
			Ciencia = ciencia;
			Civil = civil;
		}
	}
}