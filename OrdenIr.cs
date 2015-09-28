namespace Civ.Orden
{
	public class OrdenIr : Orden
	{
		/// <summary>
		/// Destino de la orden
		/// </summary>
		public Pseudoposicion Destino;

		/// <param name="destino">Destino.</param>
		public OrdenIr(Pseudoposicion destino)
		{
			Destino = destino;
		}

		public override bool Ejecutar(float t, Armada armada)
		{
			Pseudoposicion PS = armada.Posicion;
			int orientacion; // Orientación de esta posición con respecto a PS
			if (armada.EnTerreno)
			{
				armada.Posicion.B = Destino.ExtremoNo(armada.Posicion.A);  //Asigna la posición de la armada en el intervalo correcto.

				//				PS.Destino = Destino;
				//				PS.Origen = armada.Posicion.Origen;

			}

			orientacion = PS.Orientacion(Destino);
			// Para este encontes, Posición debería ser una auténtica Pseudoposición

			// Avanzar
			PS.Avanzar(t * armada.Velocidad);

			// Si cambia de orientación, significa que llegó
			if (orientacion != PS.Orientacion(Destino))
			{
				AlLlegar(armada);
				return true;
			}


			//Revisar si están en el mismo Terreno-intervalo
			if (Destino.Equals(PS))
			{
				armada.Posicion.A = Destino.A;
				armada.Posicion.B = Destino.B;
				armada.Posicion.loc = Destino.loc;
				AlLlegar(armada);
				return true;
			}

			/*
			if (PS.A == destino.A && PS.B == destino.B) // Esto debe pasar siempre, por ahora.
			{
				if (PS.loc >= destino.loc)
				{
					// Ya llegó.
					armada.CivDueño.AgregaMensaje(new IU.Mensaje("Armada {0} LLegó a su destino en {1} : Orden {2}", armada, destino, this));
					return true;
					// Orden = null;
				}
			} 
*/
			return false;

		}

		protected virtual void AlLlegar(Armada armada)
		{
			armada.CivDueño.AgregaMensaje(new IU.Mensaje("Armada {0} LLegó a su destino en {1} : Orden {2}", armada, Destino, this));
			return;
		}
	}
}

