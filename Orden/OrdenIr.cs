using System;

namespace Civ.Orden
{
	public class OrdenIr : IOrden
	{
		/// <summary>
		/// Destino de la orden
		/// </summary>
		public Pseudoposicion Destino;

		public Armada Armada { get; }

		OrdenIr (Armada armada)
		{			
			this.Armada = armada;
		}

		/// <param name="destino">Destino.</param>
		/// <param name="armada">Armada</param>
		public OrdenIr (Armada armada, Pseudoposicion destino)
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
			Pseudoposicion PS = Armada.Posición;
			if (PS.Equals (Destino))
			{
				OnLlegar ();
				return true;

			}
			// int orientacion; // Orientación de esta posición con respecto a PS
			if (Armada.EnTerreno)
			{
				Armada.Posición.B = Destino.ExtremoNo (Armada.Posición.A);  // Asigna la posición de la armada en el intervalo correcto.
			}

			// orientacion = PS.Orientacion(Destino);
			// Para este encontes, Posición debería ser una auténtica Pseudoposición

			// Avanzar
			var Avance = (float)t.TotalHours * Armada.Velocidad;
			if (PS.AvanzarHacia (Destino, ref Avance))
			{
				OnLlegar ();
				return true;
			}

			//Revisar si están en el mismo Terreno-intervalo
			if (Destino.Equals (PS))
			{
				Armada.Posición.A = Destino.A;
				Armada.Posición.B = Destino.B;
				Armada.Posición.Loc = Destino.Loc;
				AlLlegar?.Invoke ();
				return true;
			}
			return false;
		}

		protected virtual void OnLlegar ()
		{
			Armada.CivDueño.AgregaMensaje (new IU.Mensaje (
				"Armada {0} LLegó a su destino en {1} : Orden {2}",
				Armada,
				Destino,
				this));
			AlLlegar?.Invoke ();
			return;
		}

		public event Action AlLlegar;
	}
}