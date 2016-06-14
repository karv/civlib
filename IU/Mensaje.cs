namespace Civ.IU
{
	/// <summary>
	/// Representa un mensaje al usuario
	/// </summary>
	public class Mensaje
	{
		#region General

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
		/// El estado de este mensaje
		/// </summary>
		/// <value>The estado.</value>
		public EstadoMensaje Estado { get; set; }

		public override string ToString ()
		{
			return string.Format (Msj, Origen);
		}

		#endregion

		#region Repetición

		/// <summary>
		/// Se usa para comparar si dos mensajes son de la misma clase y no deben duplicarse.
		/// </summary>
		public IRepetidor VerificadorRepetición;

		#endregion

		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="IU.Mensaje"/> class.
		/// </summary>
		/// <param name="nMensaje">Texto</param>
		/// <param name="nOrigen">Objeto vinculado</param>
		/// <param name="repetidor">Objeto para verificar si es un mensaje repetido.</param>
		public Mensaje (string nMensaje,
		                IRepetidor repetidor,
		                params object [] nOrigen)
		{
			Msj = nMensaje;
			Origen = nOrigen;
			VerificadorRepetición = repetidor;
			Estado = EstadoMensaje.NoLeído;
			#if DEBUG
			//System.Console.WriteLine ("+" + ToString ());
			#endif
		}

		#endregion
	}

	/// <summary>
	/// El estado de un mensaje
	/// </summary>
	public enum EstadoMensaje
	{
		NoLeído,
		Leído
	}
}