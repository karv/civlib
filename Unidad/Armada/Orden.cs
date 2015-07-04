using System;

namespace Civ
{

	public partial class Armada
	{
		public Civ.Orden.Orden Orden;
	}
}

namespace Civ.Orden
{
	/// <summary>
	/// Representa una orden de una armada
	/// </summary>
	public abstract class Orden
	{
		/// <summary>
		/// Ejecutar the specified t and armada.
		/// Devuelve true si la orden ha sido terminada.
		/// </summary>
		/// <param name="t">T.</param>
		/// <param name="armada">Armada.</param>
		public abstract bool Ejecutar(float t, Armada armada);
	}

	public class OrdenIr : Orden
	{
		public Pseudoposicion Destino;

		public override bool Ejecutar(float t, Armada armada)
		{
			Pseudoposicion PS;
			if (armada.EnTerreno)
			{
				// Convertir Posición en Pseudoposición.
				// TODO
				PS = new Pseudoposicion();
				PS.Avance = 0;
//				PS.Destino = Destino;
//				PS.Origen = armada.Posicion.Origen;

				armada.Posicion = PS;
			}
			else
			{
				PS = armada.Posicion;
			}

			// Para este encontes, Posición debería ser una auténtica Pseudoposición

			// Avanzar
			PS.Avance += t * armada.Velocidad;


			//Revisar si están en el mismo Terreno-intervalo
			//TODO
			/*
			if (PS.Origen == Destino.Origen && PS.Destino == Destino.Destino) // Esto debe pasar siempre, por ahora.
			{
				if (PS.Avance >= Destino.Avance)
				{
					// Ya llegó.
					armada.CivDueño.AgregaMensaje(new IU.Mensaje("Armada {0} LLegó a su destino en {1} : Orden {2}", armada, Destino, this));
					return true;
					// Orden = null;
				}
			}
*/
			return false;
		}
	}

	public class OrdenEstacionado : Orden
	{
		public override bool Ejecutar(float t, Armada armada)
		{
			return false;
		}
	}
}