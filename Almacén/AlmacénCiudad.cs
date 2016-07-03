using System;
using Civ.RAW;
using Civ.ObjetosEstado;
using Civ.Global;

namespace Civ.Almacén
{
	/// <summary>
	/// Representa el almacén de recursos de una ciudad,
	/// manipulando y controlando los recursos ecológicos y genéricos.
	/// </summary>
	[Serializable]
	public class AlmacénCiudad : AlmacénGenérico, IAlmacén
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

		public new float this [int id]
		{
			get
			{
				return base [id] + CiudadDueño.Terr.Eco.AlmacénRecursos [Juego.Data.Recursos [id]];
			}
			set
			{
				var recurso = Juego.Data.Recursos [id];
				if (recurso.EsGlobal)
					CiudadDueño.CivDueño.Almacén [recurso] = value;
				else if (recurso.EsEcológico)
					CiudadDueño.Terr.Eco.RecursoEcológico [recurso] = value;
				else
				{
					if (float.IsNaN (value))
						System.Diagnostics.Debugger.Break ();
					base [id] = value;
				}
			}
		}

		/// <summary>
		/// Revisa si el almacén posee al menos una lista de recursos.
		/// </summary>
		/// <param name="reqs">Lista de recursos para ver si posee</param>
		/// <param name="veces">Cuántas veces contiene estos requisitos</param>
		/// <returns>true sólo si posee tales recursos.</returns>
		public bool PoseeRecursos (IAlmacénRead reqs, ulong veces = 1)
		{
			
			return ContieneRecursos (reqs) >= veces;
		}

		#endregion

	}
}