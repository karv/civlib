using System;
using System.Collections.Generic;
using ListasExtra;
using Civ.Data;

namespace Civ
{
	/// <summary>
	/// Representa un conjunto de recursos que no están (necesariamente) en una ciudad.
	/// </summary>
	public class DropStack : IPosicionable, IAlmacén
	{
		public DropStack (Pseudoposición pos)
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

		public Pseudoposición Posición { get; }

		Pseudoposición IPosicionable.Posición ()
		{
			return Posición;
		}

		#endregion

		#region Almacén

		IEnumerable<Recurso> IAlmacénRead.Recursos
		{
			get
			{
				return Almacén.Keys;
			}
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

