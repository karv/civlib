using Civ.ObjetosEstado;
using System;
using System.Collections.Generic;

namespace Civ.Combate
{
	/// <summary>
	/// Representa el análisis, contexto o resultado de un tick de combate.
	/// </summary>
	public interface IAnálisisBatalla
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

		void Ejecutar ();

		/// <summary>
		/// Devuelve la duración del combate.
		/// </summary>
		TimeSpan Duración { get; }

		/// <summary>
		/// Une este análisis con otro.
		/// No modifica el otro.
		/// </summary>
		void UnirCon (IAnálisisBatalla anal);

		/// <summary>
		/// Revisa y devuelve un valor indicando si tiene sentido unir esta instancia con otra dada.
		/// </summary>
		/// <returns><c>true</c>, si es posible unir, <c>false</c> otherwise.</returns>
		bool EsUnibleCon (IAnálisisBatalla anal);

		/// <summary>
		/// Devuelve el daño directo causado
		/// </summary>
		/// <value>The daño directo.</value>
		float DañoDirecto { get; }

		/// <summary>
		/// Devuelve el daño disperso causado
		/// </summary>
		/// <value>The daño disperso.</value>
		float DañoDisperso { get; }
	}

	public interface IAnálisisCombate
	{
		string Análisis ();

		Armada ArmadaYo { get; }

		Armada ArmadaOtro { get; }

		/// <summary>
		/// Devuelve la duración de la batalla.
		/// </summary>
		TimeSpan Duración { get; }

		ICollection<IAnálisisBatalla> Batallas { get; }

		void Ejecutar ();

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
	}
}