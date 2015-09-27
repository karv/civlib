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
			this.Armada = armada;
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
			int orientacion; // Orientación de esta posición con respecto a PS
			if (Armada.EnTerreno)
			{
				Armada.Posicion.B = Destino.ExtremoNo(Armada.Posicion.A);  //Asigna la posición de la armada en el intervalo correcto.

				//				PS.Destino = Destino;
				//				PS.Origen = armada.Posicion.Origen;

			}

			orientacion = PS.Orientacion(Destino);
			// Para este encontes, Posición debería ser una auténtica Pseudoposición

			// Si está Atrás, ejecutar PS.Invertir();
			PS.Hacerconsistente(Destino);
			if (PS.loc > Destino.loc)
				t = -t;
			
			// Avanzar
			PS.Avanzar((float)t.TotalHours * Armada.Velocidad);

			// Si cambia de orientación, significa que llegó
			if (orientacion != PS.Orientacion(Destino))
			{
				AlLlegar(Armada);
				return true;
			}


			//Revisar si están en el mismo Terreno-intervalo
			if (Destino.Equals(PS))
			{
				Armada.Posicion.A = Destino.A;
				Armada.Posicion.B = Destino.B;
				Armada.Posicion.loc = Destino.loc;
				AlLlegar(Armada);
				return true;
			}


			return false;

		}

		protected virtual void AlLlegar(Armada armada)
		{
			armada.CivDueño.AgregaMensaje(new IU.Mensaje("Armada {0} LLegó a su destino en {1} : Orden {2}", armada, Destino, this));
			return;
		}
	}
}

