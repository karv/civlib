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
		#region General

		/// <summary>
		/// Nombre del edificio
		/// </summary>
		public string Nombre;
		/// <summary>
		/// Número máximo de trabajadores
		/// </summary>
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
		/// Especifica si este edificio se contruye automáticalente al cumplir todos los requisitos.
		/// </summary>
		public bool EsAutoConstruible;

		#endregion

		#region Producción y trabajos

		/// <summary>
		/// Devuelve los recursos y su cantidad que genera, incluso si no existe trabajador.
		/// </summary>
		public ListaPeso<Recurso> Salida { get; }

		/// <summary>
		/// La lista de trabajos de este edificio
		/// </summary>
		public HashSet<TrabajoRAW> Trabajos { get; set; }

		#endregion

		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.RAW.EdificioRAW"/> class.
		/// </summary>
		public EdificioRAW ()
		{
			Salida = new ListaPeso<Recurso> ();
			Trabajos = new HashSet<TrabajoRAW> ();
		}

		#endregion

		#region General

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Civ.RAW.EdificioRAW"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Civ.RAW.EdificioRAW"/>.</returns>
		public override string ToString ()
		{
			return Nombre;
		}

		#endregion

		#region Requerimientos

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

		#endregion
	}
}