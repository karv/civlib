using System;
using ListasExtra.Lock;
using ListasExtra;
using System.Collections.Generic;

namespace Civ
{
	public partial class Ciudad: IAlmacenante
	{
		IAlmacén IAlmacenante.Almacen
		{
			get
			{
				return (IAlmacén)Almacen;
			}
		}

		/// <summary>
		/// Almacén de recursos.
		/// </summary>
		public AlmacenCiudad Almacen;

		/// <summary>
		/// Devuelve el alimento existente en la ciudad.
		/// </summary>
		/// <value>The alimento almacén.</value>
		public float AlimentoAlmacen
		{
			get
			{
				return Almacen[RecursoAlimento];
			}
			set
			{
				Almacen.Add(RecursoAlimento, value);
			}
		}
	}

	public class AlmacenCiudad:ListaPesoBloqueable<Recurso>, IAlmacén
	{
		public AlmacenCiudad(Ciudad C)
		{
			CiudadDueño = C;
		}

		public readonly Ciudad CiudadDueño;

		/// <summary>
		/// Devuelve la cantidad de un recurso existente en ciudad.
		/// Incluye los recursos ecológicos.
		/// 
		/// O establece la cantidad de recursos de esta ciudad (o global, según el tipo de recurso).
		/// </summary>
		/// <param name="R">Recurso a contar</param>
		new public float this [Recurso R]
		{
			get
			{
				float r;

				r = base[R]; // Devuelve lo almacenado en esta ciudad.
				if (CiudadDueño.Terr.Eco.RecursoEcologico.ContainsKey(R))
					r += CiudadDueño.Terr.Eco.RecursoEcologico[R].Cant;

				return r;
			}
			set
			{
				throw new Exception("Obosoleto :c");
				if (R.EsGlobal)
				{
					CiudadDueño.CivDueno.Almacen[R] = value;
				}
				else
				{
					base[R] = value;
				}
			}
		}

		/// <summary>
		/// Revisa si el almacén posee al menos una lista de recursos.
		/// </summary>
		/// <param name="reqs">Lista de recursos para ver si posee</param>
		/// <returns>true sólo si posee tales recursos.</returns>
		public bool PoseeRecursos(ListaPeso<Recurso> reqs)
		{
			return Contains(reqs); 
			// return this >= reqs;
		}

		#region IAlmacén implementation

		public void changeRecurso(Recurso rec, float delta)
		{
			this.Add(rec, delta);
		}

		[Obsolete]
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