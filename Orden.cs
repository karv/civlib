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

}