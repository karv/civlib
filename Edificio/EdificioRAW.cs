using System;
using System.Collections.Generic;

namespace Civ
{
	[Serializable()]
	/// <summary>
	/// Representa una clase de edificios. Para sólo lectura.
	/// </summary>
	public class EdificioRAW : IRequerimiento
	{
		public string Nombre;
		public ulong MaxWorkers;

		public override string ToString()
		{
			return Nombre;
		}

		public EdificioRAW ()
		{

		}

		// IRequerieminto
		bool Civ.IRequerimiento.LoSatisface (Ciudad C){
			return C.ExisteEdificio(this);
		}
		string Civ.IRequerimiento.ObtenerId()
		{
			return Nombre;
		}

		// Requiere
		/// <summary>
		/// IRequerimientos necesarios para construir.
		/// </summary>
		public List<String> Requiere = new List<string>();

		// Construcción
		/// <summary>
		/// Lista de nombres de sus IRequerimientos.
		/// </summary>
		//public ListasExtra.ListaPeso<string> Requiere = new ListasExtra.ListaPeso<string> ();
		//public List<string> Requiere = new List<string>();
		public List<Basic.Par<string, float>> ReqRecursos = new List<Basic.Par<string, float>>();

		/// <summary>
		/// Devuelve la lista de requerimientos
		/// </summary>
		/// <value>El IRequerimiento</value> 
		public List<IRequerimiento> Reqs ()
		{
			List<IRequerimiento> ret = Basic.Covertidor<string, IRequerimiento>.ConvertirLista(Requiere, x => Global.g_.Data.EncuentraRequerimiento(x));
			return ret;
		}

	}

}

