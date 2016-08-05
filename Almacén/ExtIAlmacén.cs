using System.Collections.Generic;
using Civ.RAW;

namespace Civ.Almacén
{
	/// <summary>
	/// Extensiones de Almacenes
	/// </summary>
	public static class ExtIAlmacén
	{
		/// <summary>
		/// Crea una copia de el almacén a un diccionario.
		/// </summary>
		/// <returns>Un diccionario con las mismas entradas que este IAlmacén.</returns>
		public static Dictionary<Recurso, float> ToDictionary (this IAlmacén alm)
		{
			var ret = new Dictionary<Recurso, float> ();
			foreach (var x in alm.Recursos)
				ret [x] = alm [x];
			return ret;
		}
	}
}