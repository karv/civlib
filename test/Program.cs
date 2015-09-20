using System;
using Civ;
using System.Reflection;
using System.IO;
using Global;
using Civ.Orden;
using System.Diagnostics;
using Civ.Barbaros;
using System.Threading;
using System.Collections.Generic;

namespace test
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			g_.CargaData();
			Global.g_.InicializarJuego();

			TestGeneradorArmadas();
		}

		/// <summary>
		/// Ejecuta el ciclo del juego
		/// </summary>
		/// <param name="MultiplicadorVelocidad">Multiplicador velocidad.</param>
		/// <param name="entreCiclos">Entre ciclos.</param>
		static void Ciclo(float MultiplicadorVelocidad, Action entreCiclos = null)
		{
			DateTime timer = DateTime.Now;
			while (true)
			{
				TimeSpan tiempo = DateTime.Now - timer;
				timer = DateTime.Now;
				float t = (float)tiempo.TotalHours * MultiplicadorVelocidad;

				// Console.WriteLine (t);
				Global.g_.Tick(t);

				entreCiclos?.Invoke();
				if (Global.g_.State.Civs.Count == 0)
					throw new Exception("Ya se acabó el juego :3");
			}

		}

		static void TestGeneradorArmadas()
		{
			UnidadRAW u = new UnidadRAW();
			u.Fuerza = 1;
			u.Nombre = "Gordo";
			ReglaGeneracionPuntuacion reg = new ReglaGeneracionPuntuacion();
			reg.ClaseArmada = new List<Tuple<UnidadRAW, ulong>>();
			reg.ClaseArmada.Add(new Tuple<UnidadRAW, ulong>(u, 100));
			reg.MaxPuntuacion = float.PositiveInfinity;
			reg.MinPuntuacion = 0;
			g_.BarbGen.Reglas.Add(reg);

			Ciclo(1000);

		}

		static void TestPeleaArmadas()
		{
			Civilizacion c1 = (Civilizacion)g_.State.Civs[0];
			Civilizacion c2 = (Civilizacion)g_.State.Civs[1];
			EstadoDiplomatico diplomaciaGuerra = new EstadoDiplomatico();
			diplomaciaGuerra.PermiteAtacar = true;
			c1.Diplomacia.Add(c2, diplomaciaGuerra);
			c2.Diplomacia.Add(c1, diplomaciaGuerra);
			g_.State.Civs.Add(c1);
			g_.State.Civs.Add(c2);

			Pseudoposicion p = new Pseudoposicion();
			p.A = g_.State.ObtenerListaTerrenos()[0];
			p.loc = 0;

			UnidadRAW u = new UnidadRAW();
			u.Fuerza = 1;
			u.Nombre = "Guerrero";
			u.Flags.Add("A pie");

			UnidadRAW uGordo = new UnidadRAW();
			uGordo.Fuerza = 150;
			uGordo.Nombre = "Hulk";
			uGordo.Flags.Add("A pie");
			uGordo.Mods.Add("A pie", 1f);
			uGordo.Dispersion = 0.1f;

			Armada ac1 = new Armada(c1, p);
			ac1.AgregaUnidad(u, 150);
			ac1[u].Entrenamiento = 1;
			//ac1.AgregaUnidad(uGordo, 1);
			Armada ac2 = new Armada(c2, p);
			ac2.AgregaUnidad(u, 150);


			// Listos para matarse
			while (ac1.Unidades.Count > 0 && ac2.Unidades.Count > 0)
			{
				Debug.WriteLine(string.Format("1]{0}\n2]{1}\n\n", ac1, ac2));
				g_.Tick(0.001f);
			}

			Debug.WriteLine("Ganador: " + (ac1.Unidades.Count > 0 ? "1" : "2"));

		}
	}
}
