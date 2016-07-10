using Civ.ObjetosEstado;
using System;

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

		/// <summary>
		/// Devuelve la duración del combate.
		/// </summary>
		TimeSpan Duración { get; }

		/// <summary>
		/// Une este análisis con otro.
		/// No modifica el otro.
		/// </summary>
		void UnirCon (IAnálisisCombate anal);

		/// <summary>
		/// Revisa y devuelve un valor indicando si tiene sentido unir esta instancia con otra dada.
		/// </summary>
		/// <returns><c>true</c>, si es posible unir, <c>false</c> otherwise.</returns>
		bool EsUnibleCon (IAnálisisCombate anal);

		float DañoDirecto { get; }

		float DañoDisperso { get; }
	}
}