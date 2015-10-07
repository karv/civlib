using System;

namespace Civ
{
	[Serializable]
	public class TickEventArgs : EventArgs
	{
		public TimeSpan Tiempo{ get; }

		public TickEventArgs (TimeSpan tiempo)
		{
			Tiempo = tiempo;
		}
	}
}

