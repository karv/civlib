using System;
using System.IO;
using Civ.Global;
using Civ.Debug;
using System.Runtime.Serialization;

namespace Civ.RAW
{
	[Serializable]
	public class Recurso : IPlainSerializable, IEquatable<Recurso>
	{
		public override string ToString ()
		{
			return Nombre;
		}

		#region IEquatable implementation

		bool IEquatable<Recurso>.Equals (Recurso other)
		{
			return ReferenceEquals (this, other);
		}

		#endregion

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
		/// Initializes a new instance of the <see cref="Civ.Data.Recurso"/>class.
		/// </summary>
		/// <param name="nombre">Nombre del recurso.</param>
		public Recurso (string nombre)
		{
			Nombre = nombre;
		}

		public Recurso ()
		{
		}

		public string Img;

		string IPlainSerializable.PlainSerialize (int tabs)
		{
			string tab = "";
			string ret;
			for (int i = 0; i < tabs; i++)
			{
				tab += "\t";
			}

			ret = tab + "(Recurso)" + Nombre + "\n";

			foreach (var x in Juego.Data.Trabajos)
			{
				// ¿Agregar?
				bool Agregar = false;
				foreach (var y in x.SalidaBase.Keys)
				{
					if (y == this)
					{
						Agregar = true;
						break;
					}
				}

				// Si este trabajo produce Robj
				if (Agregar)
				{
					Debug.IPlainSerializable Ser = x;
					ret += Ser.PlainSerialize (tabs + 1);
				}
			}

			return ret;
		}

		public float Puntuación
		{
			get
			{
				return Valor;
			}
		}
	}
}