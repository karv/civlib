using System;
using System.Collections.Generic;
using Civ.ObjetosEstado;
using Civ.Global;

namespace Civ.Ciencias
{

	[Serializable]
	public sealed class AvanceEventArgs : EventArgs
	{
		public readonly Ciencia Ciencia;
		public readonly ICivilización Civil;

		public AvanceEventArgs (Ciencia ciencia, ICivilización civil)
		{
			Ciencia = ciencia;
			Civil = civil;
		}
	}
}