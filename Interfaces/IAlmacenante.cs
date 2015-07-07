using System;

namespace Civ
{
	/// <summary>
	/// Provee métodos para accesar a su almacén
	/// </summary>
	public interface IAlmacenante
	{
		IAlmacén Almacen { get; }
	}

	public static class ExiIAlmacen
	{
		public static Single obtenerRecurso(this IAlmacenante a, Recurso R)
		{
			return a.Almacen.recurso(R);
		}
	}
}

