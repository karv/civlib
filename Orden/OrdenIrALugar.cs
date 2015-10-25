using Graficas.Rutas;

namespace Civ.Orden
{
	public class OrdenIrALugar:OrdenSerie
	{
		public OrdenIrALugar (Armada armada, Pseudoposición destino)
			: base (armada)
		{
			var origen = armada.Posición;

			var RutaAA = Global.Juego.State.Rutas.CaminoÓptimo (origen.A, destino.A);

			Encolar (new OrdenIr (armada, origen.A.Pos));

			foreach (var x in RutaAA.Pasos)
			{
				Encolar (new OrdenIr (armada, x.Destino.Pos));
			}

			Encolar (new OrdenIr (armada, destino));

		}
	}
}

