using System;
using Global;
using CivLibrary.Debug;
using System.Collections.Generic;
using System.Threading;

namespace Civ.Barbaros
{
	/// <summary>
	/// Un generador de armadas bárbaras
	/// </summary>
	public class GeneradorArmadasBarbaras : ITickable
	{
		/// <summary>
		/// El tiempo esperado para generar bárbaro.
		/// En horas
		/// </summary>
		public double TiempoEsperadoGenerar = 1.0 / 6.0;

		public List<IReglaGeneracion> Reglas = new List<IReglaGeneracion>();

		double lambda
		{
			get
			{
				return 1 / TiempoEsperadoGenerar;
			}
		}

		public GeneradorArmadasBarbaras()
		{
		}

		/// <summary>
		/// Devuelve si debe generar bárbaro
		/// </summary>
		/// <returns><c>true</c>, if barbaro should be generared, <c>false</c> otherwise.</returns>
		/// <param name="t">Tiempo</param>
		public bool GenerarBarbaro(float t)
		{
			// Genera bárbaros bajo una distribución exponencial con lambda = 1/c
			// E = c = ProbBarbPotHora
			// Acumulada: f(x) = 1-e^(-lambda * x)
			// Densidad:  F(x) = lambda * e ^(-lambda * x)
			// Esperanza  E(X) = 1/lambda = c
			double Probabilidad = 1 - Math.Exp(-lambda * t);
			return g_.r.NextDouble() < Probabilidad;
		}

		/// <summary>
		/// Devuelve una armada bárbara
		/// </summary>
		/// <returns>The armada.</returns>
		public Armada getArmada()
		{
			System.Diagnostics.Debug.WriteLine("Generar bárbaro...");

			// Escoger una regla
			List<IReglaGeneracion> reglas = Reglas.FindAll(x => x.EsPosibleGenerar(g_.State));
			if (reglas.Count == 0)
			{
				System.Diagnostics.Debug.WriteLine("No hay regla para este caso");
				return null;
			}
			

			IReglaGeneracion usarRegla = reglas[g_.r.Next(reglas.Count)];
			Armada ret = usarRegla.GenerarArmada();
			System.Diagnostics.Debug.WriteLine(string.Format("Armada generada {0}", ret));
			return ret;
		}



		public void Tick(float t)
		{
			if (GenerarBarbaro(t))
				getArmada();
		}
	}
}