//
//  AlmacénCiudad.cs
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

using ListasExtra;
using System.Collections.Generic;
using System;
using Civ.RAW;
using Civ.ObjetosEstado;

namespace Civ.Almacén
{
	[Serializable]
	public class AlmacénCiudad: ListaPeso<Recurso>, IAlmacén
	{
		/// <summary>
		/// Initializes a new instance
		/// </summary>
		/// <param name="ciudad">Ciudad de este almacén</param>
		public AlmacénCiudad (Ciudad ciudad)
		{
			CiudadDueño = ciudad;
		}

		public readonly Ciudad CiudadDueño;

		/// <summary>
		/// Devuelve la cantidad de un recurso existente en ciudad.
		/// Incluye los recursos ecológicos.
		/// 
		/// O establece la cantidad de recursos de esta ciudad (o global, según el tipo de recurso).
		/// </summary>
		/// <param name="recurso">Recurso a contar</param>
		new public float this [Recurso recurso]
		{
			get
			{
				float r;

				r = base [recurso]; // Devuelve lo almacenado en esta ciudad.
				if (CiudadDueño.Terr.Eco.ListaRecursos.Contains (recurso))
					r += CiudadDueño.Terr.Eco.AlmacénRecursos [recurso];

				return r;
			}
			set
			{
				if (recurso.EsGlobal)
				{
					CiudadDueño.CivDueño.Almacén [recurso] = value;
				}
				else
				{
					if (float.IsNaN (value))
						System.Diagnostics.Debugger.Break ();
					base [recurso] = value;
				}
			}
		}

		/// <summary>
		/// Revisa si el almacén posee al menos una lista de recursos.
		/// </summary>
		/// <param name="reqs">Lista de recursos para ver si posee</param>
		/// <param name="veces">Cuántas veces contiene estos requisitos</param>
		/// <returns>true sólo si posee tales recursos.</returns>
		public bool PoseeRecursos (ListaPeso<Recurso> reqs, ulong veces = 1)
		{
			return this >= reqs * veces;
		}

		/// <summary>
		/// Ocurre cuando cambia el almacén de un recurso
		/// Recurso, valor viejo, valor nuevo
		/// </summary>
		event EventHandler<CambioElementoEventArgs<Recurso, float>> IAlmacénRead.AlCambiar
		{
			add
			{
				AlCambiarValor += value;
			}
			remove
			{
				AlCambiarValor -= value;
			}
		}

		#region IAlmacénRead implementation

		IEnumerable<Recurso> IAlmacénRead.Recursos
		{
			get
			{
				return Keys;
			}
		}

		#endregion
	}
}