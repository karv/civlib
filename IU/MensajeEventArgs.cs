using System.Runtime.InteropServices;

namespace Civ.IU
{

	[System.Serializable]
	public sealed class MensajeEventArgs : System.EventArgs
	{
		public Mensaje Msj { get; }

		public ManejadorMensajes Manager { get; }

		public MensajeEventArgs (Mensaje msj, ManejadorMensajes man)
		{
			Msj = msj;
			Manager = man;
		}
	}
}