using System;
using System.Collections.Generic;
using Global;
using Civ.Data;
using Basic;
using System.Reflection;

namespace Civ
{
	/// <summary>
	/// Representa el terreno donde se construye una ciudad.
	/// </summary>
	public class Terreno: ITickable, IEquatable<Terreno>, IEquatable<Pseudoposición>, IPosicionable
	{
		public Pseudoposición Pos
		{
			get
			{
				return Juego.State.Mapa.PuntoFijo (this) as Pseudoposición;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Terreno"/> class.
		/// </summary>
		/// <param name="ecosistema">Ecología a usar para crear el terreno.</param>
		public Terreno (Ecosistema ecosistema)
		{
			Random r = Juego.Rnd;

			Nombre = ecosistema.Nombres.Elegir ();

			foreach (var x in ecosistema.PropPropiedad.Keys)
			{
				if (r.NextDouble () <= ecosistema.PropPropiedad [x])
				{	// Si el azar determina (¡Qué loco suena eso!) que hay que agregarle la propiedad...
					Innatos.Add (x);
				}
			}
		}

		#region IPosicionable implementation

		Pseudoposición IPosicionable.Posición ()
		{
			return Pos;
		}

		#endregion

		#region IEquatable implementation

		bool IEquatable<Terreno>.Equals (Terreno other)
		{
			return ReferenceEquals (this, other);
		}

		bool IEquatable<Pseudoposición>.Equals (Pseudoposición other)
		{
			return other.EnTerreno && ((IEquatable<Terreno>)other.A).Equals (this);
		}

		#endregion

		/// <summary>
		/// Terrenos vecinos.
		/// </summary>
		public ICollection<Terreno> Vecinos
		{
			get
			{
				return Juego.State.Topología.Vecino (this);
			}
		}

		public string Nombre;

		/// <summary>
		/// Propiedades que se contruyen al construir una ciudad aquí.
		/// </summary>        
		public ICollection<Propiedad> Innatos
		{
			get
			{
				return Eco.Innatos;
			}
		}

		public override string ToString ()
		{
			return Nombre;
		}

		// Ecología
		/// <summary>
		/// Representa la ecología del terreno.
		/// </summary>
		public Ecología Eco = new Ecología ();


		/// <summary>
		/// Ciudad que está contruida en este terreno.
		/// </summary>
		public Ciudad CiudadConstruida;

		/// <summary>
		/// Da un tick de longitud t al terreno.
		/// </summary>
		/// <param name="t">Longitud del tick</param>
		public void Tick (TimeSpan t)
		{
			AlTickAntes?.Invoke (t);
			Eco.Tick (t);
			AlTickDespués?.Invoke (t);
		}

		#region Eventos

		/// <summary>
		/// Ocurre antes del tick
		/// </summary>
		public event Action<TimeSpan> AlTickAntes;

		/// <summary>
		/// Ocurre después del tick
		/// </summary>
		public event Action<TimeSpan> AlTickDespués;

		#endregion
	}
}