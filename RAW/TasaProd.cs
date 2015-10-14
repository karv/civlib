using System;
using Civ.Data;
using Civ.Data.Import;

namespace Civ.Data.TasaProd
{
	/// <summary>
	/// Forma en que los recursos natirales crecen
	/// </summary>
	public abstract class TasaProd : IImportable
	{
		public Recurso Recurso;

		void IImportable.Importar (System.IO.StreamReader reader)
		{
			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine ();
				line.ToLower ();
				var spl = line.Split (':');
				for (int i = 0; i < spl.Length; i++)
				{
					spl [i] = spl [i].Trim ();
				}
				ImportarLinea (spl);
			}
		}

		string RecursoId;

		protected virtual void CrearVínculos ()
		{
			Recurso = ImportMachine.Valor (RecursoId) as Recurso;
		}

		protected virtual void Limpiar ()
		{
			RecursoId = null;
		}

		void IImportable.Vincular ()
		{
			CrearVínculos ();
			Limpiar ();
		}

		/// <summary>
		/// Los pasos de imporatación cuando lee una línea
		/// </summary>
		/// <param name="spl">Spl.</param>
		protected virtual void ImportarLinea (string [] spl)
		{
			if (spl [0] == "recurso")
			{
				RecursoId = spl [1];
			}
		}

		#region ITickable implementation

		public abstract void Tick (IAlmacén alm, TimeSpan t);

		public abstract float DeltaEsperado (IAlmacénRead alm);

		#endregion
	}

	/// <summary>
	/// Tasa prod constante.
	/// Comportamiento lineal
	/// </summary>
	public class TasaProdConstante: TasaProd
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


		public override float DeltaEsperado (IAlmacénRead alm)
		{
			return Crecimiento;
		}

		protected override void ImportarLinea (string [] spl)
		{
			base.ImportarLinea (spl);
			switch (spl [0])
			{
				case "max":
					Max = float.Parse (spl [1]);
					break;
				case "crecimiento":
					Crecimiento = float.Parse (spl [1]);
					break;
			}
		}

		#endregion
		
	}

	/// <summary>
	/// Tasa prod exp.
	/// Comportamiento exponencial
	/// </summary>
	public class TasaProdExp:TasaProd
	{
		public float Max;
		public float CrecimientoBase;


		#region implemented abstract members of TasaProd

		public override void Tick (IAlmacén alm, TimeSpan t)
		{
			alm [Recurso] *= CrecimientoBase * (float)t.TotalHours; // TODO, debe tener comportamiento exponencial
			alm [Recurso] = Math.Min (Max, alm [Recurso]);
			
		}

		public override float DeltaEsperado (IAlmacénRead alm)
		{
			return alm [Recurso] * CrecimientoBase;
		}

		protected override void ImportarLinea (string [] spl)
		{
			base.ImportarLinea (spl);
			switch (spl [0])
			{
				case "max":
					Max = float.Parse (spl [1]);
					break;
				case "crecimiento":
					CrecimientoBase = float.Parse (spl [1]);
					break;
			}
		}

		#endregion
	}
}