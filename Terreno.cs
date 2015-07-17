using System;
using ListasExtra;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Civ
{
	/// <summary>
	/// Representa el terreno donde se construye una ciudad.
	/// </summary>
	public class Terreno: Pseudoposicion, ITickable, IEquatable<Terreno>, IEquatable<Pseudoposicion>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Terreno"/> class.
		/// </summary>
		/// <param name="Eco">Ecología a usar para crear el terreno.</param>
		public Terreno(Ecosistema Eco)
		{
			Vecinos.Nulo = float.PositiveInfinity;
			A = this;
			loc = 0;
			Random r = new Random();

			Nombre = Eco.Nombres[r.Next(Eco.Nombres.Count)];

			foreach (var x in Eco.PropPropiedad.Keys)
			{
				if (r.NextDouble() <= Eco.PropPropiedad[x])
				{	// Si el azar determina (¡Qué loco suena eso!) que hay que agregarle la propiedad...
					Innatos.Add(x);
				}
			}
		}

		#region IEquatable implementation

		bool IEquatable<Terreno>.Equals(Terreno other)
		{
			return ReferenceEquals(this, other);
		}

		bool IEquatable<Pseudoposicion>.Equals(Pseudoposicion other)
		{
			return other.enTerreno && ((IEquatable<Terreno>)other.A).Equals(this);
		}

		#endregion

		/// <summary>
		/// Terrenos vecinos.
		/// </summary>
		public readonly ListaPeso<Terreno> Vecinos = new ListaPeso<Terreno>();
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

		/// <summary>
		/// Da un tick de longitud t al terreno.
		/// </summary>
		/// <param name="t">Longitud del tick</param>
		public void Tick(float t)
		{
			//Crecimiento automático de recursos ecológicos.
			foreach (var x in Eco.RecursoEcologico.Keys)
			{
				Ecologia.RecursoEstado RE = new Ecologia.RecursoEstado();

				RE.Cant = Eco.RecursoEcologico[x].Cant + Eco.RecursoEcologico[x].Crec * t;
				RE.Cant = Math.Min(Eco.RecursoEcologico[x].Cant, Eco.RecursoEcologico[x].Max);
				RE.Crec = Eco.RecursoEcologico[x].Crec;
				RE.Max = Eco.RecursoEcologico[x].Max;
				Eco.RecursoEcologico.Remove(x);
				Eco.RecursoEcologico.Add(x, RE);
			}
		}
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
		/// Representa las propiedades que puede adquirir un ecosistema.
		/// </summary>
		//[DataContract (Name= "Propiedad")]
		public class EcosistemaPropiedades : ListaPeso<Propiedad>
		{
		}

		/// <summary>
		/// El nombre del terreno
		/// </summary>
		[DataMember]
		public string Nombre;
		[DataMember(Name = "Propiedades")]
		EcosistemaPropiedades _PropPropiedad = new EcosistemaPropiedades();

		/// <summary>
		/// Es la lista de probabilidades de que una <c>Propiedad</c> <c>Innata</c> aparezca en un terreno con esta ecología.
		/// </summary>
		/// <value><c>ListaPeso</c> de asignación de <c>Propiedades</c> con sus probabilidades.</value>
		public EcosistemaPropiedades PropPropiedad
		{
			get
			{
				return _PropPropiedad;
			}
		}

		[DataMember(Name = "Nombres")]
		public List<string> Nombres;

		public Terreno CrearTerreno()
		{
			return new Terreno(this);
		}
	}
}
