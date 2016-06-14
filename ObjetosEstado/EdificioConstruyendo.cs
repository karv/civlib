using System;
using ListasExtra;
using Civ.RAW;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Representa un edificio en construcción.
	/// </summary>
	[Serializable]
	public class EdificioConstruyendo
	{
		#region General

		public EdificioRAW RAW;

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
		public bool EstáCompletado ()
		{
			return RecursosRestantes.SumaTotal () == 0;
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

		#endregion

		#region Recursos

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

		#endregion

		#region Base

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

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre al completar el edificio, 
		/// justo después de crear la instancia en la ciudad.
		/// </summary>
		public event Action<Edificio> AlCompletar;

		#endregion
	}
}