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

		event EventHandler<CambioElementoEventArgs<Recurso, float>> IAlmacénRead.AlCambiar
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
		public float ContieneRecursos (IAlmacénRead otrosReqs)
		{
			return ContieneRecursos (otrosReqs.ToDictionary ());
		}

	}
}