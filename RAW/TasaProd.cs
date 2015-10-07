//
//  TasaProd.cs
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
using Civ.Data;

namespace Civ.Data.TasaProd
{
	/// <summary>
	/// Forma en que los recursos natirales crecen
	/// </summary>
	public abstract class TasaProd
	{
		public Recurso Recurso;

		#region ITickable implementation

		public abstract void Tick (IAlmacenante alm, TimeSpan t);

		public abstract float DeltaEsperado (IAlmacenante alm);

		#endregion
	}

	/// <summary>
	/// Tasa prod constante.
	/// Comportamiento lineal
	/// </summary>
	public class TasaProdConstante: TasaProd
	{
		/// <summary>
		/// Máximo del recurso que puede ofrecer esta tasa de crecimiento
		/// </summary>
		public float Max;
		/// <summary>
		/// Aumento de recurso por hora
		/// </summary>
		public float Crecimiento;


		#region TasaProd

		public override void Tick (IAlmacenante alm, TimeSpan t)
		{
			
			if (alm.Almacen [Recurso] < Max)
				alm.Almacen [Recurso] += Crecimiento * (float)t.TotalHours;
		}

		public override float DeltaEsperado (IAlmacenante alm)
		{
			return Crecimiento;
		}

		#endregion
		
	}

	/// <summary>
	/// Tasa prod exp.
	/// Comportamiento exponencial
	/// </summary>
	public class TasaProdExp:TasaProd
	{
		public float Max;
		public float CrecimientoBase;


		#region implemented abstract members of TasaProd

		public override void Tick (IAlmacenante alm, TimeSpan t)
		{
			if (alm.Almacen [Recurso] < Max)
				alm.Almacen [Recurso] += CrecimientoBase * (float)t.TotalHours;
			
		}

		public override float DeltaEsperado (IAlmacenante alm)
		{
			return alm.ObtenerRecurso (Recurso) * CrecimientoBase;
		}

		#endregion
	}
}