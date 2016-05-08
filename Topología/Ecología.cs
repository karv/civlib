using System.Collections.Generic;
using System;
using Civ.RAW;
using System.Linq;

namespace Civ.Topología
{
	[Serializable]
	/// <summary>
	/// Representa la ecología del terreno.
	/// </summary>
	public class Ecología : ITickable
	{
		public Ecología (IDictionary<Recurso, float> almacénInicial)
		{
			foreach (var y in almacénInicial)
				RecursoEcológico [y.Key] = y.Value;
		}

		public Ecología ()
		{
		}

		public readonly AlmacénGenérico RecursoEcológico = new AlmacénGenérico ();

		/// <summary>
		/// Propiedades innatas del lugar
		/// </summary>
		public readonly ICollection<Propiedad> Innatos = new HashSet<Propiedad> ();

		public event Action<TimeSpan> AlTickAntes;

		public event Action<TimeSpan> AlTickDespués;

		/// <summary>
		/// Devuelve el almacén de recursos.
		/// </summary>
		public IAlmacénRead AlmacénRecursos
		{
			get
			{
				return RecursoEcológico;
			}
		}

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
				foreach (var y in x.Salida.Where (z => z.Recurso.EsEcológico))
				{
					y.Tick (RecursoEcológico, t);
				}
			}
			AlTickDespués?.Invoke (t);
		}
	}
}