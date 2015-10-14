using System.Collections.Generic;
using System;

namespace Civ
{
	public class ControlDiplomacia : Dictionary<ICivilización, EstadoDiplomático>, IDiplomacia
	{
		public bool PermiteAtacar (Armada arm)
		{
			EstadoDiplomático dip;
			return TryGetValue (arm.CivDueño, out dip) && dip.PermiteAtacar;
		}

		//TODO Por ahora no se puede ya que Dicionary no tiene métodos para monitorear cambios
		event EventHandler IDiplomacia.AlCambiarDiplomacia
		{
			add
			{
			}
			remove
			{
			}
		}
	}
}