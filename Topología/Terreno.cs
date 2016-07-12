using System;
using System.Collections.Generic;
using Civ.RAW;
using Civ.Global;
using Civ.ObjetosEstado;

namespace Civ.Topología
{
	/// <summary>
	/// Representa el terreno donde se construye una ciudad.
	/// </summary>
	[Serializable]
	public class Terreno : ITickable, IEquatable<Terreno>, IEquatable<Pseudoposición>, IPosicionable
	{
		#region Topología

		/// <summary>
		/// Devuelve este terreno como Pseudoposición
		/// </summary>
		/// <value>The position.</value>
		public Pseudoposición Pos { get; private set; }

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

		/// <summary>
		/// Construye una nueva posición y se le asigna a este terreno
		/// </summary>
		/// <remarks>No elimina la posición anterior.</remarks>
		public void AsignarPosición ()
		{
			Pos = new Pseudoposición (Juego.State.Mapa.PuntoFijo (this));
		}

		#endregion

		#region Ecología

		/// <summary>
		/// Representa la ecología del terreno.
		/// </summary>
		public Ecología Eco;

		/// <summary>
		/// Devuelve el ecosistema de este terreno
		/// </summary>
		/// <value>The ecosistema.</value>
		public Ecosistema Ecosistema { get; }

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

		#endregion

		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Topología.Terreno"/> class.
		/// </summary>
		/// <param name="ecosistema">Ecología a usar para crear el terreno.</param>
		public Terreno (Ecosistema ecosistema)
			: this ()
		{
			Ecosistema = ecosistema;
			Random r = HerrGlobal.Rnd;

			GenerarNombre ();

			Eco = new Ecología ();

			foreach (var x in ecosistema.PropPropiedad.Keys)
			{
				if (r.NextDouble () <= ecosistema.PropPropiedad [x])
				{	// Si el azar determina (¡Qué loco suena eso!) que hay que agregarle la propiedad...
					Innatos.Add (x);
					foreach (var y in x.Iniciales)
						Eco.RecursoEcológico [y.Key] += y.Value;
				}
			}
		}

		Terreno ()
		{
		}

		#endregion

		#region Nombre

		/// <summary>
		/// Nombre del terreno
		/// </summary>
		public string Nombre;

		void GenerarNombre ()
		{
			var r = HerrGlobal.Rnd;
			#if DEBUG
			Nombre = Ecosistema.Nombre + "\\" + Ecosistema.Nombres.Elegir () + "\\";
			#else
			Nombre = ecosistema.Nombres.Elegir ();
			#endif
			Nombre += r.Next (10000).ToString ();
		}

		#endregion

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

		#region General

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Civ.Topología.Terreno"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Civ.Topología.Terreno"/>.</returns>
		public override string ToString ()
		{
			return Nombre;
		}

		/// <summary>
		/// Ciudad que está contruida en este terreno.
		/// </summary>
		public Ciudad CiudadConstruida;

		/// <summary>
		/// Da un tick de longitud t al terreno.
		/// </summary>
		/// <param name="t">Longitud del tick</param>
		public void Tick (TiempoEventArgs t)
		{
			AlTickAntes?.Invoke (this, t);
			Eco.Tick (t);
			AlTickDespués?.Invoke (this, t);
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre antes del tick
		/// </summary>
		public event EventHandler AlTickAntes;

		/// <summary>
		/// Ocurre después del tick
		/// </summary>
		public event EventHandler AlTickDespués;

		#endregion
	}
}