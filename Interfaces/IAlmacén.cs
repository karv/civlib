﻿using System;
using System.Collections.Generic;

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
		IEnumerable<Recurso> recursos { get; }

		/// <summary>
		/// Devuelve la cantidad existente de un recurso dado.
		/// </summary>
		/// <param name="recurso">Recurso.</param>
		[Obsolete]
		float recurso (Recurso recurso);

		/// <summary>
		/// Devuelve la cantidad existente de un recurso dado.
	

		/// <summary>
		/// Almacén de lectura y escritura
		/// </summary>
		/// <param name="recurso">Recurso.</param>
		float this [Recurso recurso]{ get; }
	}

	public interface IAlmacén : IAlmacénRead
	{
		/// <summary>
		/// Establece la cantidad de recursos
		/// </summary>
		/// <param name="rec">Rec.</param>
		/// <param name="val">Value.</param>
		[Obsolete]
		void SetRecurso (Recurso rec, float val);

		/// <summary>
		/// Cambia la cantidad de recursos por una cantidad dada.
		/// </summary>
		/// <param name="rec">Rec.</param>
		/// <param name="delta">Delta.</param>
		[Obsolete]
		void ChangeRecurso (Recurso rec, float delta);

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
			foreach (var x in alm.recursos)
			{
				ret [x] = alm [x];
			}
			return ret;
		}
	}
}