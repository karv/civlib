using Graficas.Continuo;
using System;
using Graficas.Grafo;

namespace Civ.Topología
{
	[Serializable]
	public class Mapa : Continuo<Terreno>
	{
		public Mapa ()
			: base (null)
		{
		}

		public Mapa (ILecturaGrafoPeso<Terreno> grafica)
			: base (grafica)
		{
		}


	}
}