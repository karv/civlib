using System;
using System.Collections.Generic;
using Civ.RAW;
using ListasExtra;

namespace Civ.Almacén
{
	/// <summary>
	/// Una lista de recursos
	/// </summary>
	public interface IAlmacén
	{
		/// <summary>
		/// Devuelve la cantidad existente de un recurso dado.
		/// </summary>
		/// <param name="recurso">Recurso.</param>
		float this [Recurso recurso]{ get; set; }

		/// <summary>
		/// Devuelve la lista de recursos implicados
		/// </summary>
		/// <value>The recursos.</value>
		IEnumerable<Recurso> Recursos { get; }

		/// <summary>
		/// Revisa si contiene (y cuántas veces) los recursos codificados en un arreglo de float.
		/// </summary>
		/// <param name="otrosReqs">Otros recursos</param>
		float ContieneRecursos (IDictionary<Recurso, float> otrosReqs);

		/// <summary>
		/// Revisa si contiene (y cuántas veces) los recursos codificados en un arreglo de float.
		/// </summary>
		float ContieneRecursos (IAlmacén otrosReqs);

		/// <summary>
		/// Ocurre cuando cambia el almacén de un recurso
		/// </summary>
		event EventHandler<CambioElementoEventArgs<Recurso, float>> AlCambiar;
	}
}