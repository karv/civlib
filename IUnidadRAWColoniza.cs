﻿using System;

namespace Civ
{
	public interface IUnidadRAWColoniza : IUnidadRAW
	{
		/// <summary>
		/// Coloniza aquí
		/// </summary>
		/// <returns>Devuelve la ciudad que colonizó</returns>
		ICiudad Coloniza (Stack stack);

		/// <summary>
		/// Revisa si puede colonizar en este momento
		/// </summary>
		/// <value><c>true</c> si puede colonizar; si no, <c>false</c>.</value>
		bool PuedeColonizar (Stack stack);

		/// <summary>
		/// Ocurre cuando esta unidad coloniza
		/// </summary>
		event Action<ICiudad> AlColonizar;
	}
}

