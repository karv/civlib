using System;
using System.Collections.Generic;
using System.Linq;
using Graficas.Continuo;
using System.Collections;
using Global;

namespace Civ
{
	/// <summary>
	/// Representa un lugar que no es terreno, más bien es un punto en una arista de la Topología del mundo.
	/// </summary>
	public class Pseudoposicion :Graficas.Continuo.Continuo<Terreno>.ContinuoPunto
	{
		Continuo<Terreno> _grafica
		{
			get
			{
				return this._universo;
			}
		}

		public Pseudoposicion() : base(Global.g_.State.Mapa)
		{
		}

		/// <summary>
		/// Devuelve una colección con las armadas que existen en esta misma poisición
		/// </summary>
		/// <returns>The posición.</returns>
		public ICollection<Armada> ArmadasMismaPos()
		{
			List<Armada> ret = new List<Armada>();
			foreach (var x in g_.State.getArmadas())
			{
				if (this.Equals((Continuo<Terreno>.ContinuoPunto)x.Posicion))
					ret.Add(x);
			}
			return ret;
		}

		/// <summary>
		/// Avanza la posición Dist de distancia hacia Destino.
		/// </summary>
		public void Avanzar(float dist)
		{
			loc += dist;
		}

		/// <summary>
		/// Revisa si está posición es Origen
		/// </summary>
		/// <value><c>true</c> if en origen; otherwise, <c>false</c>.</value>
		public bool enTerreno
		{
			get
			{
				return enOrigen();
			}
		}

		/// <summary>
		/// Devuelve el extremo de esta pseudoposición que no es el extremo dado.
		/// Si sólo tiene un extremo, devuelve este único.
		/// </summary>
		/// <param name="noExtremo">Extremo excluido.</param>
		public Terreno getExtremoNo(Terreno noExtremo)
		{
			if (B == null)
				return A;
			if (A.Equals(noExtremo))
				return B;
			else
				return A;
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
		public Pseudoposicion Clonar()
		{
			Pseudoposicion ret = new Pseudoposicion();
			ret.A = A;
			ret.B = B;
			ret.loc = loc;
			return ret;
		}

		#endregion

		/// <summary>
		/// Devuelve la orientación de esta posición cn respecto a otra.
		/// </summary>
		/// <returns>The orientacion.</returns>
		/// <param name="other">Other.</param>
		public int getOrientacion(Pseudoposicion other)
		{
			return A == other.A && B == other.B && loc < other.loc ? -1 : 1; // -1 si está 'de el lado izquierdo'
		}
	}
}