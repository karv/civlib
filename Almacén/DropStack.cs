using System;
using System.Collections.Generic;
using ListasExtra;
using Civ.Topología;
using Civ.RAW;

namespace Civ.Almacén
{
	/// <summary>
	/// Representa un conjunto de recursos que no están (necesariamente) en una ciudad.
	/// </summary>
	[Serializable]
	public class DropStack : IPosicionable, IAlmacén
	{
		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Almacén.DropStack"/> class.
		/// </summary>
		/// <param name="pos">Position.</param>
		public DropStack (Pseudoposición pos)
		{
			Almacén = new ListaPeso<Recurso> ();
			Posición = pos;
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre cuando cambia el almacén de un recurso
		/// Recurso, valor viejo, valor nuevo
		/// </summary>
		event EventHandler<CambioElementoEventArgs<Recurso, float>> IAlmacén.AlCambiar
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

		#endregion

		#region Posición

		/// <summary>
		/// Devuelve la posición de este drop
		/// </summary>
		/// <value>The posición.</value>
		public Pseudoposición Posición { get; }

		Pseudoposición IPosicionable.Posición ()
		{
			return Posición;
		}

		#endregion

		#region Almacén

		IEnumerable<Recurso> IAlmacén.Recursos
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

		/// <summary>
		/// Devuelve el contenido en este drop
		/// </summary>
		public ListaPeso<Recurso> Almacén { get; }

		/// <summary>
		/// Revisa si contiene (y cuántas veces) los recursos codificados en un arreglo de float.
		/// </summary>
		/// <param name="otrosReqs">Otros recursos</param>
		/// <returns>The recursos.</returns>
		public float ContieneRecursos (IDictionary<Recurso, float> otrosReqs)
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Revisa si contiene (y cuántas veces) los recursos codificados en un arreglo de float.
		/// </summary>
		/// <param name="otrosReqs">Otros recursos</param>
		/// <returns>The recursos.</returns>
		public float ContieneRecursos (IAlmacén otrosReqs)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}