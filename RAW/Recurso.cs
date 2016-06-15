using System;
using System.Runtime.Serialization;

namespace Civ.RAW
{
	/// <summary>
	/// Representa un recurso
	/// </summary>
	[Serializable]
	public class Recurso : IEquatable<Recurso>
	{
		#region IEquatable implementation

		bool IEquatable<Recurso>.Equals (Recurso other)
		{
			return ReferenceEquals (this, other);
		}

		#endregion

		#region General

		/// <summary>
		/// Desaparece al final del turno.
		/// </summary>
		public bool Desaparece;
		/// <summary>
		/// ¿El recurso es científico?
		/// </summary>
		public bool EsCientifico;
		/// <summary>
		/// Nombre del recurso.
		/// </summary>
		public string Nombre;
		/// <summary>
		/// Devuelve o establece si el recurso es global. De ser false, se almacena en cada ciudad por separado.
		/// De ser true, cada ciudad puede tomar de un almacén global.
		/// </summary>
		public bool EsGlobal;
		/// <summary>
		/// El valor del recurso,
		/// útil para la IA
		/// </summary>
		[DataMember]
		public float Valor;
		/// <summary>
		/// Si este recurso se puede almacenar en Ecología
		/// </summary>
		public bool EsEcológico;

		/// <summary>
		/// Devuelve la puntuación que suma a una ciudad cada unidad de este recurso al estar en su almacén
		/// </summary>
		public float Puntuación
		{
			get
			{
				return Valor;
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Civ.RAW.Recurso"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Civ.RAW.Recurso"/>.</returns>
		public override string ToString ()
		{
			return Nombre;
		}

		#endregion

		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.RAW.Recurso"/>class.
		/// </summary>
		/// <param name="nombre">Nombre del recurso.</param>
		public Recurso (string nombre)
		{
			Nombre = nombre;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.RAW.Recurso"/> class.
		/// </summary>
		public Recurso ()
		{
		}

		#endregion

		#region Otros

		/// <summary>
		/// Nombre del archivo de la imagen del recurso.
		/// Relativo al directorio img.
		/// </summary>
		public string Img;

		#endregion
	}
}