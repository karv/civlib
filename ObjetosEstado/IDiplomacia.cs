using System;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Interfaz para decidir cómo comportarse diplomáticamente
	/// </summary>
	public interface IDiplomacia
	{
		/// <summary>
		/// Si se le permite atacar a cierta armada.
		/// </summary>
		bool PermiteAtacar (Armada arm);

		/// <summary>
		/// Si se le permite el paso a través de una armada o ciudad de esta diplomacia a otra armada dada.
		/// </summary>
		/// <returns><c>true</c>, si se le permite el pasom <c>false</c> otherwise.</returns>
		/// <param name="arm">Armada extranjera</param>
		bool PermitePaso (Armada arm);

		/// <summary>
		/// Ocurre al cambiar las opcines de diplomacia.
		/// </summary>
		event Action AlCambiarDiplomacia;
	}
}