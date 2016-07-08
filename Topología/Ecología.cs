using System.Collections.Generic;
using System;
using Civ.RAW;
using System.Linq;
using Civ.Almacén;
using Civ.Global;

namespace Civ.Topología
{
	/// <summary>
	/// Representa la ecología del terreno.
	/// </summary>
	[Serializable]
	public class Ecología : ITickable
	{
		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Topología.Ecología"/> class.
		/// Copia el contenido de un almacén dado a mi almacén interno.
		/// </summary>
		/// <param name="almacénInicial">Almacén inicial.</param>
		public Ecología (IDictionary<Recurso, float> almacénInicial)
		{
			foreach (var y in almacénInicial)
				RecursoEcológico [y.Key] = y.Value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Topología.Ecología"/> class.
		/// </summary>
		public Ecología ()
		{
		}

		#endregion

		#region Ecología

		/// <summary>
		/// El almacén de recursos ecológicos
		/// </summary>
		public readonly AlmacénGenérico RecursoEcológico = new AlmacénGenérico ();

		/// <summary>
		/// Propiedades innatas del lugar
		/// </summary>
		public readonly ICollection<Propiedad> Innatos = new HashSet<Propiedad> ();

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
				return new List<Recurso> (RecursoEcológico.Recursos);
			}
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre antes del tick
		/// </summary>
		public event EventHandler AlTickAntes;

		/// <summary>
		/// Ocurre después del tick
		/// </summary>
		public event EventHandler AlTickDespués;

		#endregion

		#region Tickable

		/// <summary>
		/// Ejecuta un tick
		/// </summary>
		/// <param name="t">Lapso del tick</param>
		public void Tick (TimeEventArgs t)
		{
			AlTickAntes?.Invoke (this, t);
			foreach (var x in Innatos)
			{
				foreach (var y in x.Salida.Where (z => z.Recurso.EsEcológico))
				{
					y.Tick (RecursoEcológico, t.GameTime);
				}
			}
			AlTickDespués?.Invoke (this, t);
		}

		#endregion
	}
}