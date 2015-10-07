using System;
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
		float recurso (Recurso recurso);
	}

	/// <summary>
	/// Almacén de lectura y escritura
	/// </summary>
	public interface IAlmacén : IAlmacénRead
	{
		/// <summary>
		/// Establece la cantidad de recursos
		/// </summary>
		/// <param name="rec">Rec.</param>
		/// <param name="val">Value.</param>
		[ObsoleteAttribute]
		void SetRecurso (Recurso rec, float val);

		/// <summary>
		/// Cambia la cantidad de recursos por una cantidad dada.
		/// </summary>
		/// <param name="rec">Rec.</param>
		/// <param name="delta">Delta.</param>
		void ChangeRecurso (Recurso rec, float delta);
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
				ret.Add (x, alm.recurso (x));
			}
			return ret;
		}
	}
}