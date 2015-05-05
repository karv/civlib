using System;
using System.Collections.Generic;

namespace Civ
{
	/// <summary>
	/// Promete habilidades para leer recursos de un almacén
	/// </summary>
	public interface IAlmacénRead
	{
		IEnumerable<Recurso> recursos { get; }

		float recurso(Recurso R);
	}

	public interface IAlmacén : IAlmacénRead
	{
		void setRecurso(Recurso rec, float val);
	}

	public static class ExtIAlmacén
	{
		/// <summary>
		/// Crea una copia de el almacén a un diccionario.
		/// </summary>
		/// <returns>Un diccionario con las mismas entradas que este IAlmacén.</returns>
		public static Dictionary<Recurso, float> ToDictionary(this IAlmacénRead alm)
		{
			Dictionary<Recurso, float> ret = new Dictionary<Recurso, float>();
			foreach (var x in alm.recursos)
			{
				ret.Add(x, alm.recurso(x));
			}
			return ret;
		}
	}
}