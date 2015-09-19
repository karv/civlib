using System;

namespace Civ
{
	/// <summary>
	/// Clase de diplomacia para civilizaciones nómadas
	/// </summary>
	public class DiplomaciaNómadas:IDiplomacia
	{
		public DiplomaciaNómadas()
		{
		}

		public bool PermiteAtacar(Armada arm)
		{
			return true;
		}
	}
}