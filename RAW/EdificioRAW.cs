using System.Collections.Generic;
using System.Runtime.Serialization;
using ListasExtra;

namespace Civ
{
	/// <summary>
	/// Representa una clase de edificios. Para sólo lectura.
	/// </summary>
	[DataContract (IsReference = true, Name = "Edificio")]
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
		public int MaxPorCiudad;
		/// <summary>
		/// Devuelve o establece el máximo número de instancias de este edificio por civilización
		/// Si vale 0, significa "sin límite"
		/// </summary>
		[DataMember]
		public int MaxPorCivilizacion;
		/// <summary>
		/// Devuelve o establece el máximo número de instancias de este edificio por mundo
		/// Si vale 0, significa "sin límite"
		/// </summary>
		[DataMember]
		public int MaxPorMundo;

		/// <summary>
		/// Devuelve los recursos y su cantidad que genera, incluso si no existe trabajador.
		/// </summary>
		[DataMember (Name = "Producción")]
		public ListaPeso<Recurso> Salida { get; }

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
		[DataMember]
		public Requerimiento Requiere = new Requerimiento ();
		// Construcción
		/// <summary>
		/// Lista de los recursos requeridos.
		/// </summary>
		[DataMember (Name = "Construcción")]
		public Dictionary<Recurso, float> ReqRecursos = new Dictionary<Recurso, float> ();
		//public List<Basic.Par<Recurso, float>> ReqRecursos = new List<Basic.Par<Recurso, float>>();
		/// <summary>
		/// Devuelve la lista de requerimientos
		/// </summary>
		/// <value>El IRequerimiento</value> 
		public List<IRequerimiento<ICiudad>> Reqs ()
		{
			//List<IRequerimiento> ret = Basic.Covertidor<string, IRequerimiento>.ConvertirLista(Requiere, x => Global.g_.Data.EncuentraRequerimiento(x));
			return Requiere.Requiere ();
		}

		/// <summary>
		/// Especifica si este edificio se contruye automáticalente al cumplir todos los requisitos.
		/// </summary>
		[DataMember (Name = "EsAutoConstruíble")]
		public bool EsAutoConstruible;

		string CivLibrary.Debug.IPlainSerializable.PlainSerialize (int tabs)
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
				ret += x.PlainSerialize (tabs + 1);
			}

			ret += ((CivLibrary.Debug.IPlainSerializable)Requiere).PlainSerialize (tabs + 1);


			return ret;

		}

		#region Defaults

		[OnDeserializing]
		void OnDeserializing (StreamingContext context)
		{
			SetDefaults ();
		}

		void SetDefaults ()
		{
			MaxPorCiudad = 1;
			MaxPorCivilizacion = 0;
			MaxPorMundo = 0;
		}

		#endregion
	}
}