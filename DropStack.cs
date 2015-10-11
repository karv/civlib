using System;
using System.Collections.Generic;
using ListasExtra;
using Civ.Data;

namespace Civ
{
	public class DropStack: IPosicionable, IAlmacén
	{
		public DropStack (Pseudoposicion pos)
		{
			Almacén = new ListaPeso<Recurso> ();
			Posición = pos;
		}

		public ListaPeso<Recurso> Almacén { get; }

		/// <summary>
		/// Ocurre cuando cambia el almacén de un recurso
		/// Recurso, valor viejo, valor nuevo
		/// </summary>
		event EventHandler<CambioElementoEventArgs<Recurso, float>> IAlmacénRead.AlCambiar
		{
			add
			{
				Almacén.AlCambiarValor += value;
			}
			remove
			{
				Almacén.AlCambiarValor -= value;
			}
		}

		#region Posición

		public Pseudoposicion Posición { get; }

		Pseudoposicion IPosicionable.Posición ()
		{
			return Posición;
		}

		#endregion

		#region Almacén

		IEnumerable<Recurso> IAlmacénRead.recursos
		{
			get
			{
				return Almacén.Keys;
			}
		}

		float IAlmacénRead.recurso (Recurso recurso)
		{
			return Almacén [recurso];
		}

		[Obsolete]
		void IAlmacén.SetRecurso (Recurso rec, float val)
		{
			Almacén [rec] = val;
		}

		[Obsolete]
		void IAlmacén.ChangeRecurso (Recurso rec, float delta)
		{
			Almacén [rec] += delta;
		}

		float IAlmacén.this [Recurso recurso]
		{
			get
			{
				return Almacén [recurso];
			}
			set
			{
				Almacén [recurso] = value;
			}
		}

		float IAlmacénRead.this [Recurso recurso]
		{
			get
			{
				return Almacén [recurso];
			}
		}

		#endregion
	}
}

