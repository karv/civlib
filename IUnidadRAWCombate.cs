﻿using System.Collections.Generic;

namespace Civ
{
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
		float Dispersion { get; }

		/// <summary>
		/// Fuerza de combate
		/// </summary>
		/// <value>La fuerza de combate</value>
		float Fuerza { get; }
	}
}