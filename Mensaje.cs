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
		public object [] Origen;

		/// <summary>
		/// Se usa para comparar si dos mensajes son de la misma clase y no deben duplicarse.
		/// </summary>
		public object VerificadorRepetición;

		/// <summary>
		/// Initializes a new instance of the <see cref="IU.Mensaje"/> class.
		/// </summary>
		/// <param name="nMensaje">Texto</param>
		/// <param name="nOrigen">Objeto vinculado</param>
		/// <param name="repetidor">Objeto para verificar si es un mensaje repetido.</param>
		public Mensaje (string nMensaje, object repetidor, params object [] nOrigen)
		{
			Msj = nMensaje;
			Origen = nOrigen;
			VerificadorRepetición = repetidor;
			#if DEBUG
			System.Console.WriteLine ("+" + ToString ());
			#endif
		}

		public override string ToString ()
		{
			return string.Format (Msj, Origen);
		}
	}
}