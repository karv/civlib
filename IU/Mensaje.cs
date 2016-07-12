using System.Collections.Generic;


namespace Civ.IU
{
	/// <summary>
	/// Representa un mensaje al usuario
	/// </summary>
	public class Mensaje : IMensaje
	{
		/// <summary>
		/// Igualador de mensajes
		/// </summary>
		public class Igualador : IEqualityComparer<IMensaje>
		{
			#region IEqualityComparer implementation

			/// <summary>
			/// Returns if two objects are equal.
			/// </summary>
			/// <param name="x">Object 0</param>
			/// <param name="y">Object 1</param>
			public bool Equals (IMensaje x, IMensaje y)
			{
				/* Devuelve true si y sólo si los tres se cumplen
				 * ninguno es null,
				 * .Tipo y .Subtipo coinciden
				 * ni .Tipo ni .Subtipo son igual a cero. 

				 * Pero siempre devuelve true si la referencia de ambos es la misma (y no nula)
				*/

				if (x == null || y == null)
					return false;
				if (ReferenceEquals (x, y))
					return true;
				if (x.Tipo == TipoRepetición.NoTipo || y.Tipo == TipoRepetición.NoTipo)
					return false;
				if (x.Tipo != y.Tipo)
					return false;
				if (x.Subtipo == null || y.Subtipo == null)
					return false;
				return x.Subtipo == y.Subtipo;
			}

			/// <Docs>The object for which the hash code is to be returned.</Docs>
			/// <para>Returns a hash code for the specified object.</para>
			/// <returns>A hash code for the specified object.</returns>
			/// <summary>
			/// Gets the hash code.
			/// </summary>
			/// <param name="obj">Object.</param>
			public int GetHashCode (IMensaje obj)
			{
				return (int)obj.Tipo + obj.Subtipo.GetHashCode ();
			}

			#endregion
		}

		#region General

		/// <summary>
		/// El tipo de repetidor
		/// </summary>
		public TipoRepetición Tipo { get; }

		/// <summary>
		/// Subtipo interno del repetidor.
		/// En la comparación, se usa su Equals (y hash) de objeto.
		/// </summary>
		/// <value>The subtipo.</value>
		public object Subtipo { get; }

		/// <summary>
		/// El texto mensaje.
		/// Se puede usar el formato de string.Format,
		/// </summary>
		public string Msj { get; }

		/// <summary>
		/// Objetos vinculados a este mensaje.
		/// Se usan como argumentos al usar ToString.
		/// </summary>
		public object [] Origen { get; }

		/// <summary>
		/// El estado de este mensaje
		/// </summary>
		/// <value>The estado.</value>
		public EstadoMensaje Estado { get; set; }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current.
		/// </summary>
		/// <returns>Devuelve el Msj con parámetros de formato de Origen</returns>
		public override string ToString ()
		{
			return string.Format (Msj, Origen);
		}

		#endregion

		#region ctor

		/// <summary>
		/// </summary>
		/// <param name="nMensaje">N mensaje.</param>
		/// <param name="tipo">Tipo.</param>
		/// <param name="subtipo">Subtipo.</param>
		/// <param name="nOrigen">N origen.</param>
		public Mensaje (
			string nMensaje,
			TipoRepetición tipo,
			object subtipo,
			params object [] nOrigen)
		{
			
			Msj = nMensaje;
			Origen = nOrigen;
			Tipo = tipo;
			Subtipo = subtipo;
			Estado = EstadoMensaje.NoLeído;
			#if DEBUG
			//System.Console.WriteLine ("+" + ToString ());
			#endif
		}

		#endregion
	}
}