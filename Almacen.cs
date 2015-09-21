using System;
using ListasExtra;
using System.Collections.Generic;

namespace Civ
{
	/// <summary>
	/// Representa un almacén genérico.
	/// </summary>
	public class Almacen:ListaPeso<Recurso>, IAlmacén
	{
		public Almacen()
		{
		}

		/// <summary>
		/// Revisa si el almacén posee al menos una lista de recursos.
		/// </summary>
		/// <param name="reqs">Lista de recursos para ver si posee</param>
		/// <returns>true sólo si posee tales recursos.</returns>
		public bool PoseeRecursos(ListaPeso<Recurso> reqs)
		{
			return this >= reqs;
		}

		/// <summary>
		/// Revisa si el almacén posee al menos una lista de recursos.
		/// </summary>
		/// <param name="reqs">Lista de recursos para ver si posee</param>
		/// <param name="Veces">Cuántas veces contiene estos requisitos</param>
		/// <returns>true sólo si posee tales recursos.</returns>
		public bool PoseeRecursos(ListaPeso<Recurso> reqs, ulong Veces)
		{
			foreach (var item in reqs)
			{
				if (this[item.Key] < item.Value * Veces)
					return false;
			}
			return true;
		}

		#region IAlmacén implementation

		public void changeRecurso(Recurso rec, float delta)
		{
			this[rec] += delta;
			//this.Add(rec, delta);
		}

		void IAlmacén.setRecurso(Recurso rec, float val)
		{
			this[rec] = val;
		}

		#endregion

		#region IAlmacénRead implementation

		float IAlmacénRead.recurso(Recurso R)
		{
			return this[R];
		}

		IEnumerable<Recurso> IAlmacénRead.recursos
		{
			get
			{
				return Keys;
			}
		}

		#endregion

	}
}

