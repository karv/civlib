using System;

namespace Civ
{
	/// <summary>
	/// Provee métodos para accesar a su almacén
	/// </summary>
	public interface IAlmacenante
	{
		IAlmacén Almacen { get; }

		float CalculaDeltaRecurso(Recurso recurso);
	}

	public static class ExiIAlmacen
	{
		public static Single ObtenerRecurso(this IAlmacenante almacén, Recurso recurso)
		{
			return almacén.Almacen.recurso(recurso);
		}
	}
}