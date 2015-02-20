using System;
using ListasExtra;
using System.Collections.Generic;

namespace Civ
{
	public partial class Ciudad
	{
		//Almacén
		/// <summary>
		/// Almacén de recursos.
		/// </summary>
		public AlmacenCiudad Almacén;

		/// <summary>
		/// Devuelve el alimento existente en la ciudad.
		/// </summary>
		/// <value>The alimento almacén.</value>
		public float AlimentoAlmacén
		{
			get
			{
				return Almacén[RecursoAlimento];
			}
			set
			{
				Almacén[RecursoAlimento] = value;
			}
		}
	}

	public class AlmacenCiudad:ListaPeso<Recurso>
	{
		public AlmacenCiudad(Ciudad C)
		{
			CiudadDueño = C;
		}

		public readonly Ciudad CiudadDueño;

		Terreno.Ecologia Eco
		{
			get
			{
				return CiudadDueño.Terr.Eco;
			}
		}

		/// <summary>
		/// Devuelve la cantidad de un recurso existente en ciudad, mas los recursos globales.
		/// 
		/// O establece la cantidad de recursos de esta ciudad (o global, según el tipo de recurso).
		/// </summary>
		/// <param name="R">Recurso a contar</param>
		new public float this [Recurso R]
		{
			get
			{
				float r;

				r = base[R] + CiudadDueño.CivDueño.Almacen[R]; // Devuelve lo almacenado en esta ciudad más los recursos globales.
				if (CiudadDueño.Terr.Eco.RecursoEcologico.ContainsKey(R))
					r += CiudadDueño.Terr.Eco.RecursoEcologico[R].Cant;

				return r;
			}
			set
			{
				if (R.EsGlobal)
				{
					CiudadDueño.CivDueño.Almacen[R] += value;
				}
				else
				{
					base[R] += value;
				}
			}
		}
	}
}

