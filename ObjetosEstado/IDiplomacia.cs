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

		bool PermitePaso (Armada arm);

		event Action AlCambiarDiplomacia;
	}
}