using System;
using Civ.Global;
using Civ.Combate;
using Civ.Almacén;
using Civ.Topología;
using Civ.RAW;

namespace Civ.ObjetosEstado
{

	[Serializable]
	public sealed class CiudadEventArgs : EventArgs
	{
		public readonly ICiudad Ciudad;

		public CiudadEventArgs (ICiudad ciudad)
		{
			Ciudad = ciudad;
		}
		
	}
}