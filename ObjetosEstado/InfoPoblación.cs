using System;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Mantiene información sobre la distribución por edades de una población
	/// </summary>
	[Serializable]
	public struct InfoPoblación
	{
		#region Interno

		/// <summary>
		/// Población <c>float</c> de infantes.
		/// </summary>
		public readonly float PrimeraEdad;
		/// <summary>
		/// Población <c>float</c> de fuerza de trabajo.
		/// </summary>
		public readonly float SegundaEdad;
		/// <summary>
		/// Población <c>float</c> de viejos.
		/// </summary>
		public readonly float TerceraEdad;

		#endregion

		/// <summary>
		/// Población infantil
		/// </summary>
		public long PreProductiva
		{
			get
			{
				return (long)PrimeraEdad;
			}
		}

		/// <summary>
		/// Población trabajadora
		/// </summary>
		public long Productiva
		{
			get
			{
				return (long)SegundaEdad;
			}
		}

		/// <summary>
		/// Población de la tercera edad
		/// </summary>
		public long PostProductiva
		{
			get
			{
				return (long)TerceraEdad;
			}
		}

		/// <summary>
		/// Devuelve la población total
		/// </summary>
		/// <value>The total.</value>
		public long Total
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
		public InfoPoblación (float pre, float prod, float post)
		{
			PrimeraEdad = pre;
			SegundaEdad = prod;
			TerceraEdad = post;
		}
	}
}