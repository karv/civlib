using Civ.ObjetosEstado;
using System;
using System.Collections.Generic;

namespace Civ.Combate
{
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