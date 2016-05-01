using System;
using Civ.Data.Import;
using System.Collections.Generic;
using Civ.ObjetosEstado;
using Civ.Debug;
using Civ.Global;
using Civ.RAW;

namespace Civ.Ciencias
{
	/// <summary>
	/// Representa un adelanto científico.
	/// </summary>
	[Serializable]
	public class Ciencia : IRequerimiento<ICiudad>, IPlainSerializable, IImportable
	{
		[Serializable]
		public class Requerimiento
		{
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
			public ICollection< Ciencia> Ciencias = new HashSet<Ciencia> ();
			// Se debe convertir en GuardedCollection cuando se lea.
		}

		/// <summary>
		/// Nombre de la ciencia;
		/// </summary>
		public String Nombre { get; set; }

		public override string ToString ()
		{
			return Nombre;
		}

		// Sobre los requerimientos.
		/// <summary>
		/// Requerimientos para poder aprender este avance.
		/// </summary>
		public Requerimiento Reqs = new Requerimiento ();


		#region IRequerimiento

		bool IRequerimiento<ICiudad>.LoSatisface (ICiudad ciudad)
		{
			return ciudad.CivDueño.Avances.Contains (this);
		}

		#endregion

		#region IImportable

		List <string []> _reqRecurso_id = new List<string []> ();
		List <string> _reqCiencia_id = new List<string> ();

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
						if (spl.Length < 2)
						{
							Console.WriteLine ("Hhuehue");
						}
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
				Console.WriteLine (string.Format (
					"Vinculando requerimiento {0} a {1}",
					x,
					Nombre));
				Reqs.Ciencias.Add (ImportMachine.Valor (x) as Ciencia);
			}

			// Limpiar
			_reqCiencia_id = null;
			_reqRecurso_id = null;
		}

		#endregion

		#region PlainSerializable

		string IPlainSerializable.PlainSerialize (int tabs)
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
				IPlainSerializable Ser = x;
				ret += Ser.PlainSerialize (tabs + 1);
			}

			foreach (var x in Reqs.Recursos.Keys)
			{
				IPlainSerializable Ser = x;
				ret += Ser.PlainSerialize (tabs + 1);
			}
			return ret;
		}

		#endregion
	}
}