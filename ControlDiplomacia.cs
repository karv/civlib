using System.Collections.Generic;

namespace Civ
{
	public class ControlDiplomacia : Dictionary<ICivilizacion, EstadoDiplomatico>, IDiplomacia
	{
		public bool PermiteAtacar(Armada arm)
		{
			EstadoDiplomatico dip;
			TryGetValue(arm.CivDueño, out dip);
			// Analysis disable ConstantNullCoalescingCondition
			return dip?.PermiteAtacar ?? false;
			// Analysis restore ConstantNullCoalescingCondition
		}
	}
}

