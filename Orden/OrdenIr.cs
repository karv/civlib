using System;
using Civ.Topología;
using Civ.ObjetosEstado;
using Civ.IU;

namespace Civ.Orden
{
	/// <summary>
	/// Orden ir.
	/// </summary>
	[Obsolete ("Usar OrdenIrALugar")]
	public class OrdenIr : IOrden
	{
		/// <summary>
		/// Destino de la orden
		/// </summary>
		public Pseudoposición Destino;

		/// <summary>
		/// Devuelve la armada de esta orden
		/// </summary>
		/// <value>The armada.</value>
		public Armada ArmadaEjecutante { get; }

		OrdenIr (Armada armada)
		{
			ArmadaEjecutante = armada;
		}

		/// <param name="destino">Destino.</param>
		/// <param name="armada">Armada</param>
		public OrdenIr (Armada armada, Pseudoposición destino)
			: this (armada)
		{
			Destino = destino;
		}


		/// <summary>
		/// Ejecuta la orden
		/// Devuelve true si la orden ha sido terminada.
		/// </summary>
		/// <param name="t">T.</param>
		public bool Ejecutar (TimeSpan t)
		{
			Pseudoposición PS = ArmadaEjecutante.Posición;

			// Avanzar
			var Avance = (float)t.TotalHours * ArmadaEjecutante.Velocidad;
			if (PS.AvanzarHacia (Destino, ref Avance))
			{
				OnLlegar ();
				return true;
			}

			return false;
		}

		/// <summary>
		/// Raises the llegar event.
		/// </summary>
		protected virtual void OnLlegar ()
		{
			ArmadaEjecutante.CivDueño.AgregaMensaje (new Mensaje (
				"Armada llegó a su destino.",
				"Armada {0} LLegó a su destino en {1} : Orden {2}",
				TipoRepetición.ArmadaTerminaOrden,
				ArmadaEjecutante,
				ArmadaEjecutante,
				Destino,
				this));
			AlLlegar?.Invoke (this, null);
		}

		/// <summary>
		/// Occurs when al llegar.
		/// </summary>
		public event EventHandler AlLlegar;
	}
}