//
//  Orden.cs
//
//  Author:
//       Edgar Carballo <karvayoEdgar@gmail.com>
//
//  Copyright (c) 2015 edgar
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Security.Cryptography;


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
		public Pseudoposicion destino;

		public OrdenIr(Pseudoposicion destino)
		{
			this.destino = destino;
		}

		public override bool Ejecutar(float t, Armada armada)
		{
			Pseudoposicion PS = armada.Posicion;
			int orientacion = 0;
			if (armada.EnTerreno)
			{
				armada.Posicion.B = destino.getExtremoNo(armada.Posicion.A);  //Asigna la posición de la armada en el intervalo correcto.

				//				PS.Destino = Destino;
				//				PS.Origen = armada.Posicion.Origen;

			}

			orientacion = PS.getOrientacion(destino);
			// Para este encontes, Posición debería ser una auténtica Pseudoposición

			// Avanzar
			PS.Avanzar(t * armada.Velocidad);

			// Si cambia de orientación, significa que llegó
			if (orientacion != PS.getOrientacion(destino))
			{
				AlLlegar(armada);
				return true;
			}


			//Revisar si están en el mismo Terreno-intervalo
			if (this.destino.Equals(PS))
			{
				armada.Posicion.A = destino.A;
				armada.Posicion.B = destino.B;
				armada.Posicion.loc = destino.loc;
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
			armada.CivDueño.AgregaMensaje(new IU.Mensaje("Armada {0} LLegó a su destino en {1} : Orden {2}", armada, destino, this));
			return;
		}
	}

}