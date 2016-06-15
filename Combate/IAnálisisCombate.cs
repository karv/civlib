using Civ.ObjetosEstado;

namespace Civ.Combate
{
	/// <summary>
	/// Representa el análisis, contexto o resultado de un tick de combate.
	/// </summary>
	public interface IAnálisisCombate
	{
		/// <summary>
		/// Devuelve el atacante
		/// </summary>
		/// <value>The atacante.</value>
		IAtacante Atacante { get; }

		/// <summary>
		/// Devuelve el defensor
		/// </summary>
		/// <value>The defensor.</value>
		Stack Defensor { get; }

		/// <summary>
		/// Devuelve un <see cref="System.String"/> que representa los resultados del combate.
		/// </summary>
		string Análisis ();
	}
}