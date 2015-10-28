using System;

namespace Civ.Orden
{
	[Obsolete ("Usar OrdenIrALugar")]
	public class OrdenIr : IOrden
	{
		/// <summary>
		/// Destino de la orden
		/// </summary>
		public Pseudoposición Destino;

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

		protected virtual void OnLlegar ()
		{
			ArmadaEjecutante.CivDueño.AgregaMensaje (new IU.Mensaje (
				"Armada {0} LLegó a su destino en {1} : Orden {2}",
				ArmadaEjecutante,
				Destino,
				this));
			AlLlegar?.Invoke ();
		}

		public event Action AlLlegar;
	}
}