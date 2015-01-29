using System;
using System.Collections.Generic;

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
		public ListasExtra.ListaPeso<Ciencia> Investigando = new ListasExtra.ListaPeso<Ciencia>();

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
			// return !Avances.Contains(C) && C.ReqCiencia.TrueForAll(z => Avances.Exists(w => (w.Nombre == z)));
			return !Avances.Contains(C) && C.Reqs.Ciencias.TrueForAll(z => Avances.Contains(z));
		}
	}
}

