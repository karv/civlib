using System;
using System.Collections.Generic;
using Civ.Orden;
using ListasExtra;
using Civ.ObjetosEstado;

namespace Civ.RAW
{
	/// <summary>
	/// Un tipo de unidad que puede colonizar
	/// </summary>
	[Serializable]
	public class UnidadRAWColono : UnidadRAW, IUnidadRAWColoniza
	{
		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.RAW.UnidadRAWColono"/> class.
		/// </summary>
		public UnidadRAWColono ()
		{
			RecursosPorUnidad = new ListaPeso<Recurso> ();
		}

		#endregion

		#region Colonizar

		/// <summary>
		/// Recursos que cada unidad aporta a una ciudad recién construida
		/// </summary>
		/// <value>The recursos por unidad.</value>
		public ListaPeso<Recurso> RecursosPorUnidad { get; }

		/// <summary>
		/// Población con la que cada unidad se convierte en población productiva en la nueva ciudad.
		/// </summary>
		public float PoblacionACiudad;

		/// <summary>
		/// Mínica cantidad para poder colonizar
		/// </summary>
		/// <value>The edificios iniciales.</value>
		public long MinCantidadColonizar;

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

			AlColonizar?.Invoke (this, new CiudadEventArgs (ret));

			return ret;
		}

		/// <summary>
		/// Revisa si un Stack puede colonizar en este momento y lugar
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		/// <returns><c>true</c>, if colonizar was pueded, <c>false</c> otherwise.</returns>
		/// <param name="stack">Stack que quiere colonizar.</param>
		public bool PuedeColonizar (Stack stack)
		{
			return stack.Posición.EnTerreno &&
			stack.ArmadaPerteneciente.Orden is OrdenEstacionado &&
			stack.Cantidad >= MinCantidadColonizar;
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre cuando esta unidad coloniza
		/// </summary>
		public event EventHandler<CiudadEventArgs> AlColonizar;

		#endregion
	}
}