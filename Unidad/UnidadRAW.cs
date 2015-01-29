using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ListasExtra;

namespace Civ
{
	/// <summary>
	/// Representa una clase de unidad
	/// </summary>
	[DataContract(Name = "Unidad", IsReference = true)]
	public class UnidadRAW
	{
		/// <summary>
		/// El nombre de la clase de unidad.
		/// </summary>
		[DataMember]
		public string Nombre;

		[DataMember(Name = "Modificadores")]
		ListaPeso<string> _Mods = new ListaPeso<string>();

		/// <summary>
		/// Lista de modificadores de combate de la unidad.
		/// </summary>        
		public ListaPeso<string> Mods
		{
			get { return _Mods; }
		}

		/// <summary>
		/// Fuerza de la unidad.
		/// </summary>
		[DataMember]
		public float Fuerza;

		[DataMember(Name = "Flags")]
		private List<string> _Flags = new List<string>();

		/// <summary>
		/// Flags.
		/// </summary>
		public List<string> Flags
		{
			get { return _Flags; }
		}

		// Reqs
		[DataMember(Name = "Requerimientos")]
		private ListaPeso<Recurso> _Reqs = new ListaPeso<Recurso>();

		/// <summary>
		/// Requerimientos para crearse.
		/// </summary>
		public ListaPeso<Recurso> Reqs
		{
			get { return _Reqs; }
		}

		/// <summary>
		/// Población productiva que requiere para entrenar.
		/// </summary>
		[DataMember]
		public ulong CostePoblación;

		/// <summary>
		/// Representa el coste de espacio de esta unidad en una armada.
		/// </summary>
		[DataMember]
		public float Peso;

		public override string ToString()
		{
			return Nombre;
		}
	}
}
