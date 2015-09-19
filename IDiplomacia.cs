using System;
using Civ;

namespace Civ
{
	/// <summary>
	/// Interfaz para decidir cómo comportarse diplomáticamente
	/// </summary>
	public interface IDiplomacia
	{
		bool PermiteAtacar(Armada arm);
	}
		
}

