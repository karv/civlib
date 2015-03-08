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
		[DataContract(IsReference = false)]
		public class Requerimiento
		{
			[DataMember(Name = "Recurso")]
			ListasExtra.ListaPeso<Recurso> _Recursos = new ListasExtra.ListaPeso<Recurso>();

			/// <summary>
			/// Devuelve la lista de recursos que se necesita para investigar
			/// </summary>
			/// <value>The recursos.</value>
			public ListasExtra.ListaPeso<Recurso> Recursos
			{
				get
				{
					return _Recursos;
				}
			}

			/// <summary>
			/// Lista de requisitos científicos.
			/// </summary>
			[DataMember(Name = "Ciencias")]
			public System.Collections.Generic.List<Ciencia> Ciencias = new System.Collections.Generic.List<Ciencia>();
		}

		/// <summary>
		/// Nombre de la ciencia;
		/// </summary>
		[DataMember(Name = "Nombre")]
		public String Nombre;

		public override string ToString()
		{
			return Nombre;
		}
		// Sobre los requerimientos.    
		/// <summary>
		/// Requerimientos para poder aprender este avance.
		/// </summary>
		[DataMember(Name = "Requiere")]
		public Requerimiento Reqs = new Requerimiento();
		// IRequerimiento
		bool Civ.IRequerimiento.LoSatisface(Ciudad C)
		{
			return C.CivDueno.Avances.Contains(this);
		}
	}
}
