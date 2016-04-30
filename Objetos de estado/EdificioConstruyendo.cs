//
//  EdificioConstruyendo.cs
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
using Civ.Data;

namespace Civ
{
	[Serializable]
	/// <summary>
	/// Representa un edificio en construcción.
	/// </summary>
	public class EdificioConstruyendo
	{
		public EdificioRAW RAW;

		/// <summary>
		/// Recursos ya usados en el edificio.
		/// </summary>
		public ListaPeso<Recurso> RecursosAcumulados { get; }

		/// <summary>
		/// Devuelve una copia de la función de recursos faltantes.
		/// </summary>
		public ListaPeso<Recurso> RecursosRestantes
		{
			get
			{
				var ret = new ListaPeso<Recurso> ();
				foreach (var x in RAW.ReqRecursos)
				{
					Recurso r = x.Key;
					ret [r] = x.Value - RecursosAcumulados [r];
				}
				return ret;
			}
		}

		public Ciudad CiudadDueño { get; }

		/// <summary>
		/// Crea una instancia.
		/// </summary>
		/// <param name="raw">El RAW de este edificio.</param>
		/// <param name="ciudad">Ciudad dueño.</param>
		public EdificioConstruyendo (EdificioRAW raw, Ciudad ciudad)
		{
			RecursosAcumulados = new ListaPeso<Recurso> ();
			RAW = raw;
			CiudadDueño = ciudad;
		}

		/// <summary>
		/// Absorbe los recursos de la ciudad para su construcción.
		/// </summary>
		public void AbsorbeRecursos ()
		{
			foreach (Recurso x in RecursosRestantes.Keys)
			{
				float abs = Math.Min (RecursosRestantes [x], CiudadDueño.Almacén [x]);
				RecursosAcumulados [x] += abs;
				CiudadDueño.Almacén [x] -= abs;
			}
		}

		/// <summary>
		/// Revisa si este edificio está completado.
		/// </summary>
		/// <returns><c>true</c> si ya no quedan recursos restantes; <c>false</c> en caso contrario.</returns>
		public bool EstaCompletado ()
		{
			return RecursosRestantes.Keys.Count == 0;
		}

		/// <summary>
		/// Contruye una instancia de su RAW en la ciudad dueño.
		/// </summary>
		/// <returns>Devuelve su edificio completado.</returns>
		public Edificio Completar ()
		{
			var ret = CiudadDueño.AgregaEdificio (RAW);
			AlCompletar?.Invoke (ret);
			return ret;
		}

		/// <summary>
		/// Devuelve el procentage construido. Número en [0,1]
		/// </summary>
		/// <returns>float entre 0 y 1.</returns>
		public float PorcentageConstruccion ()
		{
			float Max = 0;
			float Act = RecursosAcumulados.SumaTotal ();

			foreach (var x in RAW.ReqRecursos.Keys)
			{
				Max += RAW.ReqRecursos [x];
			}

			return Act / Max;
		}

		/// <summary>
		/// Ocurre al completar el edificio, 
		/// justo después de crear la instancia en la ciudad.
		/// </summary>
		public event Action<Edificio> AlCompletar;
	}
}