using System;

namespace IU
{
	/// <summary>
	/// Representa un mensaje al usuario
	/// </summary>
	public class Mensaje
	{
		/// <summary>
		/// El texto mensaje.
		/// </summary>
		public string Msj;
		/// <summary>
		/// Objeto vinculado a este mensaje.
		/// </summary>
		public object Origen;

		/// <summary>
		/// Initializes a new instance of the <see cref="IU.Mensaje"/> class.
		/// </summary>
		/// <param name="nMensaje">Texto</param>
		/// <param name="nOrigen">Objeto vinculado</param>
		public Mensaje(string nMensaje, object nOrigen)
		{
			Msj = nMensaje;
			Origen = nOrigen;
		}

		public override string ToString()
		{
			return Mensaje;
		}
	}
}

