using Graficas.Continuo;
using System;
using Graficas.Grafo;

namespace Civ.Topología
{
	[Serializable]
	public class Mapa : Continuo<Terreno>
	{
		#region ctor

		public Mapa ()
			: base (null)
		{
		}

		public Mapa (Grafo<Terreno, float> grafica)
			: base (grafica)
		{
		}

		#endregion
	}
}