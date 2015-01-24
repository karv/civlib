using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Civ
{
	/// <summary>
	/// Representa una clase de edificios. Para sólo lectura.
	/// </summary>
    [DataContract(IsReference = true, Name="Edificio")]
    public class EdificioRAW : IRequerimiento
	{
        [DataMember]
		public string Nombre;
        [DataMember]
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
		/// Lista de sus IRequerimientos.
		/// </summary>
        [DataMember(Name = "Construcción")]
        public Dictionary<Recurso, float> ReqRecursos = new Dictionary<Recurso, float>();
        //public List<Basic.Par<Recurso, float>> ReqRecursos = new List<Basic.Par<Recurso, float>>();

		/// <summary>
		/// Devuelve la lista de requerimientos
		/// </summary>
		/// <value>El IRequerimiento</value> 
		public List<IRequerimiento> Reqs ()
		{
			//List<IRequerimiento> ret = Basic.Covertidor<string, IRequerimiento>.ConvertirLista(Requiere, x => Global.g_.Data.EncuentraRequerimiento(x));
            return Requiere.Requiere();
		}

		/// <summary>
		/// Especifica si este edificio se contruye automáticalente al cumplir todos los requisitos.
		/// </summary>
		public bool EsAutoConstruíble;
	}

}

