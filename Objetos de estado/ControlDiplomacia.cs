using System.Collections.Generic;

namespace Civ
{
	public class ControlDiplomacia : Dictionary<ICivilización, EstadoDiplomatico>, IDiplomacia
	{
		public bool PermiteAtacar (Armada arm)
		{
			EstadoDiplomatico dip;
			TryGetValue (arm.CivDueño, out dip);
			// Analysis disable ConstantNullCoalescingCondition
			return dip?.PermiteAtacar ?? false;
			// Analysis restore ConstantNullCoalescingCondition
		}

		public event System.EventHandler AlCambiarDiplomacia;
	}
}