using System;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Argumento para eventos sobre la transferencia de objetos sobre diferentes civilizaciones.
	/// </summary>
	[Serializable]
	public sealed class TransferenciaObjetoEventArgs : EventArgs
	{
		/// <summary>
		/// Devuelve la civilización anterior
		/// </summary>
		/// <value>The anterior.</value>
		public ICivilización Anterior { get; }

		/// <summary>
		/// Devuelve la civilización actual
		/// </summary>
		/// <value>The actual.</value>
		public ICivilización Actual { get; }

		/// <summary>
		/// El objeto que cambia de civilización
		/// </summary>
		public readonly object Objeto;

		/// <param name="anterior">Anterior.</param>
		/// <param name="actual">Actual.</param>
		/// <param name="objeto">Objeto.</param>
		public TransferenciaObjetoEventArgs (ICivilización anterior,
		                                     ICivilización actual,
		                                     object objeto)
		{
			Anterior = anterior;
			Actual = actual;
			Objeto = objeto;
		}
	}
	
}