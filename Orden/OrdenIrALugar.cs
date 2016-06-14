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
		#region Orden

		public bool Ejecutar (TimeSpan t)
		{
			var avance = (float)t.TotalHours * ArmadaEjecutante.Velocidad;

			if (ArmadaEjecutante.Posición.Equals (Ruta.NodoFinal))
			{
				AlLlegar?.Invoke ();
				return true;
			}
			ArmadaEjecutante.Posición.AvanzarHacia (Ruta, avance);
			return false;
		}

		public Armada ArmadaEjecutante { get; }

		#endregion

		#region Ir a

		public Continuo<Terreno>.Ruta Ruta { get; }

		public TimeSpan TiempoEstimado
		{
			get
			{
				var fl = Ruta.Longitud / ArmadaEjecutante.Velocidad;
				return TimeSpan.FromHours (fl);
			}
		}

		#endregion

		#region ctor

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

		#endregion

		#region Eventos

		public event Action AlLlegar;

		#endregion
	}
}