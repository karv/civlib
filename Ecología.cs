using System.Collections.Generic;
using Civ.Data;

namespace Civ
{
	/// <summary>
	/// Representa la ecología del terreno.
	/// </summary>
	public class Ecología : ITickable, IAlmacénRead
	{
		readonly Dictionary<Recurso, RecursoEstado> RecursoEcologico = new Dictionary<Recurso, RecursoEstado> ();

		public readonly ICollection<Propiedad> Innatos = new C5.HashSet<Propiedad> ();

		public event System.EventHandler<ListasExtra.CambioElementoEventArgs<Recurso, float>> AlCambiar;

		public float this [Recurso recurso]
		{
			get
			{
				return RecursoEcologico [recurso].Cant;
			}
			protected set
			{
				float prev = RecursoEcologico [recurso].Cant;
				RecursoEcologico [recurso].Cant = value;
				AlCambiar?.Invoke (
					this,
					new ListasExtra.CambioElementoEventArgs<Recurso, float> (
						recurso,
						prev,
						RecursoEcologico [recurso].Cant));
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

		public void Tick (System.TimeSpan t)
		{
			AlTickAntes?.Invoke (t);
			foreach (var x in RecursoEcologico)
			{
				x.Value.Cant += x.Value.Crec * (float)t.TotalHours;
			}
			AlTickDespués?.Invoke (t);

		}
	}

	public class RecursoEstado
	{
		public float Cant;
		public float Max;
		public float Crec;
	}
}