using System;
using System.Collections.Generic;
using ListasExtra;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Civ
{
	/// <summary>
	/// Representa un trabajo en un edificioRAW
	/// </summary>	
	[DataContract(IsReference = true, Name = "Trabajo")]
	public class TrabajoRAW
	{
		/// <summary>
		/// Nombre
		/// </summary>
		[DataMember]
		public string Nombre;
		/// <summary>
		/// EdificioRAW vinculado a este trabajo.
		/// </summary>
		[DataMember]
		public EdificioRAW Edificio;
		[DataMember(Name = "Salida")]
		ListaPeso<Recurso> _SalidaBase = new ListaPeso<Recurso>();

		/// <summary>
		/// Recursos producidos por trabajador*turno (Base)
		/// </summary>
		public ListaPeso<Recurso> SalidaBase
		{
			get
			{
				return _SalidaBase;
			}
		}

		/// <summary>
		/// Recursos consumidos por trabajador*turno (Base)
		/// </summary>
		[DataMember(Name = "Entrada")]
		ListaPeso<Recurso> _EntradaBase = new ListaPeso<Recurso>();

		/// <summary>
		/// Recursos consumidos por trabajador*turno (Base)
		/// </summary>		
		public ListaPeso<Recurso> EntradaBase
		{
			get
			{
				return _EntradaBase;
			}
		}

		public override string ToString()
		{
			return string.Format("{0} @ {1}", Nombre, Edificio);
		}
		// Requiere
		/// <summary>
		/// Lista de requerimientos.
		/// </summary>
		[DataMember]
		public Requerimiento Requiere = new Requerimiento();

		/// <summary>
		/// Devuelve la lista de requerimientos.
		/// </summary>
		/// <value>El IRequerimiento</value> 
		public List<IRequerimiento> Reqs()
		{
			return Requiere.Requiere();
		}
	}
}