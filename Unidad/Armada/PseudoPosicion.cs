using System;

namespace Civ
{
	/// <summary>
	/// Representa un lugar que no es terreno, más bien es un punto en una arista de la Topología del mundo.
	/// </summary>
	public class Pseudoposicion : IPosición
	{
		/// <summary>
		/// Punto A de esta posición.
		/// </summary>
		public Terreno Origen;
		/// <summary>
		/// Punto B de esta posición
		/// </summary>
		public Terreno Destino;
		/// <summary>
		/// Distancia entre Origen y "esta posición"
		/// </summary>
		public float Avance;

		/// <summary>
		/// Calcula la distancia entre Origen y Destino.
		/// </summary>
		public float Distancia()
		{
			return Origen.Vecinos[Destino] - Avance;
		}

		/// <summary>
		/// Avanza la posición Dist de distancia hacia Destino.
		/// </summary>
		public void Avanzar(float Dist)
		{
			Avance += Dist;
		}

		/// <summary>
		/// Revisa si esta posición es Destino
		/// </summary>
		/// <value><c>true</c> if en destino; otherwise, <c>false</c>.</value>
		public bool enDestino
		{
			get
			{
				return Distancia() >= 0;
			}
		}

		/// <summary>
		/// Revisa si está posición es Origen
		/// </summary>
		/// <value><c>true</c> if en origen; otherwise, <c>false</c>.</value>
		public bool enOrigen
		{
			get
			{
				return Avance == 0;
			}
		}

		#region IEquatable implementation

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

		#endregion
	}
}
