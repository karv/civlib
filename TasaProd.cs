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

namespace Civ.TasaProd
{
	/// <summary>
	/// Forma en que los recursos natirales crecen
	/// </summary>
	public abstract class TasaProd
	{
		public Civ.Recurso recurso;

		#region ITickable implementation

		public abstract void Tick(IAlmacenante alm, float t);

		public abstract float DeltaEsperado(IAlmacenante alm);

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
		public float max;
		/// <summary>
		/// Aumento de recurso por hora
		/// </summary>
		public float crec;


		#region TasaProd

		public override void Tick(IAlmacenante alm, float t)
		{
			
			if (alm.Almacen.recurso(recurso) < max)
				alm.Almacen.changeRecurso(recurso, crec * t);
		}

		public override float DeltaEsperado(IAlmacenante alm)
		{
			return crec;
		}

		#endregion
		
	}

	/// <summary>
	/// Tasa prod exp.
	/// Comportamiento exponencial
	/// </summary>
	public class TasaProdExp:TasaProd
	{
		public float max;
		public float baseCrec;


		#region implemented abstract members of TasaProd

		public override void Tick(IAlmacenante alm, float t)
		{
			if (alm.Almacen.recurso(recurso) < max)
			{
				float crec = alm.obtenerRecurso(recurso) * baseCrec * t;
				alm.Almacen.changeRecurso(recurso, crec);
			}
		}

		public override float DeltaEsperado(IAlmacenante alm)
		{
			return alm.obtenerRecurso(recurso) * baseCrec;
		}

		#endregion
	}
}

