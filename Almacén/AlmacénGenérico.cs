using System;
using ListasExtra;
using Civ.RAW;

namespace Civ.Almacén
{
	/// <summary>
	/// Un almacén genérico.
	/// <para>
	/// Todos los almacenes deben heredar a éste.
	/// </para>
	/// </summary>
	[Serializable]
	public class AlmacénGenérico : ListaPeso<Recurso>, IAlmacén
	{
		#region ctor

		/// <summary>
		/// </summary>
		public AlmacénGenérico ()
		{
		}

		#endregion

		#region General

		/// <summary>
		/// Devuelve o establece un <c>float</c> la cantidad de un recurso en este almacén
		/// </summary>
		/// <param name="rec">Tipo de recurso</param>
		public new virtual float this [Recurso rec]
		{
			get
			{
				return base [rec];
			}
			set
			{
				base [rec] = value;
			}
		}

		/// <summary>
		/// Devuelve la lista de recursos implicados
		/// </summary>
		/// <value>The recursos.</value>
		public System.Collections.Generic.IEnumerable<Recurso> Recursos
		{
			get
			{
				return Keys;
			}
		}

		/// <summary>
		/// Clona esta instancia
		/// </summary>
		public AlmacénGenérico Clonar ()
		{
			var ret = new AlmacénGenérico ();
			foreach (var x in this)
				ret.Add (x);
			return ret;
		}

		#endregion

		#region Eventos

		event EventHandler<CambioElementoEventArgs<Recurso, float>> IAlmacén.AlCambiar
		{
			add
			{
				AlCambiarValor += value;
			}
			remove
			{
				AlCambiarValor -= value;
			}
		}

		#endregion

		/// <summary>
		/// Revisa si contiene (y cuántas veces) los recursos codificados en un arreglo de float.
		/// </summary>
		/// <param name="otrosReqs">Otros recursos</param>
		/// <returns>The recursos.</returns>
		public float ContieneRecursos (System.Collections.Generic.IDictionary<Recurso, float> otrosReqs)
		{
			return (float)VecesConteniendoA (otrosReqs);
		}

		/// <summary>
		/// Revisa si contiene (y cuántas veces) los recursos codificados en un arreglo de float.
		/// </summary>
		/// <param name="otrosReqs">Otros recursos</param>
		/// <returns>The recursos.</returns>
		public float ContieneRecursos (IAlmacén otrosReqs)
		{
			return ContieneRecursos (otrosReqs.ToDictionary ());
		}

	}
}