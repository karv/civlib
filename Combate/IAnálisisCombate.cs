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
	}

	/// <summary>
	/// Un análisis de combate:
	/// representa el estado de toda una batalla entre dos <see cref="Civ.ObjetosEstado.Armada"/>
	/// </summary>
	public interface IAnálisisCombate
	{
		/// <summary>
		/// Devuelve un <see cref="System.String"/> que representa el resultado (posiblemente parcial)de este combate.
		/// </summary>
		string Análisis ();

		/// <summary>
		/// Armada original.
		/// La civilización de esta armada es la que está vinculada al manejador de análisis.
		/// </summary>
		Armada ArmadaYo { get; }

		/// <summary>
		/// La otra armada implicada en el combate
		/// </summary>
		Armada ArmadaOtro { get; }

		/// <summary>
		/// Devuelve la duración de la batalla.
		/// </summary>
		TimeSpan Duración { get; }

		/// <summary>
		/// Devuelve la colección de análisis de batallas que forman este combate.
		/// </summary>
		ICollection<IAnálisisBatalla> Batallas { get; }

		/// <summary>
		/// Ejecuta este combate. Haciendo que tenga efecto.
		/// </summary>
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