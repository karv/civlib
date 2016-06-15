using Graficas.Continuo;
using System;
using Graficas.Grafo;

namespace Civ.Topología
{
	/// <summary>
	/// Representa el mapa de este <c>mundo</c>.
	/// </summary>
	[Serializable]
	public class Mapa : Continuo<Terreno>
	{
		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Topología.Mapa"/> class.
		/// </summary>
		public Mapa ()
			: base (null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Topología.Mapa"/> class.
		/// </summary>
		/// <param name="grafica">Topología a usar como base</param>
		public Mapa (Grafo<Terreno, float> grafica)
			: base (grafica)
		{
		}

		#endregion
	}
}