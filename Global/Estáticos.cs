using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Civ;
using Civ.Options;
using Civ.Bárbaros;
using Graficas;
using System.IO;
using Basic;
using ListasExtra.Extensiones;

namespace Global
{
	/// <summary>
	/// Los objetos globales.
	/// </summary>	
	public static class Juego
	{
		static NewGameOptions PrefsJuegoNuevo = new NewGameOptions ();
		public static GeneradorArmadasBarbaras BarbGen = new GeneradorArmadasBarbaras ();
		[DataMember (Name = "Data")]
		public static GameData Data = new GameData ();
		public static GameState State = new GameState ();

		static Juego ()
		{
			BarbGen.Reglas.Add (new ReglaGeneracionBarbaraGeneral ());
		}

		public static void Tick (TimeSpan t)
		{
			foreach (ITickable Civ in State.Civs)
			{
				Civ.Tick (t);
			}

			// Ticks de terreno
			foreach (var x in State.Topología.Nodos)
			{
				x.Tick (t);
			}

			// Peleas entre armadas de Civs enemigas
			for (int i = 1; i < State.Civs.Count; i++)
			{
				ICivilización civA = State.Civs [i];
				for (int j = 0; j < i; j++)
				{
					ICivilización civB = State.Civs [j];
					{
						foreach (var ArmA in civA.Armadas)
						{
							foreach (var ArmB in civB.Armadas)
							{
								if ((civA.Diplomacia.PermiteAtacar (ArmB)) ||
								    (civB.Diplomacia.PermiteAtacar (ArmA)))
								{
									if (ArmA.Posición.Equals (ArmB.Posición))
									{
										ArmA.Pelea (ArmB, t);
									}
								}
							}
						}
					}
				}
			}

			// Matar Civs sin ciudades.
			EliminarMuertos ();

			// Generar b��rbaros
			BarbGen.Tick (t);
		}

		/// <summary>
		/// Elimina civilizaciones muertas
		/// </summary>
		static void EliminarMuertos ()
		{
			foreach (var x in State.CivsVivas())
			{
				if (x.Ciudades.Count == 0)
					State.Civs.Remove (x);
			}
		}

		#region IO

		const string archivo = "Data.xml";

		/// <summary>
		/// Carga del archivo predeterminado.
		/// </summary>
		public static void CargaData ()
		{
			Stream stream = System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream ("Data.xml");
			Data = Store.Store<GameData>.Deserialize (stream);
			stream.Dispose ();
		}

		public static void GuardaData ()
		{
			Store.Store<GameData>.Serialize (archivo, Data);
		}

		public static void GuardaData (string f)
		{
			Store.Store<GameData>.Serialize (f, Data);
		}

		#endregion

		/// <summary>
		/// Un Random compartido
		/// </summary>
		public static Random Rnd = new Random ();

		/// <summary>
		/// Inicializa el g_State, a partir de el g_Data.
		/// Usarse cuando se quiera iniciar un juego.
		/// </summary>
		public static void InicializarJuego ()
		{
			//State = new GameState();

			// Hacer la topolog�a
			var Terrenos = new List<Terreno> ();
			State.Topología = new Grafo<Terreno> ();
			State.Mapa = new Graficas.Continuo.Continuo<Terreno> (State.Topología);

			Terreno T;
			Ecosistema Eco;
			Civilización C;
			Ciudad Cd;

			for (int i = 0; i < PrefsJuegoNuevo.NumTerrenos; i++)
			{
				Eco = Data.Ecosistemas [Rnd.Next (Data.Ecosistemas.Count)]; // Selecciono un ecosistema al azar.
				T = new Terreno (Eco);                               // Le asocio un terreno consistente con el ecosistema.
				Terrenos.Add (T);
				//State.Topologia.AgregaVertice(T, State.Topologia.Nodos[r.Next(State.Topologia.Nodos.Length)], 1 + (float)r.NextDouble());
			}

			//State.Topologia = Graficas.Grafica<Civ.Terreno>.GeneraGraficaAleatoria(Terrenos);
			ConstruirTopología (Terrenos);
			/*
			// Vaciar la topolog�a en cada Terreno
			foreach (var x in State.Topologia.Nodos)
			{
				Terreno a = x;
				Terreno b = x;
				a.Vecinos[b] = State.Topologia[a, b];
				b.Vecinos[a] = State.Topologia[b, a];
			}
			*/
			// Asignar una ciudad de cada civilizaci�n en terrenos vac�os y distintos lugares.
			var Terrs = State.TerrenosLibres ();
			for (int i = 0; i < PrefsJuegoNuevo.NumCivs; i++)
			{
				C = new Civilización ();

				T = Terrs.Aleatorio (Rnd);
				Terrs.Remove (T);

				Cd = new Ciudad (C, T, PrefsJuegoNuevo.PoblacionInicial);
				C.AddCiudad (Cd);

				State.Civs.Add (C);
			}

			// Construir las rutas óptimas
			State.Rutas = new ConjuntoRutasÓptimas<Terreno> (State.Topología);

			// Incluir el alimento inicial en cada ciudad
			foreach (var c in State.CiudadesExistentes())
			{
				c.Almacen [Juego.Data.RecursoAlimento] = PrefsJuegoNuevo.AlimentoInicial;
			}
		}

