using Civ.ObjetosEstado;
using System;

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

		/// <summary>
		/// Ejecuta este análisis,
		/// haciendo que tenga efecto en en stack defensor.
		/// </summary>
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

		/// <summary>
		/// Devuelve la cantidad de unidades en el stack defensor
		/// </summary>
		long CantidadInicialDef { get; }

		/// <summary>
		/// Devuelve la cantidad de unidades del atacante
		/// </summary>
		long CantidadInicialAtt { get; }

		/// <summary>
		/// Devuelve la cantidad final (o progresivo) de unidades del defensa
		/// </summary>
		long CantidadFinalDef { get; }

		/// <summary>
		/// Devuelve la cantidad final (o progresivo) de unidades del atacante
		/// </summary>
		long CantidadFinalAtt { get; }
	}
	
}