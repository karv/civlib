using System;
using Civ.Almacén;

namespace Civ.RAW
{
	/// <summary>
	/// Forma en que los recursos natirales crecen
	/// </summary>
	[Serializable]
	public abstract class TasaProd
	{
		public Recurso Recurso;

		#region ITickable implementation

		public abstract void Tick (IAlmacén alm, TimeSpan t);

		#endregion
	}

	/// <summary>
	/// Tasa prod constante.
	/// Comportamiento lineal
	/// </summary>
	[Serializable]
	public class TasaProdConstante : TasaProd
	{
		/// <summary>
		/// Máximo del recurso que puede ofrecer esta tasa de crecimiento
		/// </summary>
		public float Max;
		/// <summary>
		/// Aumento de recurso por hora
		/// </summary>
		public float Crecimiento;

		#region TasaProd

		public override void Tick (IAlmacén alm, TimeSpan t)
		{
			
			if (alm [Recurso] < Max)
				alm [Recurso] += Crecimiento * (float)t.TotalHours;
		}


		#endregion
	}

	/// <summary>
	/// Tasa prod exp.
	/// Comportamiento exponencial
	/// </summary>
	[Serializable]
	public class TasaProdExp : TasaProd
	{
		public float Max;
		public float CrecimientoBase;

		#region implemented abstract members of TasaProd

		public override void Tick (IAlmacén alm, TimeSpan t)
		{
			alm [Recurso] *= (float)Math.Pow (CrecimientoBase, t.TotalHours);
			alm [Recurso] = Math.Min (Max, alm [Recurso]);
		}

		#endregion
	}
}