using System;
using System.Runtime.Serialization;

namespace Civ
{
	/// <summary>
	/// Representa un adelanto científico.
	/// </summary>
    [DataContract(IsReference = true)]
    public class Ciencia : IRequerimiento
	{
        [DataContract(IsReference = true)]
        public class Requerimiento
        {
            /// <summary>
            /// Recurso que se necesita para investigar.
            /// </summary>
            [DataMember(Name = "Recurso")]
            public Recurso Rec;
            /// <summary>
            /// Cantidad de <see cref="RecursoReq"/> que se necesita para investigar.
            /// </summary>
            [DataMember(Name = "Cantidad")]
            public float Cantidad;
            /// <summary>
            /// Lista de requerimientos científicos.
            /// </summary>
            [DataMember(Name = "Ciencias")]
            public System.Collections.Generic.List<Ciencia> Ciencias = new System.Collections.Generic.List<Ciencia>();
        }


		/// <summary>
		/// Nombre de la ciencia;
		/// </summary>
        [DataMember(Name = "Nombre")]
		public String Nombre;

		public override string ToString ()
		{
			return Nombre;
		}

			// Sobre los requerimientos.    
        /// <summary>
        /// Requerimientos para poder aprender este avance.
        /// </summary>
        [DataMember(Name = "Requiere")]
        public Requerimiento Reqs;

			// IRequerimiento
		bool Civ.IRequerimiento.LoSatisface (Ciudad C){
			return C.CivDueño.Avances.Contains(this);
		}

        string IRequerimiento.ObtenerId()
        {
            return Nombre;
        }   //TODO: ¿Se necesita esto?
    }
}
