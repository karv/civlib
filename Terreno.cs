using System;
using ListasExtra;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Civ
{
	/// <summary>
	/// Representa el terreno donde se construye una ciudad.
	/// </summary>
	public class Terreno : IPosicion
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Terreno"/> class.
		/// </summary>
		/// <param name="Eco">Ecología a usar para crear el terreno.</param>
		public Terreno(Ecosistema Eco)
		{
			Random r = new Random();

			foreach (var x in Eco.PropPropiedad.Keys)
			{
				if (r.Next() < Eco.PropPropiedad[x])
				{	// Si el azar determina (¡Qué loco suena eso!) que hay que agregarle la propiedad...
					Innatos.Add(x);
				}
			}
			Nombre = Eco.Nombre;
		}

		public string Nombre;
		/// <summary>
		/// Propiedades que se contruyen al construir una ciudad aquí.
		/// </summary>        
		public List<Propiedad> Innatos = new List<Propiedad>();

		public override string ToString()
		{
			return Nombre;
		}
		// Ecología
		/// <summary>
		/// Representa la ecología del terreno.
		/// </summary>
		public Ecologia Eco = new Ecologia();

		/// <summary>
		/// Representa la ecología del terreno.
		/// </summary>
		public class Ecologia
		{
			public struct RecursoEstado
			{
				public float Cant;
				public float Max;
				public float Crec;
			}

			public Dictionary<Recurso, RecursoEstado> RecursoEcologico = new Dictionary<Recurso, RecursoEstado>();
		}

		/// <summary>
		/// Ciudad que está contruida en este terreno.
		/// </summary>
		public Ciudad CiudadConstruida;
	}

	/// <summary>
	/// Es la forma RAW de Terreno.
	/// Es, en forma menos técnica, una clase de ecosistema ecológico.
	/// Ej. Selva, desierto, etc.
	/// </summary>
	[DataContract]
	public class Ecosistema
	{
		/// <summary>
		/// El nombre del terreno
		/// </summary>
		[DataMember]
		public string Nombre;
		[DataMember(Name = "Propiedades")]
		ListaPeso<Propiedad> _PropPropiedad = new ListaPeso<Propiedad>();

		/// <summary>
		/// Es la lista de probabilidades de que una <c>Propiedad</c> <c>Innata</c> aparezca en un terreno con esta ecología.
		/// </summary>
		/// <value><c>ListaPeso</c> de asignación de <c>Propiedades</c> con sus probabilidades.</value>
		public ListaPeso<Propiedad> PropPropiedad
		{
			get
			{
				return _PropPropiedad;
			}
		}

		public Terreno CrearTerreno()
		{
			return new Terreno(this);
		}
	}
}
