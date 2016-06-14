using ListasExtra;
using System.Runtime.Serialization;
using System;
using Civ.RAW;

namespace Civ.Ciencias
{
	/// <summary>
	/// Representa una entrada de una ciencia que se está investigando.
	/// </summary>
	[CollectionDataContract]
	[Serializable]
	public class InvestigandoCiencia : ListaPeso<Recurso>
	{
		#region General

		/// <summary>
		/// La ciencia anclada.
		/// </summary>
		public readonly Ciencia Ciencia;

		/// 

		/// <summary>
		/// Devuelve true si está completada.
		/// </summary>
		/// <returns><c>true</c>, if completada was estaed, <c>false</c> otherwise.</returns>
		public bool EstáCompletada ()
		{
			return this >= Ciencia.Reqs.Recursos;
		}

		/// <summary>
		/// Obtiene el porcentage de avance total
		/// Considerando que cada recurso vale lo mismo
		/// </summary>
		/// <returns>The pct.</returns>
		public float ObtPct ()
		{
			float Max = 0; // Ciencia.Reqs.Recursos.SumaTotal();
			float Curr = SumaTotal ();

			foreach (var x in Ciencia.Reqs.Recursos.Keys)
			{
				Max += Ciencia.Reqs.Recursos [x];
			}

			return Curr / Max;
		}

		#endregion

		#region ctor

		public InvestigandoCiencia ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.InvestigandoCiencia"/> class.
		/// </summary>
		/// <param name="ciencia">Ciencia</param>
		public InvestigandoCiencia (Ciencia ciencia)
		{
			Ciencia = ciencia;
		}

		#endregion

		#region Objeto

		public override string ToString ()
		{
			return string.Format ("{0}: {1}%", Ciencia.Nombre, ObtPct ());
		}

		#endregion
	}
}