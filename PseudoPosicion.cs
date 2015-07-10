using System;
using System.Collections.Generic;
using System.Linq;

namespace Civ
{
	/// <summary>
	/// Representa un lugar que no es terreno, más bien es un punto en una arista de la Topología del mundo.
	/// </summary>
	public class Pseudoposicion :Graficas.Continuo.Continuo<Terreno>.ContinuoPunto
	{
		public Pseudoposicion() : base(Global.g_.State.Mapa)
		{
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

	}
}