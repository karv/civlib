using System;
using Civ.ObjetosEstado;


namespace Civ.Orden
{
	[Serializable]
	public class OrdenEstacionado : IOrden
	{
		public bool Ejecutar (TimeSpan t)
		{
			return false;
		}

		public Armada ArmadaEjecutante
		{
			get
			{
				return null;
			}
		}
	}
}