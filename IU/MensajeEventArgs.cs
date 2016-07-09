
namespace Civ.IU
{
	/// <summary>
	/// Argumentos para eventos de mensaje
	/// </summary>
	[System.Serializable]
	public sealed class MensajeEventArgs : System.EventArgs
	{
		/// <summary>
		/// Devuelve el mensaje.
		/// </summary>
		/// <value>The msj.</value>
		public IMensaje Msj { get; }

		/// <summary>
		/// Devuelve el manejador de mensaje involucrado.
		/// </summary>
		/// <value>The manager.</value>
		public ManejadorMensajes Manager { get; }

		/// <param name="msj">Msj.</param>
		/// <param name="man">Man.</param>
		public MensajeEventArgs (IMensaje msj, ManejadorMensajes man)
		{
			Msj = msj;
			Manager = man;
		}
	}
}