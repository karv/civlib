﻿using System;
using Civ;
using Global;
using System.Diagnostics;
using Civ.Barbaros;
using System.Collections.Generic;
using Civ.Orden;
using IU;

namespace Test
{
	class MainClass
	{
		static Civilizacion MyCiv;
		static Ciudad MyCiudad;

		public static void Main()
		{
			Juego.CargaData();
			Juego.InicializarJuego();


			MyCiv = Juego.State.Civs[0] as Civilizacion;
			MyCiudad = MyCiv.Ciudades[0] as Ciudad;

			MyCiv.OnNuevoMensaje += delegate
			{
				while (MyCiv.ExisteMensaje)
				{
					Mensaje m = MyCiv.SiguitenteMensaje();
					Debug.WriteLine(m.ToString());
				}
			};

			TestBigIrA();
		}

		static void TestRecoger()
		{
			UnidadRAW u = Juego.Data.Unidades[0];
			u.MaxCarga = 100; // Porque yo lo digo
			var pos = new Pseudoposicion();
			Terreno terrA = MyCiudad.Posicion().A;
			Terreno terrB = Juego.State.ObtenerListaTerrenos()[0];
			Juego.State.Topología[terrA, terrB] = 1;
			pos.A = terrA;
			pos.B = terrB;
			pos.Loc = 0.5f;
			var drop = new DropStack(pos);
			Juego.State.Drops.Add(drop);

			var arm = new Armada(MyCiudad);
			MyCiv.Armadas.Add(arm);

			arm.AgregaUnidad(u, 10);
			Debug.WriteLine("Peso máximo: " + arm[u].Carga.MaxCarga);

			var ord = new OrdenRecoger(arm, drop);
			arm.Orden = ord;

			ord.AlLlegar += delegate
			{
				Debug.WriteLine("Llegamos a " + ord.StackTarget.Posicion());
			};
			ord.AlRegresar += delegate
			{
				Debug.WriteLine("Regresamos a " + ord.Origen);
			};

			Ciclo(1000);
		}

		static void TestBigIrA()
		{
			var u = new UnidadRAW();
			u.Nombre = "Velociraptor";
			u.Velocidad = 10;
			u.Fuerza = 1;
			Terreno destino = Juego.State.ObtenerListaTerrenos()[0];

			var arm = new Armada(MyCiudad);

			arm.AgregaUnidad(u, 5);
			var ord = new OrdenIrALugar(arm, destino);
			ord.AlAcabarUnaOrden += delegate
			{
				Debug.WriteLine("Armada está en " + arm.Posicion);
			};
			ord.AlTerminar += delegate
			{
				Debug.WriteLine("Armada llegó a " + arm.Posicion);
				Debug.WriteLine("Ahora está at ease; orden: " + arm.Orden);
			};
			arm.Orden = ord;

			Ciclo(500);
		}

		static void TestReclutar()
		{
			UnidadRAW u = Juego.Data.Unidades[0];
			MyCiudad.Reclutar(u, 3);
		}

		static void TestArmadaDesaparecen()
		{
			var arm = new Armada(MyCiudad);

			Debug.WriteLine(arm.CivDueño.Armadas);

			Juego.Tick(TimeSpan.FromHours(1));
			Debug.WriteLine(arm.CivDueño.Armadas);
		}

		/// <summary>
		/// Ejecuta el ciclo del juego
		/// </summary>
		/// <param name="multiplicadorVelocidad">Multiplicador velocidad.</param>
		/// <param name="entreCiclos">Entre ciclos.</param>
		static void Ciclo(float multiplicadorVelocidad, Action entreCiclos = null)
		{
			DateTime timer = DateTime.Now;
			while (true)
			{				
				TimeSpan Tiempo = DateTime.Now - timer;
				timer = DateTime.Now;
				Tiempo = new TimeSpan((long)(Tiempo.Ticks * multiplicadorVelocidad));

				// Console.WriteLine (t);
				Juego.Tick(Tiempo);

				entreCiclos?.Invoke();
				if (Juego.State.Civs.Count == 0)
					throw new Exception("Ya se acabó el juego :3");
			}

		}

		static void TestCiudad()
		{
			ICiudad cd = Juego.State.CiudadesExistentes()[0];
			Action Entreturnos = delegate
			{
				if (Juego.Rnd.NextDouble() < 0.001f)
				{
					foreach (var x in cd.Almacen.recursos)
					{
						Debug.WriteLine(
							string.Format("{0}: {1}({2})", x, cd.Almacen.recurso(x), cd.CalculaDeltaRecurso(x))
						);
					}

				}
			};
			
			Ciclo(1000, Entreturnos);
		}

		static void TestGeneradorArmadas()
		{
			var u = new UnidadRAW();
			u.Fuerza = 1;
			u.Nombre = "Gordo";
			var reg = new ReglaGeneracionPuntuacion();
			reg.ClaseArmada = new List<Tuple<UnidadRAW, ulong>>();
			reg.ClaseArmada.Add(new Tuple<UnidadRAW, ulong>(u, 100));
			reg.MaxPuntuacion = float.PositiveInfinity;
			reg.MinPuntuacion = 0;
			//g_.BarbGen.Reglas.Add(reg);

			Ciclo(1000);

		}

		static void TestPeleaArmadas()
		{
			var c1 = (Civilizacion)Juego.State.Civs[0];
			var c2 = (Civilizacion)Juego.State.Civs[1];
			var diplomaciaGuerra = new EstadoDiplomatico();
			diplomaciaGuerra.PermiteAtacar = true;
			c1.Diplomacia.Add(c2, diplomaciaGuerra);
			c2.Diplomacia.Add(c1, diplomaciaGuerra);
			Juego.State.Civs.Add(c1);
			Juego.State.Civs.Add(c2);

			var p = new Pseudoposicion();
			p.A = Juego.State.ObtenerListaTerrenos()[0];
			p.Loc = 0;

			var u = new UnidadRAW();
			u.Fuerza = 1;
			u.Nombre = "Guerrero";
			u.Flags.Add("A pie");

			var uGordo = new UnidadRAW();
			uGordo.Fuerza = 150;
			uGordo.Nombre = "Hulk";
			uGordo.Flags.Add("A pie");
			uGordo.Mods.Add("A pie", 1f);
			uGordo.Dispersion = 0.1f;

			var ac1 = new Armada(c1, p);
			ac1.AgregaUnidad(u, 150);
			ac1[u].Entrenamiento = 1;
			//ac1.AgregaUnidad(uGordo, 1);
			var ac2 = new Armada(c2, p);
			ac2.AgregaUnidad(u, 150);


			// Listos para matarse
			Ciclo(50);

			Debug.WriteLine("Ganador: " + (ac1.Unidades.Count > 0 ? "1" : "2"));

		}
	}
}
