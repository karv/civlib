using ListasExtra;
using System;
using Civ.RAW;
using Civ.ObjetosEstado;

namespace Civ.Almacén
{
	/// <summary>
	/// Representa el almacén de recursos de una ciudad,
	/// manipulando y controlando los recursos ecológicos y genéricos.
	/// </summary>
	[Serializable]
	public class AlmacénCiudad : AlmacénGenérico
	{
		#region ctor

		/// <summary>
		/// Initializes a new instance
		/// </summary>
		/// <param name="ciudad">Ciudad de este almacén</param>
		public AlmacénCiudad (Ciudad ciudad)
		{
			CiudadDueño = ciudad;
		}

		#endregion

		#region General

		/// <summary>
		/// Ciudad que posee este almacén
		/// </summary>
		public readonly Ciudad CiudadDueño;

		/// <summary>
		/// Devuelve la cantidad de un recurso existente en ciudad.
		/// Incluye los recursos ecológicos.
		/// 
		/// O establece la cantidad de recursos de esta ciudad (o global, según el tipo de recurso).
		/// </summary>
		/// <param name="rec">Recurso a contar</param>
		public override float this [Recurso rec]
		{
			get
			{
				float r;

				r = base [rec]; // Devuelve lo almacenado en esta ciudad.
				if (CiudadDueño.Terr.Eco.ListaRecursos.Contains (rec))
					r += CiudadDueño.Terr.Eco.AlmacénRecursos [rec];

				return r;
			}
			set
			{
				if (rec.EsGlobal)
				{
					CiudadDueño.CivDueño.Almacén [rec] = value;
				}
				else if (rec.EsEcológico)
				{
					CiudadDueño.Terr.Eco.RecursoEcológico [rec] = value;
				}
				else
				{
					if (float.IsNaN (value))
						System.Diagnostics.Debugger.Break ();
					base [rec] = value;
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

		#endregion

	}
}