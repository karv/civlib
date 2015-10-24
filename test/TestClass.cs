using System;
using Civ;
using Global;
using System.Diagnostics;
using Civ.Bárbaros;
using System.Collections.Generic;
using Civ.Orden;
using IU;
using NUnit.Framework;
using Basic;
using Civ.Data;

namespace Test
{
	[TestFixture]
	public class TestClass
	{
		Civilización MyCiv;
		Ciudad MyCiudad;

		public void Main ()
		{

			TestBigIrA ();
		}

		[Test]
		public void Init ()
		{
			Juego.CargaData ();
			Juego.InicializarJuego ();


			MyCiv = Juego.State.Civs [0] as Civilización;
			MyCiudad = MyCiv.Ciudades [0] as Ciudad;

			MyCiv.AlNuevoMensaje += delegate
			{
				while (MyCiv.ExisteMensaje)
				{
					Mensaje m = MyCiv.SiguienteMensaje ();
					Debug.WriteLine (m.ToString ());
				}
			};

		}

		[Test]
		public void TestCambioDiplomacia ()
		{
			Init ();
			MyCiv.Diplomacia.AlCambiarDiplomacia += () => Console.WriteLine ("Cambió diplomacia.");

			var Enemigo = Juego.State.Civs [1];
			Console.WriteLine ("Antes de todo");
			MyCiv.Diplomacia.Add (Enemigo, new EstadoDiplomático ());
			Console.WriteLine ("Medio");
			MyCiv.Diplomacia [Enemigo].PermiteAtacar = true;
			Console.WriteLine ("Al terminar");

		}

		[Test]
		public void TestRecoger ()
		{
			Init ();
			IUnidadRAW u = Juego.Data.Unidades.Elegir ();
			Console.WriteLine (u);
			var pos = new Pseudoposición ();
			Terreno terrA = MyCiudad.Posición ().A;
			Terreno terrB = Juego.State.Terrenos ().Elegir ();
			Juego.State.Topología [terrA, terrB] = 1;
			pos.A = terrA;
			pos.B = terrB;
			pos.Loc = 0.5f;
			var drop = new DropStack (pos);
			Juego.State.Drops.Add (drop);

			var arm = new Armada (MyCiudad);
			MyCiv.Armadas.Add (arm);

			arm.AgregaUnidad (u, 10);
			Debug.WriteLine ("Peso máximo: " + arm [u].Carga.MaxCarga);

			var ord = new OrdenRecoger (arm, drop);
			arm.Orden = ord;

			ord.AlLlegar += delegate
			{
				Debug.WriteLine ("Llegamos a " + ord.StackTarget.Posición);
			};
			ord.AlRegresar += delegate
			{
				Debug.WriteLine ("Regresamos a " + ord.Origen);
			};

			Ciclo (1000);
		}

		[Test]
		public void TestBigIrA ()
		{
			Init ();
			var u = new UnidadRAWCombate ();
			u.Nombre = "Velociraptor";
			u.Velocidad = 10;
			Terreno destino = Juego.State.Terrenos ().Elegir ();
			Assert.NotNull (destino); //Por alguna razón estaba pasando mucho esto
			Assert.AreNotEqual (destino.Vecinos, 0);
			var arm = new Armada (MyCiudad);

			arm.AgregaUnidad (u, 5);
			var ord = new OrdenIrALugar (arm, destino);
			ord.AlAcabarUnaOrden += delegate
			{
				Debug.WriteLine ("Armada está en " + arm.Posición);
			};
			ord.AlTerminar += delegate
			{
				Debug.WriteLine ("Armada llegó a " + arm.Posición);
				Debug.WriteLine ("Ahora está at ease; orden: " + arm.Orden);
			};
			arm.Orden = ord;
			Console.WriteLine ("Entrando al ciclo");
			Ciclo (500);
		}

		[Test]
		public void TestReclutar ()
		{
			Init ();
			IUnidadRAW u = Juego.Data.Unidades.Elegir ();
			MyCiudad.Reclutar (u, 3);
		}

		[Test]
		public void TestArmadaDesaparecen ()
		{
			Init ();
			var arm = new Armada (MyCiudad);

			Debug.WriteLine (arm.CivDueño.Armadas);

			Juego.Tick (TimeSpan.FromHours (1));
			Debug.WriteLine (arm.CivDueño.Armadas);
		}

