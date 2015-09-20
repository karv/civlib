//
//  AlmacénCiv.cs
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
using System.Linq;

namespace Civ
{
	/// <summary>
	/// Almacena recursos globales.
	/// </summary>
	public class AlmacénCiv:ListaPeso<Recurso>, IAlmacén
	{
		public readonly ICivilizacion Civil;

		public AlmacénCiv(ICivilizacion C) : base()
		{
			Civil = C;
		}

		/// <summary>
		/// Elimina los recursos con la flaf "Desaparece"
		/// </summary>
		public void RemoverRecursosDesaparece()
		{
			foreach (var x in Entradas)
			{
				if (x.Desaparece)
					this[x] = 0;
			}
		}

		/// <summary>
		/// Devuelve una copia de la lista de entradas.
		/// </summary>
		public Recurso[] Entradas
		{
			get
			{
				return Keys.ToArray<Recurso>();
			}
		}

		/// <summary>
		/// Devuelve la cantidad de recursos presentes.
		/// Si R es global devuelve su valor "as is".
		/// Si R no es globa, suma los almacenes de cada ciudad.
		/// 
		/// O establece los recursos globales del almacén global.
		/// </summary>
		/// <param name="R">Recurso</param>
		new public float this [Recurso R]
		{
			get
			{
				if (R.EsGlobal)
				{
					return base[R];
				}
				else
				{
					float ret = 0;
					foreach (var x in Civil.Ciudades)
					{
						ret += x.Almacen.recurso(R);
					}
					return ret;
				}
			}
			set
			{
				if (R.EsGlobal)
					base[R] = value;
				else
				{
					throw new Exception(string.Format("Sólo se pueden almacenar recursos globales en AlmacenCiv.\n{0} no es global.", R));
				}
			}
		}

		#region IAlmacénRead implementation

		float IAlmacénRead.recurso(Recurso R)
		{
			return this[R];
		}

		System.Collections.Generic.IEnumerable<Recurso> IAlmacénRead.recursos
		{
			get
			{
				return Keys;
			}
		}

		#endregion

		#region IAlmacén implementation

		public void changeRecurso(Recurso rec, float delta)
		{
			this.Add(rec, delta);
		}

		[Obsolete]
		void IAlmacén.setRecurso(Recurso rec, float val)
		{
			this[rec] = val;
		}

		#endregion
	}
}