using System.Collections.Generic;
using Graficas.Continuo;
using Civ.Global;
using System;
using Civ.ObjetosEstado;

namespace Civ.Topología
{
	[Serializable]
	/// <summary>
	/// Representa un lugar que no es terreno, más bien es un punto en una arista de la Topología del mundo.
	/// </summary>
	public class Pseudoposición : Continuo<Terreno>.ContinuoPunto
	{
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
		/*
		public Pseudoposición ()
			: base (Juego.State.Mapa)
		{
			Objeto = null;
		}
		*/

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
			var ret = new List<Armada> ();
			foreach (var x in Juego.State.ArmadasExistentes())
			{
				if (Equals (x.Posición))
					ret.Add (x);
			}
			return ret;
		}

		/// <summary>
		/// Avanza la posición Dist de distancia hacia Destino.
		/// </summary>
		[Obsolete]
		public void Avanzar (float dist)
		{
			Loc += dist;
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

		/// <summary>
		/// Devuelve el extremo de esta pseudoposición que no es el extremo dado.
		/// Si sólo tiene un extremo, devuelve este único.
		/// </summary>
		/// <param name="noExtremo">Extremo excluido.</param>
		[Obsolete]
		public Terreno ExtremoNo (Terreno noExtremo)
		{
			if (B == null)
				return A;
			return  A.Equals (noExtremo) ? B : A;
		}

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

		/// <summary>
		/// Devuelve la orientación de esta posición cn respecto a otra.
		/// </summary>
		/// <returns>The orientacion.</returns>
		/// <param name="other">Other.</param>
		[Obsolete]
		public int Orientacion (Pseudoposición other)
		{
			return A == other.A && B == other.B && Loc < other.Loc ? -1 : 1; // -1 si está 'de el lado izquierdo'
		}

	}
}