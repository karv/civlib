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
		/// Se puede usar el formato de <see cref="string.Format"/>,
		/// </summary>
		public string Msj;
		/// <summary>
		/// Objetos vinculados a este mensaje.
		/// Se usan como argumentos al usar ToString.
		/// </summary>
		public object[] Origen;

		/// <summary>
		/// Initializes a new instance of the <see cref="IU.Mensaje"/> class.
		/// </summary>
		/// <param name="nMensaje">Texto</param>
		/// <param name="nOrigen">Objeto vinculado</param>
		public Mensaje(string nMensaje, params object[] nOrigen)
		{
			Msj = nMensaje;
			Origen = nOrigen;
		}

		public override string ToString()
		{
			return string.Format(Msj, Origen);
		}
	}
}

