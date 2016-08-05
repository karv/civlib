using System;
using System.Collections.Generic;
using Civ.RAW;
using ListasExtra;

namespace Civ.Almacén
{
	/// <summary>
	/// Almacén de lectura y escritura
	/// </summary>
	public interface IAlmacén : IAlmacénRead
	{
		/// <summary>
		/// Devuelve la cantidad existente de un recurso dado.
		/// </summary>
		/// <param name="recurso">Recurso.</param>
		new float this [Recurso recurso]{ get; set; }
	}
}