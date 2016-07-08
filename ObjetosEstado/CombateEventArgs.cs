using ListasExtra;
using System;
using System.Collections.Generic;
using System.Linq;
using Civ.Combate;
using Civ.Orden;
using Civ.Global;
using Civ;
using Civ.RAW;
using Civ.Topología;
using Civ.IU;
using Civ.Almacén;

namespace Civ.ObjetosEstado
{

	[Serializable]
	public class CombateEventArgs : EventArgs
	{
		public IAnálisisCombate Análisis { get; }

		public CombateEventArgs (IAnálisisCombate anal)
		{
			Análisis = anal;
		}
	}
}