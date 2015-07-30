//
//  InvestigandoCiencia.cs
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

namespace Civ
{
	/// <summary>
	/// Representa una entrada de una ciencia que se está investigando.
	/// </summary>
	public class InvestigandoCiencia: ListaPeso<Recurso>
	{
		/// <summary>
		/// La ciencia anclada.
		/// </summary>
		public readonly Ciencia Ciencia;

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.InvestigandoCiencia"/> class.
		/// </summary>
		/// <param name="C">Ciencia</param>
		public InvestigandoCiencia(Ciencia C) : base()
		{
			Ciencia = C;
		}

		/// <summary>
		/// Obtiene el porcentage de avance total
		/// Considerando que cada recurso vale lo mismo
		/// </summary>
		/// <returns>The pct.</returns>
		public float ObtPct()
		{
			float Max = 0; // Ciencia.Reqs.Recursos.SumaTotal();
			float Curr = 0; //SumaTotal();

			foreach (var x in Ciencia.Reqs.Recursos.Keys)
			{
				Max += Ciencia.Reqs.Recursos[x];
			}

			foreach (var x in Keys)
			{
				Curr += this[x];
			}

			return Curr / Max;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", Ciencia.Nombre, ObtPct().ToString());
		}

		/// <summary>
		/// Devuelve true si está completada.
		/// </summary>
		/// <returns><c>true</c>, if completada was estaed, <c>false</c> otherwise.</returns>
		public bool EstaCompletada()
		{
			foreach (var x in Ciencia.Reqs.Recursos.Keys)
			{
				if (this[x] < Ciencia.Reqs.Recursos[x])
					return false;
			}
			return true;
		}
	}

}

