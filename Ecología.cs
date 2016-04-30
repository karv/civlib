using System.Collections.Generic;
using Civ.Data;
using System;

namespace Civ
{
	[Serializable]
	/// <summary>
	/// Representa la ecología del terreno.
	/// </summary>
	public class Ecología : ITickable
	{
		readonly AlmacénGenérico RecursoEcológico = new AlmacénGenérico ();

		/// <summary>
		/// Propiedades innatas del lugar
		/// </summary>
		public readonly ICollection<Propiedad> Innatos = new HashSet<Propiedad> ();

		public event Action<TimeSpan> AlTickAntes;

		public event Action<TimeSpan> AlTickDespués;

		/// <summary>
		/// Devuelve el almacén de recursos.
		/// </summary>
		public IAlmacénRead AlmacénRecursos { get; }

		/// <summary>
		/// Devuelve la lista de los recursos en esta Ecología
		/// </summary>
		public ICollection<Recurso> ListaRecursos
		{
			get
			{
				return RecursoEcológico.Keys;
			}
		}

		/// <summary>
		/// Ejecuta un tick
		/// </summary>
		/// <param name="t">Lapso del tick</param>
		public void Tick (TimeSpan t)
		{
			AlTickAntes?.Invoke (t);
			foreach (var x in Innatos)
			{
				foreach (var y in x.Salida)
				{
					if (y.Recurso.EsEcológico) // Sólo hace esto para recursos ecológicos
						y.Tick (RecursoEcológico, t);
				}
			}
			AlTickDespués?.Invoke (t);
		}
	}
}