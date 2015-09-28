using Graficas.Rutas;

namespace Civ.Orden
{
	public class OrdenIrALugar:OrdenSerie
	{
		public OrdenIrALugar(Armada armada, Pseudoposicion destino) : base(armada)
		{
			var origen = armada.Posicion;

			var RutaAA = Global.Juego.State.Topologia.CaminoÓptimo(origen.A, destino.A);

			ColaOrden.Enqueue(new OrdenIr(armada, origen.A));

			foreach (var x in RutaAA.Pasos)
			{
				ColaOrden.Enqueue(new OrdenIr(armada, x.Destino));
			}

			ColaOrden.Enqueue(new OrdenIr(armada, destino));

		}
	}
}

