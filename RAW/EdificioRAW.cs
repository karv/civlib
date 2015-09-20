using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Civ
{
	/// <summary>
	/// Representa una clase de edificios. Para sólo lectura.
	/// </summary>
	[DataContract(IsReference = true, Name = "Edificio")]
	public class EdificioRAW : IRequerimiento<ICiudad>, CivLibrary.Debug.IPlainSerializable
	{
		[DataMember]
		public string Nombre;
		[DataMember]
		public ulong MaxWorkers;
		/// <summary>
		/// Devuelve o establece el máximo número de instancias de este edificio por ciudad
		/// </summary>
		[DataMember]
		public int MaxPorCiudad = 1;
		/// <summary>
		/// Devuelve o establece el máximo número de instancias de este edificio por civilización
		/// Si vale 0, significa "sin límite"
		/// </summary>
		[DataMember]
		public int MaxPorCivilizacion = 0;
		/// <summary>
		/// Devuelve o establece el máximo número de instancias de este edificio por mundo
		/// Si vale 0, significa "sin límite"
		/// </summary>
		[DataMember]
		public int MaxPorMundo = 0;
		[DataMember(Name = "Producción")]
		public TrabajoRAW.DiferenciaRecursos _Salida = new TrabajoRAW.DiferenciaRecursos();

		/// <summary>
		/// Devuelve los recursos y su cantidad que genera, incluso si no existe trabajador.
		/// </summary>
		public TrabajoRAW.DiferenciaRecursos Salida
		{
			get { return _Salida; }
		}

		public override string ToString()
		{
			return Nombre;
		}

		public EdificioRAW()
		{
		}
		// IRequerieminto
		bool Civ.IRequerimiento<ICiudad>.LoSatisface(ICiudad C)
		{
			return C.ExisteEdificio(this);
		}
		// Requiere
		/*  //TODO: Borrar si es que funciona

		public System.Collections.Generic.List<IRequerimiento> Requiere()
		{
			List<IRequerimiento> ret = new List<IRequerimiento>();
			foreach (Ciencia x in _ReqCiencia)      { ret.Add(x); }
			foreach (EdificioRAW x in _ReqEdificio) { ret.Add(x); }

			return ret;
		}

		[DataMember (Name="ReqCiencias")]
		public List<Ciencia> _ReqCiencia = new List<Ciencia>();

		[DataMember(Name = "ReqEdificios")]
		public List<EdificioRAW> _ReqEdificio = new List<EdificioRAW>();
		*/
		/// <summary>
		/// IRequerimientos necesarios para construir.
		/// </summary>        
		[DataMember]
		public Requerimiento Requiere = new Requerimiento();
		// Construcción
		/// <summary>
		/// Lista de los recursos requeridos.
		/// </summary>
		[DataMember(Name = "Construcción")]
		public Dictionary<Recurso, float> ReqRecursos = new Dictionary<Recurso, float>();
		//public List<Basic.Par<Recurso, float>> ReqRecursos = new List<Basic.Par<Recurso, float>>();
		/// <summary>
		/// Devuelve la lista de requerimientos
		/// </summary>
		/// <value>El IRequerimiento</value> 
		public List<IRequerimiento<ICiudad>> Reqs()
		{
			//List<IRequerimiento> ret = Basic.Covertidor<string, IRequerimiento>.ConvertirLista(Requiere, x => Global.g_.Data.EncuentraRequerimiento(x));
			return Requiere.Requiere();
		}

		/// <summary>
		/// Especifica si este edificio se contruye automáticalente al cumplir todos los requisitos.
		/// </summary>
		[DataMember(Name = "EsAutoConstruíble")]
		public bool EsAutoConstruible;

		string CivLibrary.Debug.IPlainSerializable.PlainSerialize(int tabs)
		{
			string tab = "";
			string ret;
			for (int i = 0; i < tabs; i++)
			{
				tab += "\t";
			}

			ret = tab + "(Edificio)" + Nombre + "\n";
			foreach (CivLibrary.Debug.IPlainSerializable x in ReqRecursos.Keys)
			{
				ret += x.PlainSerialize(tabs + 1);
			}

			ret += ((CivLibrary.Debug.IPlainSerializable)Requiere).PlainSerialize(tabs + 1);


			return ret;

		}

		#region Defaults

		[OnDeserializing]
		void OnDeserializing(StreamingContext context)
		{
			SetDefaults();
		}

		void SetDefaults()
		{
			MaxPorCiudad = 1;
			MaxPorCivilizacion = 0;
			MaxPorMundo = 0;
		}

		#endregion
	}
}