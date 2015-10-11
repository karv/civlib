﻿using System;
using System.Collections.Generic;
using Global;
using Civ.Data;
using Basic;

namespace Civ
{
	/// <summary>
	/// Representa el terreno donde se construye una ciudad.
	/// </summary>
	public class Terreno: Pseudoposición, ITickable, IEquatable<Terreno>, IEquatable<Pseudoposición>, IPosicionable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Terreno"/> class.
		/// </summary>
		/// <param name="ecosistema">Ecología a usar para crear el terreno.</param>
		public Terreno (Ecosistema ecosistema)
		{
			
			A = this;
			Loc = 0;
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
			return this;
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
				return Universo.GráficaBase.Vecinos (this);
			}
		}

		public string Nombre;

		/// <summary>
		/// Propiedades que se contruyen al construir una ciudad aquí.
		/// </summary>        
		public ICollection<Propiedad> Innatos = new List<Propiedad> ();

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
			//TODO ¿No debería Propiedad hacer esto?
			//Crecimiento automático de recursos ecológicos.
			foreach (var x in Eco.RecursoEcologico.Keys)
			{
				var RE = new RecursoEstado ();

				RE.Cant = Eco.RecursoEcologico [x].Cant + Eco.RecursoEcologico [x].Crec * (float)t.TotalHours;
				RE.Cant = Math.Min (
					Eco.RecursoEcologico [x].Cant,
					Eco.RecursoEcologico [x].Max);
				RE.Crec = Eco.RecursoEcologico [x].Crec;
				RE.Max = Eco.RecursoEcologico [x].Max;
				Eco.RecursoEcologico.Remove (x);
				Eco.RecursoEcologico.Add (x, RE);
			}
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