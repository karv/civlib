using System.Collections.Generic;
using Graficas.Continuo;
using Civ.Global;
using System;
using Civ.ObjetosEstado;

namespace Civ.Topología
{
	/// <summary>
	/// Representa un lugar que no es terreno, más bien es un punto en una arista de la Topología del mundo.
	/// </summary>
	[Serializable]
	public class Pseudoposición : Continuo<Terreno>.ContinuoPunto
	{
		#region ctor

		public Pseudoposición (IPosicionable objeto)
			: base (Juego.State.Mapa)
		{
			Objeto = objeto;
			var p = Objeto.Posición ();
			A = p.A;
			B = p.B;
			Loc = p.Loc;
		}

		public Pseudoposición (Continuo<Terreno>.ContinuoPunto p)
			: base (Juego.State.Mapa, p.A, p.B, p.Loc)
		{
			Objeto = null;
		}

		#endregion

		#region General

		/// <summary>
		/// Objeto en esta posición
		/// Armada, ciudad, etc
		/// </summary>
		public IPosicionable Objeto { get; }

		/// <summary>
		/// Devuelve una colección con las armadas que existen en esta misma poisición
		/// </summary>
		/// <returns>The posición.</returns>
		public ICollection<Armada> ArmadasMismaPos ()
		{
			// THINK: ¿Debe revolver una colección de IPosicionables?
			var ret = new List<Armada> ();
			foreach (var x in Juego.State.ArmadasExistentes())
			{
				if (Equals (x.Posición))
					ret.Add (x);
			}
			return ret;
		}

		/// <summary>
		/// Revisa si está posición es Origen
		/// </summary>
		/// <value><c>true</c> if en origen; otherwise, <c>false</c>.</value>
		public bool EnTerreno
		{
			get
			{
				return EnOrigen;
			}
		}

		#endregion

		#region IEquatable implementation

		/*
		public bool Equals(IPosición other)
		{
			if (ReferenceEquals(this, other))
				return true;
			
			if (other is Terreno)
			{
				Terreno otherTerreno = (Terreno)other;
				return (enOrigen && otherTerreno.Equals(Origen)) || (enDestino && otherTerreno.Equals(Destino));
			}
			if (other is Pseudoposicion)
			{
				Pseudoposicion otherPP = (Pseudoposicion)other;
				return (otherPP.Origen.Equals(Origen) && otherPP.Destino.Equals(Destino) && otherPP.Avance == Avance) ||
				(otherPP.Origen.Equals(Destino) && otherPP.Destino.Equals(Origen) && otherPP.Avance == 1 - Avance);
			}

			throw new NotImplementedException();
		}
		*/

		#endregion

		#region Cloneable implementation

		/// <summary>
		/// Devuelve una nueva pseudoposición equivalente a ésta.
		/// </summary>
		public Pseudoposición Clonar (IPosicionable p)
		{
			return new Pseudoposición (p);
		}

		#endregion
	}
}