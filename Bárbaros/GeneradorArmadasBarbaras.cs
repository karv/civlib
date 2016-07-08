using System;
using Civ.Global;
using System.Collections.Generic;
using System.Linq;
using Civ.ObjetosEstado;
using Civ.Orden;
using ListasExtra.Extensiones;
using System.Diagnostics;

namespace Civ.Bárbaros
{
	/// <summary>
	/// Un generador de armadas bárbaras
	/// Usando distribución exponencial
	/// </summary>
	[Serializable]
	public class GeneradorArmadasBarbaras : ITickable
	{
		#region Estadístico

		/// <summary>
		/// El tiempo esperado para generar bárbaro.
		/// En horas
		/// </summary>
		public double TiempoEsperadoGenerar = 1.0 / 6.0;

		/// 

		double lambda
		{
			get
			{
				return 1 / TiempoEsperadoGenerar;
			}
		}

		#endregion

		#region General

		/// <summary>
		/// Las reglas de generación
		/// </summary>
		public HashSet<IReglaGeneración> Reglas { get; }

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
			return HerrGlobal.Rnd.NextDouble () < Probabilidad;
		}

		/// <summary>
		/// Devuelve una armada bárbara
		/// </summary>
		/// <returns>The armada.</returns>
		public Armada Armada ()
		{
			// Escoger una regla
			var reglas = new List<IReglaGeneración> (Reglas.Where (x => x.EsPosibleGenerar (Juego.State)));
			if (reglas.Count == 0)
			{
				System.Diagnostics.Debug.WriteLine ("No hay regla para este caso");
				return null;
			}


			IReglaGeneración usarRegla = reglas [HerrGlobal.Rnd.Next (reglas.Count)];
			Armada ret = usarRegla.GenerarArmada ();

			#if DEBUG
			if (ret == null)
			{
				Console.WriteLine ("Se intentó agregar una unidad bárbara, pero la media militar es muy baja.");
			}
			else
			{
				Console.WriteLine ("Ha aparecido una armada bárbara en " + ret.Posición);
				Console.WriteLine ("Unidades");
				foreach (var x in ret.Unidades)
					Console.WriteLine (x);
				Console.WriteLine (string.Format (
					"Peso: {0}; Velocidad: {1}",
					ret.Peso,
					ret.Velocidad));

				// Órdenes
				DarOrden (ret);
				#if DEBUG
				var ord = ret.Orden as OrdenIrALugar;
				Console.WriteLine ("Tiempo estimado: " + ord.TiempoEstimado);

				ord.AlLlegar += AlLlegar;
				#endif
			}
			#endif

			return ret;
		}

		[Conditional ("DEBUG")]
		static void AlLlegar (object sender, EventArgs args)
		{
			Debug.WriteLine ("Llegué");
			Debug.WriteLine (args);
		}

		static void DarOrden (Armada arm)
		{
			var destino = Juego.State.CiudadesExistentes ().Aleatorio ();
			arm.Orden = new OrdenIrALugar (arm, destino.Posición ());
		}


		#endregion

		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Bárbaros.GeneradorArmadasBarbaras"/> class.
		/// </summary>
		public GeneradorArmadasBarbaras ()
		{
			Reglas = new HashSet<IReglaGeneración> ();
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre antes del tick
		/// </summary>
		public event EventHandler AlTickAntes;

		/// <summary>
		/// Ocurre después del tick
		/// </summary>
		public event EventHandler AlTickDespués;

		/// 

		#endregion

		#region ITickable

		/// <summary>
		/// Ejecuta un tick
		/// </summary>
		/// <param name="t">Lapso del tick</param>
		public void Tick (TimeEventArgs t)
		{
			AlTickAntes?.Invoke (this, t);
			if (GenerarBarbaro (t.GameTime))
				Armada ();
			AlTickDespués?.Invoke (this, t);
		}

		#endregion
	}
}