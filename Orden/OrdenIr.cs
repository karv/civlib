using System;

namespace Civ.Orden
{
	public class OrdenIr : Orden
	{
		/// <summary>
		/// Destino de la orden
		/// </summary>
		public Pseudoposicion Destino;

		OrdenIr(Armada armada)
		{			
			Armada = armada;
		}

		/// <param name="destino">Destino.</param>
		/// <param name="armada">Armada</param>
		public OrdenIr(Armada armada, Pseudoposicion destino) : this(armada)
		{
			Destino = destino;
		}


		public override bool Ejecutar(TimeSpan t)
		{
			Pseudoposicion PS = Armada.Posicion;
			if (PS.Equals(Destino))
			{
				OnLlegar();
				return true;

			}
			int orientacion; // Orientación de esta posición con respecto a PS
			if (Armada.EnTerreno)
			{
				Armada.Posicion.B = Destino.ExtremoNo(Armada.Posicion.A);  //Asigna la posición de la armada en el intervalo correcto.

				//				PS.Destino = Destino;
				//				PS.Origen = armada.Posicion.Origen;

			}

			orientacion = PS.Orientacion(Destino);
			// Para este encontes, Posición debería ser una auténtica Pseudoposición

			// Avanzar
			var Avance = (float)t.TotalHours * Armada.Velocidad;
			PS.AvanzarHacia(Destino, ref Avance);

			// Si cambia de orientación, significa que llegó
			if (orientacion != PS.Orientacion(Destino))
			{
				OnLlegar();
				return true;
			}


			//Revisar si están en el mismo Terreno-intervalo
			if (Destino.Equals(PS))
			{
				Armada.Posicion.A = Destino.A;
				Armada.Posicion.B = Destino.B;
				Armada.Posicion.Loc = Destino.Loc;
				AlLlegar?.Invoke(Armada, null);
				return true;
			}


			return false;

		}

		protected virtual void OnLlegar()
		{
			Armada.CivDueño.AgregaMensaje(new IU.Mensaje("Armada {0} LLegó a su destino en {1} : Orden {2}", Armada, Destino, this));
			AlLlegar?.Invoke(this, null);
			return;
		}

		public event EventHandler AlLlegar;
	}
}

