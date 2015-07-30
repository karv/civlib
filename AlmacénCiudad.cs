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
using System;
using ListasExtra;
using System.Collections.Generic;

namespace Civ
{
	public class AlmacenCiudad: ListaPeso<Recurso>, IAlmacén
	{
		public AlmacenCiudad(Ciudad C) : base()
		{
			CiudadDueño = C;
		}

		public readonly Ciudad CiudadDueño;

		/// <summary>
		/// Devuelve la cantidad de un recurso existente en ciudad.
		/// Incluye los recursos ecológicos.
		/// 
		/// O establece la cantidad de recursos de esta ciudad (o global, según el tipo de recurso).
		/// </summary>
		/// <param name="R">Recurso a contar</param>
		new public float this [Recurso R]
		{
			get
			{
				float r;

				r = base[R]; // Devuelve lo almacenado en esta ciudad.
				if (CiudadDueño.Terr.Eco.RecursoEcologico.ContainsKey(R))
					r += CiudadDueño.Terr.Eco.RecursoEcologico[R].Cant;

				return r;
			}
			set
			{
				// System.Diagnostics.Debug.WriteLine("AlmacénCiudad[R].set es Obsoleto");
				if (R.EsGlobal)
				{
					CiudadDueño.CivDueno.Almacen[R] = value;
				}
				else
				{
					if (float.IsNaN(value))
						System.Diagnostics.Debugger.Break();
					base[R] = value;
				}
			}
		}

		/// <summary>
		/// Revisa si el almacén posee al menos una lista de recursos.
		/// </summary>
		/// <param name="reqs">Lista de recursos para ver si posee</param>
		/// <returns>true sólo si posee tales recursos.</returns>
		public bool PoseeRecursos(ListaPeso<Recurso> reqs)
		{
			return this >= reqs;
		}

		#region IAlmacén implementation

		public void changeRecurso(Recurso rec, float delta)
		{
			this[rec] += delta;
			//this.Add(rec, delta);
		}

		void IAlmacén.setRecurso(Recurso rec, float val)
		{
			this[rec] = val;
		}

		#endregion

		#region IAlmacénRead implementation

		float IAlmacénRead.recurso(Recurso R)
		{
			return this[R];
		}

		IEnumerable<Recurso> IAlmacénRead.recursos
		{
			get
			{
				return Keys;
			}
		}

		#endregion
	}
}