		public static void ConstruirTopología (IEnumerable<Terreno> lista)
		{
			foreach (var x in lista)
			{
				foreach (var y in new List<Terreno> (lista)) // Evitar múltiples enumeraciones de lista
				{
					if (Rnd.NextDouble () < PrefsJuegoNuevo.Compacidad)
					{
						State.Topología [x, y] =
							PrefsJuegoNuevo.MinDistNodos + (float)Rnd.NextDouble () * (PrefsJuegoNuevo.MaxDistNodos - PrefsJuegoNuevo.MinDistNodos);
					}
				}
			}
			State.Topología.EsSimetrico = true;
		}

		#region Unicidad de nombres

		static string ReadRandomLine (string resourseName)
		{
			Stream stream = System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream (resourseName);
			string ret = ReadRandomLine (stream);
			stream.Dispose ();
			return ret;
		}

		static string ReadRandomLine (Stream stream)
		{
			var read = new StreamReader (stream);
			var nombres = new List<string> ();
			while (!read.EndOfStream)
			{
				nombres.Add (read.ReadLine ());
			}
			read.Dispose ();

			string baseNombre = nombres [Rnd.Next (nombres.Count)];
			return baseNombre;
		}

		/// <summary>
		/// Devuelve un nombre de civilizaciónn único
		/// </summary>
		/// <returns>The unique civ name.</returns>
		public static string NombreCiudadÚnico ()
		{
			string baseNombre = ReadRandomLine ("NombresCiudad.txt");
			string unique = HacerÚnico (
				                baseNombre,
				                State.CiudadesExistentes ().ConvertirLista (x => x.Nombre));
			return unique;
		}

		/// <summary>
		/// Devuelve un nombre de civilización único
		/// </summary>
		/// <returns>The unique civ name.</returns>
		public static string NombreCivÚnico ()
		{
			string baseNombre = ReadRandomLine ("NombresCiv.txt");
			string unique = HacerÚnico (
				                baseNombre,
				                State.Civs.ConvertirLista (x => x.Nombre));

			return unique;
		}

		/// <summary>
		/// Devuelve un string que se genera al agregar un entero al final 
		/// de tal forma que el nombre no esté en una lista determinada.
		/// </summary>
		/// <returns>The unico.</returns>
		/// <param name="str">String base</param>
		/// <param name="universo">Lista de strings que debe evitar devolver</param>
		/// <param name="enumInicial">Número entero con el que se empieza la enumeración en caso de repetición</param>
		static string HacerÚnico (string str,
		                          ICollection<string> universo,
		                          int enumInicial = 0)
		{
			if (!universo.Contains (str))
				return str;

			string strtmp;
			int i = enumInicial;

			while (true)
			{
				strtmp = str + (i++);
				if (!universo.Contains (strtmp))
					return strtmp;
			}
		}

		#endregion
	}
}
