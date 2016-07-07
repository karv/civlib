using System;
using Civ.ObjetosEstado;

namespace Civ.RAW
{
	/// <summary>
	/// Un tipo de unidad que puede colonizar.
	/// </summary>
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
		event EventHandler AlColonizar;
	}
}

