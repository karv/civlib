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
		/// Almacén de lectura y escritura
		/// </summary>
		/// <param name="recurso">Recurso.</param>
		float this [Recurso recurso]{ get; }

		/// <summary>
		/// Ocurre cuando cambia el almacén de un recurso
		/// </summary>
		event EventHandler<CambioElementoEventArgs<Recurso, float>> AlCambiar;
	}
	
}