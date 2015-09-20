using System;
using Global;
using CivLibrary.Debug;

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
			System.Diagnostics.Debug.WriteLine("Generar bárbaro");
			return null;
		}

		public void Tick(float t)
		{
			if (GenerarBarbaro(t))
				getArmada();
		}
	}
}