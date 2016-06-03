using Graficas.Continuo;
using Civ.Global;
using System;
using Civ.Topología;
using Civ.ObjetosEstado;

namespace Civ.Orden
{
	[Serializable]
	public class OrdenIrALugar : IOrden
	{
		public bool Ejecutar (TimeSpan t)
		{
			var avance = (float)t.TotalHours * ArmadaEjecutante.Velocidad;

			ArmadaEjecutante.Posición.AvanzarHacia (Ruta, avance);
			if (ArmadaEjecutante.Posición.Equals (Ruta.NodoFinal))
			{
				AlLlegar?.Invoke ();
				return true;
			}
			return false;
		}

		public Armada ArmadaEjecutante { get; }

		public Continuo<Terreno>.Ruta Ruta { get; }

		public OrdenIrALugar (Armada armada, Pseudoposición destino)
		{
			var origen = armada.Posición;
			ArmadaEjecutante = armada;
			Ruta = Continuo<Terreno>.RutaÓptima (origen, destino, Juego.State.Rutas);
			//Ruta = Juego.State.Mapa.RutaÓptima (origen, destino, Juego.State.Rutas);
			//Ruta = new Continuo<Terreno>.Ruta (origen);
			//var rutaIntermedia = Juego.State.Rutas.CaminoÓptimo (origen.A, destino.A);
			//Ruta.Concat (rutaIntermedia);
			//Ruta.ConcatFinal (destino);
		}

		public TimeSpan TiempoEstimado
		{
			get
			{
				var fl = Ruta.Longitud / ArmadaEjecutante.Velocidad;
				return TimeSpan.FromHours (fl);
			}
		}

		public event Action AlLlegar;
	}
}

