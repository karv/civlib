using System;
using Civ;
using Global;
using System.Diagnostics;
using Civ.Barbaros;
using System.Collections.Generic;

namespace Test
{
	class MainClass
	{
		static ICivilizacion MyCiv;
		static ICiudad MyCiudad;

		public static void Main ()
		{
			Juego.CargaData ();
			Juego.InicializarJuego ();


			MyCiv = Juego.State.Civs [0];
			MyCiudad = MyCiv.Ciudades [0];

			TestReclutar ();
		}

		static void TestReclutar ()
		{
			UnidadRAW u = Juego.Data.Unidades [0];
			MyCiudad.Reclutar (u, 3);
		}

		static void TestArmadaDesaparecen ()
		{
			var arm = new Armada (MyCiudad);

			Debug.WriteLine (arm.CivDueño.Armadas);

			Juego.Tick ();
			Debug.WriteLine (arm.CivDueño.Armadas);
		}

		/// <summary>
		/// Ejecuta el ciclo del juego
		/// </summary>
		/// <param name="multiplicadorVelocidad">Multiplicador velocidad.</param>
		/// <param name="entreCiclos">Entre ciclos.</param>
		static void Ciclo (float multiplicadorVelocidad, Action entreCiclos = null)
		{
			DateTime timer = DateTime.Now;
			while (true) {
				TimeSpan tiempo = DateTime.Now - timer;
				timer = DateTime.Now;
				float t = (float)tiempo.TotalHours * multiplicadorVelocidad;

				// Console.WriteLine (t);
				Juego.Tick (t);

				entreCiclos?.Invoke ();
				if (Juego.State.Civs.Count == 0)
					throw new Exception ("Ya se acabó el juego :3");
			}

		}

		static void TestCiudad ()
		{
			ICiudad cd = Juego.State.CiudadesExistentes () [0];
			Action Entreturnos = delegate {
				if (Juego.Rnd.NextDouble () < 0.001f) {
					foreach (var x in cd.Almacen.recursos) {
						Debug.WriteLine (
							string.Format ("{0}: {1}({2})", x, cd.Almacen.recurso (x), cd.CalculaDeltaRecurso (x))
						);
					}

				}
			};
			
			Ciclo (100, Entreturnos);
		}

		static void TestGeneradorArmadas ()
		{
			var u = new UnidadRAW ();
			u.Fuerza = 1;
			u.Nombre = "Gordo";
			var reg = new ReglaGeneracionPuntuacion ();
			reg.ClaseArmada = new List<Tuple<UnidadRAW, ulong>> ();
			reg.ClaseArmada.Add (new Tuple<UnidadRAW, ulong> (u, 100));
			reg.MaxPuntuacion = float.PositiveInfinity;
			reg.MinPuntuacion = 0;
			//g_.BarbGen.Reglas.Add(reg);

			Ciclo (1000);

		}

		static void TestPeleaArmadas ()
		{
			var c1 = (Civilizacion)Juego.State.Civs [0];
			var c2 = (Civilizacion)Juego.State.Civs [1];
			var diplomaciaGuerra = new EstadoDiplomatico ();
			diplomaciaGuerra.PermiteAtacar = true;
			c1.Diplomacia.Add (c2, diplomaciaGuerra);
			c2.Diplomacia.Add (c1, diplomaciaGuerra);
			Juego.State.Civs.Add (c1);
			Juego.State.Civs.Add (c2);

			var p = new Pseudoposicion ();
			p.A = Juego.State.ObtenerListaTerrenos () [0];
			p.loc = 0;

			var u = new UnidadRAW ();
			u.Fuerza = 1;
			u.Nombre = "Guerrero";
			u.Flags.Add ("A pie");

			var uGordo = new UnidadRAW ();
			uGordo.Fuerza = 150;
			uGordo.Nombre = "Hulk";
			uGordo.Flags.Add ("A pie");
			uGordo.Mods.Add ("A pie", 1f);
			uGordo.Dispersion = 0.1f;

			var ac1 = new Armada (c1, p);
			ac1.AgregaUnidad (u, 150);
			ac1 [u].Entrenamiento = 1;
			//ac1.AgregaUnidad(uGordo, 1);
			var ac2 = new Armada (c2, p);
			ac2.AgregaUnidad (u, 150);


			// Listos para matarse
			while (ac1.Unidades.Count > 0 && ac2.Unidades.Count > 0) {
				Debug.WriteLine (string.Format ("1]{0}\n2]{1}\n\n", ac1, ac2));
				Juego.Tick (0.001f);
			}

			Debug.WriteLine ("Ganador: " + (ac1.Unidades.Count > 0 ? "1" : "2"));

		}
	}
}
