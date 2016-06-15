using System.Collections.Generic;

namespace Civ.RAW
{
	/// <summary>
	/// Un tipo de unidad que puede atacar
	/// </summary>
	public interface IUnidadRAWCombate : IUnidadRAW
	{
		/// <summary>
		/// Devuevle el modificador de daño a los que conservan una flag dada.
		/// </summary>
		/// <returns>Devuelve el modificador de fuerza</returns>
		/// <param name="modificador">El flag a comparar</param>
		float getModificador (string modificador);

		/// <summary>
		/// Devuelve la lista de mopdificadores.
		/// </summary>
		/// <value>The modificadores.</value>
		IEnumerable<string> Modificadores { get; }

		/// <summary>
		/// Dispersión del daño
		/// </summary>
		/// <value>The dispersion.</value>
		/// <remarks>Un valor alto (=1)hace que ataque a cada unidad de un stack objetivo con la misma potencia.
		/// Uno bajo (=0) hace que ataque unidad por unidad. </remarks>
		float Dispersión { get; }

		/// <summary>
		/// Devuelve el daño que puede causar por unidad tiempo
		/// </summary>
		float Ataque { get; }
	}
}