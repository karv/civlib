using System;
using System.Collections.Generic;
using ListasExtra;

namespace Civ
{
	public partial class Civilizacion
	{

		// Avances
		/// <summary>
		/// Lista de avances de la civilización
		/// </summary>
		public List<Ciencia> Avances = new List<Ciencia>();
		/// <summary>
		/// Ciencias que han sido parcialmente investigadas.
		/// </summary>
		public ListaPeso<Ciencia, ListaPeso<Recurso>> Investigando
			= new ListaPeso<Ciencia, ListaPeso<Recurso>>(null, new ListaPeso<Recurso>());

		/// <summary>
		/// Devuelve las ciencias que no han sido investigadas y que comple todos los requesitos para investigarlas.
		/// </summary>
		public List<Ciencia> CienciasAbiertas()
		{
			List<Ciencia> ret = new List<Ciencia>();
			foreach (Ciencia x in Global.g_.Data.Ciencias)
			{
				if (EsCienciaAbierta(x))
				{
					ret.Add(x);
				}
			}
			return ret;
		}

		/// <summary>
		/// Revisa si una ciencia se puede investigar.
		/// </summary>
		/// <param name="C">Una ciencia</param>
		/// <returns><c>true</c> si la ciencia se puede investigar; <c>false</c> si no.</returns>
		bool EsCienciaAbierta(Ciencia C)
		{
			return !Avances.Contains(C) && C.Reqs.Ciencias.TrueForAll(z => Avances.Contains(z));
		}

		/// <summary>
		/// Devuelve true sólo si la ciencia ya está completada.
		/// </summary>
		/// <returns><c>true</c>, if requerimientos recursos was satisfaced, <c>false</c> otherwise.</returns>
		/// <param name="C">C.</param>
		bool SatisfaceRequerimientosRecursos(Ciencia C)
		{
			// Si ya se conoce la ciencia, entonces devuelve true.
			if (Avances.Contains(C))
				return true;
			foreach (var x in C.Reqs.Recursos.Data.Keys)
			{
				if (Investigando[C][x] < C.Reqs.Recursos[x])
				{
					return false;
				}
			}
			return true;
		}
	}
}
