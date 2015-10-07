using System;

namespace Civ
{
	/// <summary>
	/// Provee métodos para accesar a su almacén
	/// </summary>
	public interface IAlmacenante
	{
		/// <summary>
		/// Su almacén
		/// </summary>
		IAlmacén Almacen { get; }

		/// <summary>
		/// Calcula la tasa de cambiod eun recurso, por hora.
		/// </summary>
		float CalculaDeltaRecurso (Recurso recurso);
	}

	public static class ExiIAlmacen
	{
		/// <summary>
		/// Obtiene los recursos que existen en el almacén correspondiente a un IAlmacenante.
		/// </summary>
		/// <returns>Los recursos</returns>
		/// <param name="almacén">Almacén.</param>
		/// <param name="recurso">Recurso.</param>
		public static float ObtenerRecurso (this IAlmacenante almacén,
		                                    Recurso recurso)
		{
			return almacén.Almacen [recurso];
		}
	}
}