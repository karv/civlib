using System;
using System.Collections.Generic;
using Civ.RAW;
using ListasExtra;

namespace Civ.Almacén
{
	/// <summary>
	/// Promete habilidades para leer recursos de un almacén
	/// </summary>
	public interface IAlmacénRead
	{
		/// <summary>
		/// Devuelve la lista de recursos implicados
		/// </summary>
		/// <value>The recursos.</value>
		IEnumerable<Recurso> Recursos { get; }

		/// <summary>
		/// Devuelve un array de float que representa las entradas de recursos.
		/// </summary>
		/// <returns>The array.</returns>
		IList<float> AsArray ();

		/// <summary>
		/// Almacén de lectura y escritura
		/// </summary>
		/// <param name="recurso">Recurso.</param>
		float this [Recurso recurso]{ get; }

		/// <summary>
		/// Revisa si contiene (y cuántas veces) los recursos codificados en un arreglo de float.
		/// </summary>
		/// <param name="otrosReqs">Otros recursos</param>
		float ContieneRecursos (IList<float> otrosReqs);

		/// <summary>
		/// Revisa si contiene (y cuántas veces) los recursos codificados en un arreglo de float.
		/// </summary>
		float ContieneRecursos (IAlmacénRead otrosReqs);

		/// <summary>
		/// Ocurre cuando cambia el almacén de un recurso
		/// </summary>
		event EventHandler<CambioElementoEventArgs<Recurso, float>> AlCambiar;
	}
	
}