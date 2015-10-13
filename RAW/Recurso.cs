using System;
using System.Runtime.Serialization;
using Civ.Data.Import;
using System.IO;
using Global;

namespace Civ.Data
{
	[DataContract (IsReference = true)]
	public class Recurso : Civ.Debug.IPlainSerializable, IEquatable<Recurso>, IImportable
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

		#region Importable

		void IImportable.Importar (StreamReader reader)
		{			
			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine ();
				line.ToLower ();
				var spl = line.Split (':');
				for (int i = 0; i < spl.Length; i++)
				{
					spl [i] = spl [i].Trim ();
				}

				switch (spl [0])
				{
					case "nombre":
						Nombre = spl [1];
						break;
					case "desaparece":
						Desaparece = spl [1] != "0";
						break;
					case "científico":
						EsCientifico = spl [1] != "0";
						break;
					case "global":
						EsGlobal = spl [1] != "0";
						break;
					case "alimento":
						#if DEBUG
						if (Juego.Data.RecursoAlimento != null)
							Console.WriteLine (string.Format (
								"se están definiendo varios recursos de alimento: {0}, {1}",
								Juego.Data.RecursoAlimento,
								this));
						#endif
						Juego.Data.RecursoAlimento = this;
						break;
				}
			}
		}

		void IImportable.Vincular ()
		{
		}

		#endregion

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