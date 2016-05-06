using System;
using System.Collections.Generic;
using Civ;
using Civ.Options;
using Civ.Bárbaros;
using System.IO;
using Basic;
using ListasExtra.Extensiones;
using Civ.Topología;
using Graficas.Grafo;
using Graficas.Rutas;
using Civ.ObjetosEstado;

namespace Civ.Global
{
	/// <summary>
	/// Los objetos globales.
	/// </summary>	
	[Serializable]
	public class Juego
	{
		public static Juego Instancia = new Juego ();
		public static NewGameOptions PrefsJuegoNuevo = new NewGameOptions ();
		public GeneradorArmadasBarbaras BarbGen = new GeneradorArmadasBarbaras ();
		public GameData GData = new GameData ();
		public GameState GState = new GameState ();

		public static GameData Data
		{
			get
			{
				return Instancia.GData;
			}
			set
			{
				Instancia.GData = value;
			}
		}

		public static GameState State
		{
			get
			{
				return Instancia.GState;
			}
			set
			{
				Instancia.GState = value;
			}
		}

		Juego ()
		{
			BarbGen.Reglas.Add (new ReglaGeneracionBarbaraGeneral ());
		}

		public void Tick (TimeSpan t)
		{
			foreach (ITickable Civ in GState.Civs)
			{
				Civ.Tick (t);
			}

			// Ticks de terreno
			foreach (var x in GState.Topología.Nodos)
			{
				x.Tick (t);
			}

			// Peleas entre armadas de Civs enemigas
			for (int i = 1; i < GState.Civs.Count; i++)
			{
				ICivilización civA = GState.Civs [i];
				for (int j = 0; j < i; j++)
				{
					ICivilización civB = GState.Civs [j];
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

			// Generar bárbaros
			BarbGen.Tick (t);
		}

		/// <summary>
		/// Elimina civilizaciones muertas
		/// </summary>
		void EliminarMuertos ()
		{
			foreach (var x in GState.CivsVivas())
			{
				if (x.Ciudades.Count == 0)
					GState.Civs.Remove (x);
			}
		}

		#region IO

		public const string ArchivoData = "Data.bin";
		public const string ArchivoState = "game.state";

		/// <summary>
		/// Carga del archivo predeterminado.
		/// </summary>
		public static void CargaData ()
		{
			// TODO
			Juego.Data = Store.BinarySerialization.ReadFromBinaryFile<GameData> (ArchivoData);
			//ImportMachine.Importar ();
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
		public void Inicializar ()
		{
			Juego.CargaData ();
			//State = new GameState();

			// Hacer la topolog�a
			var Terrenos = new List<Terreno> ();
			GState.Topología = new Grafo<Terreno> ();
			GState.Mapa = new Mapa (GState.Topología);

			Terreno T;
			Ecosistema Eco;
			Civilización C;
			Ciudad Cd;

			for (int i = 0; i < PrefsJuegoNuevo.NumTerrenos; i++)
			{
				Eco = GData.Ecosistemas.Elegir ();
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
			var Terrs = GState.TerrenosLibres ();
			for (int i = 0; i < PrefsJuegoNuevo.NumCivs; i++)
			{
				C = new Civilización ();

				T = Terrs.Aleatorio (Rnd);
				Terrs.Remove (T);

				Cd = new Ciudad (C, T, PrefsJuegoNuevo.PoblacionInicial);
				C.AddCiudad (Cd);

				GState.Civs.Add (C);
			}

			// Construir las rutas óptimas
			GState.Rutas = new ConjuntoRutasÓptimas<Terreno> (GState.Topología);

			// Incluir el alimento inicial en cada ciudad
			foreach (var c in GState.CiudadesExistentes())
			{
				c.Almacén [Instancia.GData.RecursoAlimento] = PrefsJuegoNuevo.AlimentoInicial;
			}
		}

		public const string FileName = "game.sav";

		public static void Cargar (string filename = FileName)
		{
			Instancia = Store.BinarySerialization.ReadFromBinaryFile<Juego> (filename);
		}

		public static void Guardar (string filename = FileName)
		{
			Store.BinarySerialization.WriteToBinaryFile (filename, Instancia);
		}

		public void ConstruirTopología (IEnumerable<Terreno> lista)
		{
			foreach (var x in lista)
			{
				foreach (var y in new List<Terreno> (lista)) // Evitar múltiples enumeraciones de lista
				{
					if (Rnd.NextDouble () < PrefsJuegoNuevo.Compacidad)
					{
						GState.Topología [x, y] =
							PrefsJuegoNuevo.MinDistNodos + (float)Rnd.NextDouble () * (PrefsJuegoNuevo.MaxDistNodos - PrefsJuegoNuevo.MinDistNodos);
					}
				}
			}
			GState.Topología.EsSimétrico = true;
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
				                Instancia.GState.CiudadesExistentes ().ConvertirLista (x => x.Nombre));
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
				                Instancia.GState.Civs.ConvertirLista (x => x.Nombre));

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
		                          System.Collections.Generic.ICollection<string> universo,
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