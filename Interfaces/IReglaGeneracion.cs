using Global;

namespace Civ.Bárbaros
{
	/// <summary>
	/// Dice cuándo y qué tipo de armadas generar
	/// </summary>
	public interface IReglaGeneracion
	{
		/// <summary>
		/// Revisa si se debe generar esta clase de armada
		/// </summary>
		/// <param name="estado">Estado del juego.</param>
		bool EsPosibleGenerar (GameState estado);

		/// <summary>
		/// Genera una armada
		/// </summary>
		Armada GenerarArmada ();
	}
}

