using System.Collections.Generic;
using Civ.RAW;
using System;
using Civ.Topología;
using Civ.Almacén;

namespace Civ.ObjetosEstado
{
	[Serializable]
	public class ReclutarEventArgs : EventArgs
	{
		public readonly IUnidadRAW Tipo;

		public readonly ulong Cantidad;

		public ReclutarEventArgs (IUnidadRAW tipo, ulong cantidad)
		{
			Tipo = tipo;
			Cantidad = cantidad;
		}
	}
	
}