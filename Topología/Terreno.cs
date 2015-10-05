using System;
using ListasExtra;
using System.Collections.Generic;
using Global;

namespace Civ
{
	/// <summary>
	/// Representa el terreno donde se construye una ciudad.
	/// </summary>
	public class Terreno: Pseudoposicion, ITickable, IEquatable<Terreno>, IEquatable<Pseudoposicion>, IPosicionable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Terreno"/> class.
		/// </summary>
		/// <param name="ecosistema">Ecología a usar para crear el terreno.</param>
		public Terreno(Ecosistema ecosistema)
		{
			A = this;
			Loc = 0;
			Random r = Juego.Rnd;

			Nombre = ecosistema.Nombres[r.Next(ecosistema.Nombres.Count)];

			foreach (var x in ecosistema.PropPropiedad.Keys)
			{
				if (r.NextDouble() <= ecosistema.PropPropiedad[x])
				{	// Si el azar determina (¡Qué loco suena eso!) que hay que agregarle la propiedad...
					Innatos.Add(x);
				}
			}
		}

		#region IPosicionable implementation

		Pseudoposicion IPosicionable.Posicion()
		{
			return this;
		}

		#endregion

		#region IEquatable implementation

		bool IEquatable<Terreno>.Equals(Terreno other)
		{
			return ReferenceEquals(this, other);
		}

		bool IEquatable<Pseudoposicion>.Equals(Pseudoposicion other)
		{
			return other.EnTerreno && ((IEquatable<Terreno>)other.A).Equals(this);
		}

		#endregion

		/// <summary>
		/// Terrenos vecinos.
		/// </summary>
		public ICollection<Terreno> Vecinos
		{
			get
			{
				return base.Universo.GráficaBase.Vecinos(this);
			}
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

		/// <summary>
		/// Da un tick de longitud t al terreno.
		/// </summary>
		/// <param name="t">Longitud del tick</param>
		public void Tick(TimeSpan t)
		{
			//Crecimiento automático de recursos ecológicos.
			foreach (var x in Eco.RecursoEcologico.Keys)
			{
				var RE = new Ecologia.RecursoEstado();

				RE.Cant = Eco.RecursoEcologico[x].Cant + Eco.RecursoEcologico[x].Crec * (float)t.TotalHours;
				RE.Cant = Math.Min(Eco.RecursoEcologico[x].Cant, Eco.RecursoEcologico[x].Max);
				RE.Crec = Eco.RecursoEcologico[x].Crec;
				RE.Max = Eco.RecursoEcologico[x].Max;
				Eco.RecursoEcologico.Remove(x);
				Eco.RecursoEcologico.Add(x, RE);
			}
		}
	}

}
