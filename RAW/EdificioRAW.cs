using System.Collections.Generic;
using ListasExtra;
using System;
using Civ.ObjetosEstado;
using Civ.Global;

namespace Civ.RAW
{
	/// <summary>
	/// Representa una clase de edificios. Para sólo lectura.
	/// </summary>
	[Serializable]
	public class EdificioRAW : IRequerimiento<ICiudad>
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
	}
}