using System.Collections.Generic;
using ListasExtra;
using Civ.Data.Import;
using System;

namespace Civ.Data
{
	/// <summary>
	/// Representa una clase de edificios. Para sólo lectura.
	/// </summary>
	public class EdificioRAW : IRequerimiento<ICiudad>, Civ.Debug.IPlainSerializable, IImportable
	{
		public string Nombre;
		public ulong MaxWorkers;
		/// <summary>
		/// Devuelve o establece el máximo número de instancias de este edificio por ciudad
		/// </summary>
		public int MaxPorCiudad = 1;
		/// <summary>
		/// Devuelve o establece el máximo número de instancias de este edificio por civilización
		/// Si vale 0, significa "sin límite"
		/// </summary>
		public int MaxPorCivilizacion;
		/// <summary>
		/// Devuelve o establece el máximo número de instancias de este edificio por mundo
		/// Si vale 0, significa "sin límite"
		/// </summary>
		public int MaxPorMundo;

		/// <summary>
		/// Devuelve los recursos y su cantidad que genera, incluso si no existe trabajador.
		/// </summary>
		public ListaPeso<Recurso> Salida { get; }

		public EdificioRAW ()
		{
			Salida = new ListaPeso<Recurso> ();
		}

		public override string ToString ()
		{
			return Nombre;
		}

		// IRequerieminto
		bool IRequerimiento<ICiudad>.LoSatisface (ICiudad ciudad)
		{
			return ciudad.ExisteEdificio (this);
		}

		// Requiere
		/// <summary>
		/// IRequerimientos necesarios para construir.
		/// </summary>        
		public Requerimiento Requiere = new Requerimiento ();
		// Construcción
		/// <summary>
		/// Lista de los recursos requeridos.
		/// </summary>
		public IDictionary<Recurso, float> ReqRecursos = new Dictionary<Recurso, float> ();
		//public List<Basic.Par<Recurso, float>> ReqRecursos = new List<Basic.Par<Recurso, float>>();
		/// <summary>
		/// Devuelve la lista de requerimientos
		/// </summary>
		/// <value>El IRequerimiento</value> 
		public ICollection<IRequerimiento<ICiudad>> Reqs ()
		{
			//List<IRequerimiento> ret = Basic.Covertidor<string, IRequerimiento>.ConvertirLista(Requiere, x => Global.g_.Data.EncuentraRequerimiento(x));
			return Requiere.Requiere ();
		}

		/// <summary>
		/// Especifica si este edificio se contruye automáticalente al cumplir todos los requisitos.
		/// </summary>
		public bool EsAutoConstruible;

		string Civ.Debug.IPlainSerializable.PlainSerialize (int tabs)
		{
			string tab = "";
			string ret;
			for (int i = 0; i < tabs; i++)
			{
				tab += "\t";
			}

			ret = tab + "(Edificio)" + Nombre + "\n";
			foreach (Civ.Debug.IPlainSerializable x in ReqRecursos.Keys)
			{
				ret += x.PlainSerialize (tabs + 1);
			}

			ret += ((Civ.Debug.IPlainSerializable)Requiere).PlainSerialize (tabs + 1);


			return ret;

		}

		#region IImportable

		C5.ArrayList<string []> _req_rec_id = new C5.ArrayList<string []> ();
		C5.ArrayList<string> _req_obj_id = new C5.ArrayList<string> ();
		C5.ArrayList<string []> _salida_id = new C5.ArrayList<string []> ();

		public void Importar (System.IO.StreamReader reader)
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
					case "max workers":
						MaxWorkers = ulong.Parse (spl [1]);
						break;
					case "max por ciudad":
						MaxPorCiudad = int.Parse (spl [1]);
						break;
					case "max por civil":
						MaxPorCivilizacion = int.Parse (spl [1]);
						break;
					case "max por mundo":
						MaxPorMundo = int.Parse (spl [1]);
						break;
					case "auto":
						EsAutoConstruible = true;
						break;
					case "requiere recurso":
						var a = new string[2];
						a [0] = spl [1];
						a [1] = spl [2];
						_req_rec_id.Add (a);
						break;
					case "requiere en ciudad":
						_req_obj_id.Add (spl [1]);
						break;
					case "salida":
						var a2 = new string[2];
						a2 [0] = spl [1];
						a2 [1] = spl [2];
						_salida_id.Add (a2);
						break;
				}
			}
		}

		void IImportable.Vincular ()
		{
			// Recursos consumidos:
			foreach (var x in _req_rec_id)
			{
				ReqRecursos.Add (
					ImportMachine.Valor (x [0]) as Recurso,
					float.Parse (x [1]));
			}
			// Req de ciudad
			foreach (var x in _req_obj_id)
			{
				Requiere.Add (ImportMachine.Valor (x));
			}
			// Salida
			foreach (var x in _salida_id)
			{
				Salida.Add (ImportMachine.Valor (x [0]) as Recurso, float.Parse (x [1]));
			}
			// limpiar
			_req_obj_id = null;
			_req_rec_id = null;
			_salida_id = null;
		}

		#endregion
	}
}