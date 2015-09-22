using System;
using Global;

namespace Civ.Barbaros
{
	/// <summary>
	/// Dice cuándo y qué tipo de armadas generar
	/// </summary>
	public interface IReglaGeneracion
	{
		/// <summary>
		/// Revisa si se debe generar esta clase de armada
		/// </summary>
		/// <param name="Estado">Estado del juego.</param>
		bool EsPosibleGenerar(g_State Estado);

		/// <summary>
		/// Genera una armada
		/// </summary>
		Armada GenerarArmada();
	}
}