		static void Ciclo (float multiplicadorVelocidad, Action entreCiclos = null)
		{
			Ciclo (multiplicadorVelocidad, TimeSpan.FromSeconds (10), entreCiclos);
		}

		/// <summary>
		/// Ejecuta el ciclo del juego
		/// </summary>
		/// <param name="multiplicadorVelocidad">Multiplicador velocidad.</param>
		/// <param name="entreCiclos">Entre ciclos.</param>
		/// <param name="duración">Duración de la prueba </param>
		static void Ciclo (float multiplicadorVelocidad,
		                   TimeSpan duración,
		                   Action entreCiclos = null)
		{
			DateTime timer = DateTime.Now;
			while (duración.Ticks > 0)
			{				
				TimeSpan tiempo = DateTime.Now - timer;
				timer = DateTime.Now;
				duración -= tiempo;
				tiempo = new TimeSpan ((long)(tiempo.Ticks * multiplicadorVelocidad));

				// Console.WriteLine (t);
				Juego.Tick (tiempo);

				entreCiclos?.Invoke ();
				if (Juego.State.Civs.Count == 0)
					throw new Exception ("Ya se acabó el juego :3");
			}

		}

		[Test]
		public void TestCiudad ()
		{
			Init ();
			ICiudad cd = Juego.State.CiudadesExistentes ().Elegir ();
			Action Entreturnos = delegate
			{
				if (Juego.Rnd.NextDouble () < 0.001f)
				{
					foreach (var x in cd.Almacén.Recursos)
					{
						Debug.WriteLine (
							string.Format (
								"{0}: {1}({2})",
								x,
								cd.Almacén [x],
								cd.CalculaDeltaRecurso (x))
						);
					}

				}
			};
			
			Ciclo (1000, Entreturnos);
		}

		[Test]
		public void TestGeneradorArmadas ()
		{
			Init ();
			var u = new UnidadRAW ();
			u.Nombre = "Gordo";
			var reg = new ReglaGeneraciónPuntuación ();
			reg.ClaseArmada = new List<Tuple<IUnidadRAW, ulong>> ();
			reg.ClaseArmada.Add (new Tuple<IUnidadRAW, ulong> (u, 100));
			reg.MaxPuntuación = float.PositiveInfinity;
			reg.MinPuntuación = 0;
			//g_.BarbGen.Reglas.Add(reg);

			Ciclo (1000);

		}

		[Test]
		public void TestPeleaArmadas ()
		{
			Init ();
			var c1 = (Civilización)Juego.State.Civs [0];
			var c2 = (Civilización)Juego.State.Civs [1];
			var diplomaciaGuerra = new EstadoDiplomático ();
			diplomaciaGuerra.PermiteAtacar = true;
			c1.Diplomacia.Add (c2, diplomaciaGuerra);
			c2.Diplomacia.Add (c1, diplomaciaGuerra);
			Juego.State.Civs.Add (c1);
			Juego.State.Civs.Add (c2);

			var p = new Pseudoposición ();
			p.A = Juego.State.Terrenos ().Elegir ();
			p.Loc = 0;

			var u = new UnidadRAWCombate ();
			u.Nombre = "Guerrero";
			u.Flags.Add ("A pie");

			var uGordo = new UnidadRAWCombate ();
			uGordo.Defensa = 10;
			uGordo.Nombre = "Hulk";
			uGordo.Flags.Add ("A pie");
			uGordo.Mods.Add ("A pie", 1f);
			uGordo.Dispersión = 0.1f;

			var ac1 = new Armada (c1, p);
			ac1.AgregaUnidad (u, 150);
			ac1 [u].Entrenamiento = 1;
			//ac1.AgregaUnidad(uGordo, 1);
			var ac2 = new Armada (c2, p);
			ac2.AgregaUnidad (u, 150);

			ac1 [u].AlSerAtacado += delegate(Civ.Combate.IAnálisisCombate anal)
			{
				Console.WriteLine (anal.Análisis ());
				Console.WriteLine (ac1);
				Console.WriteLine (ac2);
			};

			// Listos para matarse
			Ciclo (50);

			Console.WriteLine ("Ganador: " + (ac1.Unidades.Count > 0 ? "1" : "2"));

		}
	}
}
