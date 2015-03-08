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
		public ListaInvestigacion Investigando = new ListaInvestigacion();

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
			InvestigandoCiencia I = Investigando.EncuentraInstancia(C);
			if (I == null)
				return false; // Si no se empieza a investigar aún, regresa false.
			return I.EstaCompletada(); // Si está en la lista, revisar si está completada.
		}
	}

	/// <summary>
	/// Representa una entrada de una ciencia que se está investigando.
	/// </summary>
	public class InvestigandoCiencia: ListaPeso<Recurso>
	{
		/// <summary>
		/// La ciencia alclada.
		/// </summary>
		public readonly Ciencia Ciencia;

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.InvestigandoCiencia"/> class.
		/// </summary>
		/// <param name="C">Ciencia</param>
		public InvestigandoCiencia(Ciencia C)
		{
			Ciencia = C;
		}

		/// <summary>
		/// Obtiene el porcentage de avance total
		/// Considerando que cada recurso vale lo mismo
		/// </summary>
		/// <returns>The pct.</returns>
		public float ObtPct()
		{
			float Max = 0; // Ciencia.Reqs.Recursos.SumaTotal();
			float Curr = 0; //SumaTotal();

			foreach (var x in Ciencia.Reqs.Recursos.Keys)
			{
				Max += Ciencia.Reqs.Recursos[x];
			}

			foreach (var x in Keys)
			{
				Curr += this[x];
			}

			return Curr / Max;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", Ciencia.Nombre, ObtPct().ToString());
		}

		/// <summary>
		/// Devuelve true si está completada.
		/// </summary>
		/// <returns><c>true</c>, if completada was estaed, <c>false</c> otherwise.</returns>
		public bool EstaCompletada()
		{
			foreach (var x in Ciencia.Reqs.Recursos.Keys)
			{
				if (this[x] < Ciencia.Reqs.Recursos[x])
					return false;
			}
			return true;
		}
	}

	/// <summary>
	/// Representa la lista de ciencias que se están investigando.
	/// </summary>
	public class ListaInvestigacion:List<InvestigandoCiencia>
	{
		/// <summary>
		/// Agrega cierta cantidad de recursos, a la investigación de una ciencia.
		/// </summary>
		/// <param name="C">Ciencia investigando.</param>
		/// <param name="R">Recurso del que se agrega.</param>
		/// <param name="Cantidad">Cantidad de tal recurso.</param>
		public void Invertir(Ciencia C, Recurso R, float Cantidad)
		{
			if (!Exists(x => x.Ciencia == C)) // Si no existe la ciencia C en la lista, se agrega
				Add(new InvestigandoCiencia(C));

			InvestigandoCiencia IC = Find(x => x.Ciencia == C); //IC es la correspondiente a la ciencia C.
			IC[R] += Cantidad;
		}

		/// <summary>
		/// Encuentra la instancia (si existe) de una ciencia.
		/// </summary>
		/// <returns>The instancia.</returns>
		/// <param name="C">Ciencia a buscar</param>
		public InvestigandoCiencia EncuentraInstancia(Ciencia C)
		{
			return Find(x => x.Ciencia == C);
		}
	}
}