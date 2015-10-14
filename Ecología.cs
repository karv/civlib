using System.Collections.Generic;
using Civ.Data;
using ListasExtra;

namespace Civ
{
	/// <summary>
	/// Representa la ecología del terreno.
	/// </summary>
	public class Ecología : ITickable, IAlmacénRead
	{
		readonly ListaPeso<Recurso> RecursoEcologico = new ListaPeso<Recurso> ();

		public readonly ICollection<Propiedad> Innatos = new C5.HashSet<Propiedad> ();

		public event System.EventHandler<ListasExtra.CambioElementoEventArgs<Recurso, float>> AlCambiar;

		public float this [Recurso recurso]
		{
			get
			{
				return RecursoEcologico [recurso];
			}
			protected set
			{
				float prev = RecursoEcologico [recurso];
				RecursoEcologico [recurso] = value;
				AlCambiar?.Invoke (
					this,
					new ListasExtra.CambioElementoEventArgs<Recurso, float> (
						recurso,
						prev,
						RecursoEcologico [recurso]));
			}
		}

		public ICollection<Recurso> recursos
		{
			get
			{
				return RecursoEcologico.Keys;
			}
		}

		IEnumerable<Recurso> IAlmacénRead.recursos
		{
			get
			{
				return RecursoEcologico.Keys;
			}
		}

		public event System.Action<System.TimeSpan> AlTickAntes;

		public event System.Action<System.TimeSpan> AlTickDespués;

		public void Tick (System.TimeSpan t) //TEST
		{
			AlTickAntes?.Invoke (t);
			foreach (var x in Innatos)
			{
				foreach (var y in x.Salida)
				{
					RecursoEcologico [y.Recurso] += y.DeltaEsperado (this) * (float)t.TotalHours;
				}
			}
			AlTickDespués?.Invoke (t);

		}
	}
}