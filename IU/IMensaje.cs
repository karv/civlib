using System.Collections.Generic;

namespace Civ.IU
{

	/// <summary>
	/// Representa un mensaje al usuario
	/// </summary>
	public interface IMensaje
	{
		/// <summary>
		/// Devuelve el formato del mensaje
		/// </summary>
		/// <value>The msj.</value>
		string Msj { get; }

		/// <summary>
		/// Devuelve los argumentos del mensaje
		/// </summary>
		/// <value>The origen.</value>
		object [] Origen { get; }

		/// <summary>
		/// El tipo de repetidor
		/// </summary>
		/// <value>The tipo.</value>
		TipoRepetición Tipo { get; }

		/// <summary>
		/// Subtipo interno del repetidor.
		/// En la comparación, se usa su Equals (y hash) de objeto.
		/// </summary>
		/// <value>The subtipo.</value>
		object Subtipo { get; }

		/// <summary>
		/// Estado del mensaje
		/// </summary>
		/// <value>The estado.</value>
		EstadoMensaje Estado { get; set; }
	}
	
}