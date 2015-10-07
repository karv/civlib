using Graficas.Rutas;

namespace Civ.Orden
{
	public class OrdenIrALugar:OrdenSerie
	{
		public OrdenIrALugar (Armada armada, Pseudoposicion destino)
			: base (armada)
		{
			var origen = armada.Posición;

			var RutaAA = Global.Juego.State.Topología.CaminoÓptimo (origen.A, destino.A);

			Enqueue (new OrdenIr (armada, origen.A));

			foreach (var x in RutaAA.Pasos)
			{
				Enqueue (new OrdenIr (armada, x.Destino));
			}

			Enqueue (new OrdenIr (armada, destino));

		}
	}
}

