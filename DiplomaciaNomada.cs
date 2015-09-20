using System;

namespace Civ
{
	/// <summary>
	/// Clase de diplomacia para civilizaciones nómadas
	/// </summary>
	public class DiplomaciaNomada:IDiplomacia
	{
		public DiplomaciaNomada()
		{
		}

		public bool PermiteAtacar(Armada arm)
		{
			return true;
		}
	}
}