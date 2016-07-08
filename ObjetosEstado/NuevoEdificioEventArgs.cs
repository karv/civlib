using System.Collections.Generic;
using Civ.RAW;
using System;
using Civ.Topología;
using Civ.Almacén;

namespace Civ.ObjetosEstado
{
	[Serializable]
	public class NuevoEdificioEventArgs : EventArgs
	{
		public readonly Edificio Edificio;

		public NuevoEdificioEventArgs (Edificio edificio)
		{
			Edificio = edificio;
		}
	}
	
}