using System.Collections.Generic;


namespace Civ.IU
{
	public enum TipoRepetición
	{
		NoTipo = 0,
		ArmadaTerminaOrden,
		PerderPoblaciónOcupada,
		DesperdiciandoRecurso
	}

	/// <summary>
	/// Representa un mensaje al usuario
	/// </summary>
	public interface IMensaje
	{
		string Msj { get; }

		object [] Origen { get; }

		TipoRepetición Tipo { get; }

		object Subtipo { get; }

		EstadoMensaje Estado { get; set; }
	}

	/// <summary>
	/// Representa un mensaje al usuario
	/// </summary>
	public class Mensaje : IMensaje
	{
		public class Igualador : IEqualityComparer<IMensaje>
		{
			#region IEqualityComparer implementation

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

			public int GetHashCode (IMensaje obj)
			{
				return (int)obj.Tipo + obj.Subtipo.GetHashCode ();
			}

			#endregion
		}

		#region General

		public TipoRepetición Tipo { get; }

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
		/// <param name="nMensaje">Texto</param>
		/// <param name="nOrigen">Objeto vinculado</param>
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