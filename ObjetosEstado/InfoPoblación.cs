using System;
using Civ.Global;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Mantiene información sobre la distribución por edades de una población
	/// </summary>
	[Serializable]
	public struct InfoPoblación : IPuntuado
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

		#region Información

		/// <summary>
		/// Población infantil
		/// </summary>
		public long PreProductiva
		{ get { return (long)PrimeraEdad; } }

		/// <summary>
		/// Población trabajadora
		/// </summary>
		public long Productiva
		{ get { return (long)SegundaEdad; } }

		/// <summary>
		/// Población de la tercera edad
		/// </summary>
		public long PostProductiva
		{ get { return (long)TerceraEdad; } }

		/// <summary>
		/// Devuelve la población total
		/// </summary>
		/// <value>The total.</value>
		public long Total
		{ get { return  PreProductiva + Productiva + PostProductiva; } }

		/// <summary>
		/// Devuelve el total poblacional real.
		/// </summary>
		/// <value>The real total.</value>
		public float RealTotal
		{ get { return PrimeraEdad + SegundaEdad + TerceraEdad; } }

		#endregion

		#region Operacional

		public InfoPoblación AgregaPoblación (long agrega)
		{
			if (agrega + SegundaEdad < 0)
				throw new Exception ();
			return new InfoPoblación (PrimeraEdad, agrega + SegundaEdad, TerceraEdad);
		}

		public InfoPoblación AgregaPoblación (float prim, float seg, float terc)
		{
			return new InfoPoblación (
				prim + PrimeraEdad,
				seg + SegundaEdad,
				terc + TerceraEdad);
		}

		public InfoPoblación AgregaPoblación (float [] delta)
		{ 
			return AgregaPoblación (delta [0], delta [1], delta [2]);
		}


		#endregion

		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="InfoPoblación"/> struct.
		/// </summary>
		/// <param name="pre">Población preproductiva</param>
		/// <param name="prod">Población productiva</param>
		/// <param name="post">Población postproductiva</param>
		public InfoPoblación (float pre, float prod, float post)
		{
			if (pre < 0 || prod < 0 || post < 0)
				throw new ArgumentException ("Ninguna población puede ser negativa.");
			PrimeraEdad = pre;
			SegundaEdad = prod;
			TerceraEdad = post;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InfoPoblación"/> struct.
		/// Sólo agrega población productiva.
		/// </summary>
		/// <param name="prod">Población productiva real</param>
		public InfoPoblación (float prod)
			: this (0, prod, 0)
		{
		}

		#endregion

		public float Puntuación
		{
			get
			{
				return PrimeraEdad * 2 + SegundaEdad * 3 + TerceraEdad;
			}
		}
	}
}