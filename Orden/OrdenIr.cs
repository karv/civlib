using System;

namespace Civ.Orden
{
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
			if (PS.Equals (Destino))
			{
				OnLlegar ();
				return true;

			}
			// int orientacion; // Orientación de esta posición con respecto a PS
			if (ArmadaEjecutante.EnTerreno)
			{
				ArmadaEjecutante.Posición.B = Destino.Extremos.Excepto (ArmadaEjecutante.Posición.A);  // Asigna la posición de la armada en el intervalo correcto.
			}

			// orientacion = PS.Orientacion(Destino);
			// Para este encontes, Posición debería ser una auténtica Pseudoposición

			// Avanzar
			var Avance = (float)t.TotalHours * ArmadaEjecutante.Velocidad;
			PS.EnMismoIntervalo (Destino);
			if (PS.AvanzarHacia (Destino, ref Avance))
			{
				OnLlegar ();
				return true;
			}

			//Revisar si están en el mismo Terreno-intervalo
			if (Destino.Equals (PS))
			{
				ArmadaEjecutante.Posición.A = Destino.A;
				ArmadaEjecutante.Posición.B = Destino.B;
				ArmadaEjecutante.Posición.Loc = Destino.Loc;
				AlLlegar?.Invoke ();
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