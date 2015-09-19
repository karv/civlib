using System;

namespace Civ
{
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

