using ListasExtra;
using System.Collections.Generic;
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
	public class AlmacénCiudad : ListaPeso<Recurso>, IAlmacén
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
		/// <param name="recurso">Recurso a contar</param>
		new public float this [Recurso recurso]
		{
			get
			{
				float r;

				r = base [recurso]; // Devuelve lo almacenado en esta ciudad.
				if (CiudadDueño.Terr.Eco.ListaRecursos.Contains (recurso))
					r += CiudadDueño.Terr.Eco.AlmacénRecursos [recurso];

				return r;
			}
			set
			{
				if (recurso.EsGlobal)
				{
					CiudadDueño.CivDueño.Almacén [recurso] = value;
				}
				else if (recurso.EsEcológico)
				{
					CiudadDueño.Terr.Eco.RecursoEcológico [recurso] = value;
				}
				else
				{
					if (float.IsNaN (value))
						System.Diagnostics.Debugger.Break ();
					base [recurso] = value;
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

		#region Eventos

		/// <summary>
		/// Ocurre cuando cambia el almacén de un recurso
		/// Recurso, valor viejo, valor nuevo
		/// </summary>
		event EventHandler<CambioElementoEventArgs<Recurso, float>> IAlmacénRead.AlCambiar
		{
			add
			{
				AlCambiarValor += value;
			}
			remove
			{
				AlCambiarValor -= value;
			}
		}

		#endregion

		#region IAlmacénRead implementation

		IEnumerable<Recurso> IAlmacénRead.Recursos
		{
			get
			{
				return Keys;
			}
		}

		#endregion
	}
}