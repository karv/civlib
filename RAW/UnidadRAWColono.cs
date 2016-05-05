using System;
using System.Collections.Generic;
using Civ.Orden;
using ListasExtra;
using Civ.ObjetosEstado;

namespace Civ.RAW
{
	[Serializable]
	public class UnidadRAWColono : UnidadRAW, IUnidadRAWColoniza
	{
		public UnidadRAWColono ()
		{
			RecursosPorUnidad = new ListaPeso<Recurso> ();
		}

		public ListaPeso<Recurso> RecursosPorUnidad { get; }

		/// <summary>
		/// Población con la que cada unidad se convierte en población productiva en la nueva ciudad.
		/// </summary>
		public float PoblacionACiudad;

		/// <summary>
		/// Mínica cantidad para poder colonizar
		/// </summary>
		/// <value>The edificios iniciales.</value>
		public ulong MinCantidadColonizar;

		/// <summary>
		/// Edificios con los que inicia la nueva ciudad.
		/// </summary>
		public List<EdificioRAW> EdificiosIniciales { get; set; }

		/// <summary>
		/// Coloniza aquí
		/// </summary>
		/// <returns>Devuelve la ciudad que colonizó</returns>
		/// <param name="stack">Stack.</param>
		public ICiudad Coloniza (Stack stack)
		{
			var ret = new Ciudad (stack.ArmadaPerteneciente.CivDueño, 
				          stack.Posición.A, 
				          PoblacionACiudad * stack.Cantidad);

			// Hacer los primeros edificios
			foreach (var x in EdificiosIniciales)
			{
				ret.AgregaEdificio (x);
			}

			// Los recursos
			foreach (var x in RecursosPorUnidad)
			{
				ret.Almacén [x.Key] = x.Value * stack.Cantidad;
			}

			AlColonizar?.Invoke (ret);

			return ret;
		}

		public bool PuedeColonizar (Stack stack)
		{
			return stack.Posición.EnTerreno &&
			stack.ArmadaPerteneciente.Orden is OrdenEstacionado &&
			stack.Cantidad >= MinCantidadColonizar;
		}

		/// <summary>
		/// Ocurre cuando esta unidad coloniza
		/// </summary>
		public event Action<ICiudad> AlColonizar;
	}
}