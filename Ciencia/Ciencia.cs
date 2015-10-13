using System;
using System.Runtime.Serialization;
using C5;
using Civ.Data.Import;

namespace Civ.Data
{
	/// <summary>
	/// Representa un adelanto científico.
	/// </summary>
	[DataContract (IsReference = true)]
	public class Ciencia : IRequerimiento<ICiudad>, Civ.Debug.IPlainSerializable, IImportable
	{
		[DataContract (IsReference = false)]
		public class Requerimiento
		{
			[DataMember (Name = "Recurso")]
			readonly RequiereCiencia _Recursos = new RequiereCiencia ();

			/// <summary>
			/// Devuelve la lista de recursos que se necesita para investigar
			/// </summary>
			/// <value>The recursos.</value>
			public RequiereCiencia Recursos
			{
				get
				{
					return _Recursos;
				}
			}

			/// <summary>
			/// Lista de requisitos científicos.
			/// </summary>
			[DataMember (Name = "Ciencias")]
			public ICollection< Ciencia> Ciencias = new HashSet<Ciencia> ();
			// Se debe convertir en GuardedCollection cuando se lea.
		}

		/// <summary>
		/// Nombre de la ciencia;
		/// </summary>
		[DataMember (Name = "Nombre")]
		public String Nombre { get; set; }

		public override string ToString ()
		{
			return Nombre;
		}

		// Sobre los requerimientos.
		/// <summary>
		/// Requerimientos para poder aprender este avance.
		/// </summary>
		[DataMember (Name = "Requiere")]
		public Requerimiento Reqs = new Requerimiento ();


		#region IRequerimiento

		bool IRequerimiento<ICiudad>.LoSatisface (ICiudad ciudad)
		{
			return ciudad.CivDueño.Avances.Contains (this);
		}

		#endregion

		#region IImportable

		ArrayList <string []> _reqRecurso_id = new ArrayList<string []> ();
		ArrayList <string> _reqCiencia_id = new ArrayList<string> ();

		void IImportable.Importar (System.IO.StreamReader reader)
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
					case "reqrecurso":
						var a = new string[2];
						a [0] = spl [1];
						a [1] = spl [2];
						_reqRecurso_id.Add (a);
						break;
					case "reqciencia":
						_reqCiencia_id.Add (spl [1]);
						break;
				}
			}
		}

		void IImportable.Vincular ()
		{
			// Vincular recursos requeridos
			foreach (var x in _reqRecurso_id)
			{
				Reqs.Recursos.Add (
					ImportMachine.Valor (x [0]) as Recurso,
					float.Parse (x [1]));
			}

			// Vincular ciencias
			foreach (var x in _reqCiencia_id)
			{
				Reqs.Ciencias.Add (ImportMachine.Valor (x) as Ciencia);
			}

			// Limpiar
			_reqCiencia_id = null;
			_reqRecurso_id = null;
		}

		#endregion

		#region PlainSerializable

		string Civ.Debug.IPlainSerializable.PlainSerialize (int tabs)
		{
			string tab = "";
			string ret;
			for (int i = 0; i < tabs; i++)
			{
				tab += "\t";
			}
			ret = tab + "(Ciencia)" + Nombre + "\n";

			foreach (Ciencia x in Reqs.Ciencias)
			{
				Civ.Debug.IPlainSerializable Ser = x;
				ret += Ser.PlainSerialize (tabs + 1);
			}

			foreach (var x in Reqs.Recursos.Keys)
			{
				Civ.Debug.IPlainSerializable Ser = x;
				ret += Ser.PlainSerialize (tabs + 1);
			}
			return ret;
		}

		#endregion
	}
}
