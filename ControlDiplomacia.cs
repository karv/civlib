using System;
using System.Collections.Generic;

namespace Civ
{
	public class ControlDiplomacia : Dictionary<Civilizacion, EstadoDiplomatico>, IDiplomacia
	{
		public ControlDiplomacia()
		{
			
		}

		public bool PermiteAtacar(Armada arm)
		{
			EstadoDiplomatico dip;
			TryGetValue(arm.CivDueño, out dip);
			return dip?.PermiteAtacar ?? false;
		}
	}
}

