using ListasExtra;
using Civ.Data.Import;
using System.Collections.Generic;
using System;

namespace Civ.Data
{
	/// <summary>
	/// Representa un trabajo en un edificioRAW
	/// </summary>	
	[Serializable]
	public class TrabajoRAW: Civ.Debug.IPlainSerializable, IImportable
	{
		public TrabajoRAW ()
		{
			EntradaBase = new ListaPeso<Recurso> ();
			SalidaBase = new ListaPeso<Recurso> ();
		}

		/// <summary>
		/// Nombre
		/// </summary>
		public string Nombre;
		/// <summary>
		/// EdificioRAW vinculado a este trabajo.
		/// </summary>
		public EdificioRAW Edificio;

		/// <summary>
		/// Recursos producidos por trabajador*turno (Base)
		/// </summary>
		public ListaPeso<Recurso> SalidaBase { get; }

		/// <summary>
		/// Recursos consumidos por trabajador*turno (Base)
		/// </summary>
		public ListaPeso<Recurso> EntradaBase { get; }

		public override string ToString ()
		{
			return string.Format ("{0} @ {1}", Nombre, Edificio);
		}
		// Requiere
		/// <summary>
		/// Lista de requerimientos.
		/// </summary>
		public Requerimiento Requiere = new Requerimiento ();

		/// <summary>
		/// Devuelve la lista de requerimientos.
		/// </summary>
		/// <value>El IRequerimiento</value> 
		public System.Collections.Generic.ICollection<IRequerimiento<ICiudad>> Reqs ()
		{
			return Requiere.Requiere ();
		}

		string Civ.Debug.IPlainSerializable.PlainSerialize (int tabs)
		{
			string tab = "";
			string ret;
			Civ.Debug.IPlainSerializable Ser;
			for (int i = 0; i < tabs; i++)
			{
				tab += "\t";
			}

			ret = tab + "(Trabajo)" + Nombre + "\n";

			Ser = Edificio;
			ret += Ser.PlainSerialize (tabs + 1);

			Ser = Requiere;
			ret += Ser.PlainSerialize (tabs + 1);

			return ret;

		}

		#region IImportable

		List <string []> _entrada_id = new List<string []> ();
		List <string []> _salida_id = new List<string []> ();
		List <string> _req_id = new List<string> ();
		string _edif_id;

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
					case "edificio":
						_edif_id = spl [1];
						break;
					case "entrada":
						var a = new string[2];
						a [0] = spl [1];
						a [1] = spl [2];
						_entrada_id.Add (a);
						break;
					case "salida":
						a = new string[2];
						a [0] = spl [1];
						a [1] = spl [2];
						_salida_id.Add (a);
						break;
					case "requiere":
						_req_id.Add (spl [1]);
						break;
				}
			}
		}

		void IImportable.Vincular ()
		{
			// Entrada y salida
			foreach (var x in _entrada_id)
			{
				EntradaBase.Add (
					ImportMachine.Valor (x [0]) as Recurso,
					float.Parse (x [1]));
			}
			foreach (var x in _salida_id)
			{
				SalidaBase.Add (
					ImportMachine.Valor (x [0]) as Recurso,
					float.Parse (x [1]));
			}
			// Req de ciudad
			foreach (var x in _req_id)
			{
				Requiere.Add (ImportMachine.Valor (x));
			}

			Edificio = ImportMachine.Valor (_edif_id) as EdificioRAW;

			// limpiar
			_entrada_id = null;
			_salida_id = null;
			_req_id = null;
			_edif_id = null;
		}

		#endregion
	}
}