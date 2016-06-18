using Graficas.Continuo;
using Civ.Global;
using System;
using Civ.Topología;
using Civ.ObjetosEstado;

namespace Civ.Orden
{
	/// <summary>
	/// Orden de armada de ir a un lugar específico fijo, no necesariamente vecino.
	/// </summary>
	[Serializable]
	public class OrdenIrALugar : IOrden
	{
		#region Orden

		/// <summary>
		/// Ejecuta la orden
		/// Devuelve true si la orden ha sido terminada.
		/// </summary>
		/// <param name="t">Tiempo de ejecución</param>
		public bool Ejecutar (TimeSpan t)
		{
			var avance = (float)t.TotalHours * ArmadaEjecutante.Velocidad;

			if (ArmadaEjecutante.Posición.Equals (Ruta.NodoFinal))
			{
				// Dispose Ruta
				((IDisposable)Ruta.NodoFinal).Dispose ();

				AlLlegar?.Invoke (this, new TransladoEventArgs (Ruta));
				return true;
			}
			//Juego.Instancia.GState.Mapa.Puntos.Contains (Ruta.NodoFinal)
			//if (Mapa(Ruta.NodoFinal))
			ArmadaEjecutante.Posición.AvanzarHacia (Ruta, avance);
			return false;
		}

		/// <summary>
		/// Devuelve la armada de esta orden
		/// </summary>
		/// <value>The armada.</value>
		public Armada ArmadaEjecutante { get; }

		#endregion

		#region Ir a

		/// <summary>
		/// Devuelve la ruta que seguirá al desplazarse con esta orden
		/// </summary>
		/// <value>The ruta.</value>
		public Continuo<Terreno>.Ruta Ruta { get; }

		/// <summary>
		/// Devuelve el tiempo estimado en llegar a su destino
		/// </summary>
		/// <value>The tiempo estimado.</value>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Orden.OrdenIrALugar"/> class.
		/// </summary>
		/// <param name="armada">Armada.</param>
		/// <param name="destino">Destino.</param>
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

		/// <summary>
		/// Ocurre al llegar al destino
		/// </summary>
		public event EventHandler AlLlegar;

		#endregion
	}
}