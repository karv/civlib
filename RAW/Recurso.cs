using System;
using System.Runtime.Serialization;

namespace Civ.Data
{
	[DataContract (IsReference = true)]
	public class Recurso : Civ.Debug.IPlainSerializable, IEquatable<Recurso>
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
		[DataMember (Name = "Desaparece")]
		public bool Desaparece;
		/// <summary>
		/// ¿El recurso es científico?
		/// </summary>
		[DataMember (Name = "Científico")]
		public bool EsCientifico;
		/// <summary>
		/// Nombre del recurso.
		/// </summary>
		[DataMember (Name = "Nombre")]
		public string Nombre;
		/// <summary>
		/// Devuelve o establece si el recurso es global. De ser false, se almacena en cada ciudad por separado.
		/// De ser true, cada ciudad puede tomar de un almacén global.
		/// </summary>
		[DataMember (Name = "Global")]
		public bool EsGlobal;

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

		[DataMember (Name = "Imagen")]
		public string Img;

		string Civ.Debug.IPlainSerializable.PlainSerialize (int tabs)
		{
			string tab = "";
			string ret;
			for (int i = 0; i < tabs; i++)
			{
				tab += "\t";
			}

			ret = tab + "(Recurso)" + Nombre + "\n";

			foreach (var x in Global.Juego.Data.Trabajos)
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
					Civ.Debug.IPlainSerializable Ser = x;
					ret += Ser.PlainSerialize (tabs + 1);
				}
			}

			return ret;
		}
	}
}