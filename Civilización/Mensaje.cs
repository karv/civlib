using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Civ
{
	public partial class Civilizacion
	{
		/// <summary>
		/// Lista de mensajes de eventos para el usuario.
		/// </summary>
		System.Collections.Queue Mensajes = new System.Collections.Queue();

		/// <summary>
		/// Agrega un mensaje de usuario a la cola.
		/// </summary>
		/// <param name="Mens">Mensaje</param>
		public void AgregaMensaje (IU.Mensaje Mens)
		{
			Mensajes.Enqueue(Mens);
		}

		/// <summary>
		/// Agrega un mensaje de usuario a la cola.
		/// </summary>
		/// <param name="str">Cadena de texto, con formato de string.Format</param>
		/// <param name="Ref">Referencias u orígenes del mensaje.</param>
		public void AgregaMensaje(string str, params object[] Ref)
		{
			AgregaMensaje(new IU.Mensaje(str, Ref));
			if (OnNuevoMensaje != null) OnNuevoMensaje.Invoke();
		}
		/// <summary>
		/// Devuelve <c>true</c> sólo si existe algún mensaje.
		/// </summary>
		public bool ExisteMensaje
		{
			get
			{
				return Mensajes.Count > 0;
			}
		}
		/// <summary>
		/// Toma de la cola el siguiente mensaje.
		/// </summary>
		/// <returns>Devuelve el mensaje siguiente en la cola.</returns>
		public IU.Mensaje SiguitenteMensaje ()
		{
			if (ExisteMensaje)
			{
				IU.Mensaje ret = (IU.Mensaje)Mensajes.Dequeue();
				return ret;
			}
			else return null;
		}

		public event Action OnNuevoMensaje;

	}
}
