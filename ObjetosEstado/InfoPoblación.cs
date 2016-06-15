using System;

namespace Civ
{
	/// <summary>
	/// Mantiene información sobre la distribución por edades de una población
	/// </summary>
	[Serializable]
	public struct InfoPoblación
	{
		/// <summary>
		/// Población infantil
		/// </summary>
		public readonly ulong PreProductiva;
		/// <summary>
		/// Población trabajadora
		/// </summary>
		public readonly ulong Productiva;
		/// <summary>
		/// Población de la tercera edad
		/// </summary>
		public readonly ulong PostProductiva;

		/// <summary>
		/// Devuelve la población total
		/// </summary>
		/// <value>The total.</value>
		public ulong Total
		{
			get
			{
				return  PreProductiva + Productiva + PostProductiva;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.InfoPoblación"/> struct.
		/// </summary>
		/// <param name="pre">Población preproductiva</param>
		/// <param name="prod">Población productiva</param>
		/// <param name="post">Población postproductiva</param>
		public InfoPoblación (ulong pre, ulong prod, ulong post)
		{
			PreProductiva = pre;
			Productiva = prod;
			PostProductiva = post;
		}
	}
}