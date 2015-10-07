using System;
using Global;
using System.Collections.Generic;

namespace Civ.Barbaros
{
	/// <summary>
	/// Un generador de armadas bárbaras
	/// Usando distribución exponencial
	/// </summary>
	public class GeneradorArmadasBarbaras : ITickable
	{
		/// <summary>
		/// El tiempo esperado para generar bárbaro.
		/// En horas
		/// </summary>
		public double TiempoEsperadoGenerar = 1.0 / 6.0;

		/// <summary>
		/// Las reglas de generación
		/// </summary>
		public List<IReglaGeneracion> Reglas { get; }

		double lambda
		{
			get
			{
				return 1 / TiempoEsperadoGenerar;
			}
		}

		public GeneradorArmadasBarbaras ()
		{
			Reglas = new List<IReglaGeneracion> ();
		}

		/// <summary>
		/// Devuelve si debe generar bárbaro
		/// </summary>
		/// <returns><c>true</c>, if barbaro should be generared, <c>false</c> otherwise.</returns>
		/// <param name="t">Tiempo</param>
		public bool GenerarBarbaro (TimeSpan t)
		{
			// Genera bárbaros bajo una distribución exponencial con lambda = 1/c
			// E = c = ProbBarbPotHora
			// Acumulada: f(x) = 1-e^(-lambda * x)
			// Densidad:  F(x) = lambda * e ^(-lambda * x)
			// Esperanza  E(X) = 1/lambda = c
			double Probabilidad = 1 - Math.Exp (-lambda * (float)t.TotalHours);
			return Juego.Rnd.NextDouble () < Probabilidad;
		}

		/// <summary>
		/// Devuelve una armada bárbara
		/// </summary>
		/// <returns>The armada.</returns>
		public Armada Armada ()
		{
			// Escoger una regla
			List<IReglaGeneracion> reglas = Reglas.FindAll (x => x.EsPosibleGenerar (Juego.State));
			if (reglas.Count == 0)
			{
				System.Diagnostics.Debug.WriteLine ("No hay regla para este caso");
				return null;
			}
			

			IReglaGeneracion usarRegla = reglas [Juego.Rnd.Next (reglas.Count)];
			Armada ret = usarRegla.GenerarArmada ();
			return ret;
		}

		#region ITickable

		/// <summary>
		/// Ejecuta un tick
		/// </summary>
		/// <param name="t">Lapso del tick</param>
		public void Tick (TimeSpan t)
		{
			if (GenerarBarbaro (t))
				Armada ();
		}

		#endregion
	}
}