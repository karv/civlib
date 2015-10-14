using System;
using System.Collections.Generic;
using Civ.Data;
using ListasExtra;

namespace Civ
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

	public static class ExtIAlmacén
	{
		/// <summary>
		/// Crea una copia de el almacén a un diccionario.
		/// </summary>
		/// <returns>Un diccionario con las mismas entradas que este IAlmacén.</returns>
		public static Dictionary<Recurso, float> ToDictionary (this IAlmacénRead alm)
		{
			var ret = new Dictionary<Recurso, float> ();
			foreach (var x in alm.Recursos)
			{
				ret [x] = alm [x];
			}
			return ret;
		}
	}
}