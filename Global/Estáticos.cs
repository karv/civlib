using System;
using System.Collections.Generic;
using Civ;
using Civ.Options;
using Civ.Bárbaros;
using System.IO;
using Civ.Global;
using ListasExtra.Extensiones;
using Civ.Topología;
using Graficas.Grafo;
using Graficas.Rutas;
using Civ.ObjetosEstado;
using System.Runtime.Serialization;
using System.Linq;
using System.Diagnostics;

namespace Civ.Global
{
	public static class HerrGlobal
	{
		public static Random Rnd = new Random ();
	}

	/// <summary>
	/// Los objetos globales.
	/// </summary>	
	[Serializable]
	public class Juego
	{
		public static Juego Instancia = new Juego ();

		[NonSerialized]
		bool _pausado;

		public bool Pausado
		{
			get
			{
				return _pausado;
			}
			set
			{
				_pausado = value;
				AlCambiarEstadoPausa?.Invoke ();
			}
		}

		[NonSerialized]
		readonly Cronómetro _cronoAutoguardado = new Cronómetro ();

		public TimeSpan Autoguardado
		{
			get
			{
				return _cronoAutoguardado.Intervalo;
			}
			set
			{
				if (value != TimeSpan.Zero)
				{
					_cronoAutoguardado.Intervalo = value;
					_cronoAutoguardado.Reestablecer ();
					_cronoAutoguardado.Habilitado = true;
					Cronómetros.Add (_cronoAutoguardado);
				}
				else
				{
					Cronómetros.Remove (_cronoAutoguardado);
					_cronoAutoguardado.Habilitado = false;
					_cronoAutoguardado.Intervalo = TimeSpan.Zero;
				}
			}
		}

		[NonSerialized]
		public static NewGameOptions PrefsJuegoNuevo = new NewGameOptions ();
		public GeneradorArmadasBarbaras BarbGen = new GeneradorArmadasBarbaras ();
		public GameData GData = new GameData ();
		public GameState GState = new GameState ();

		// Es hashset porque no quiero repeticiones
		[NonSerialized]
		public HashSet<Cronómetro> Cronómetros = new HashSet<Cronómetro> ();

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

		[OnSerialized]
		void Defaults ()
		{
			Cronómetros = new HashSet<Cronómetro> ();
			Pausado = false;
		}

		Juego ()
		{
			BarbGen.Reglas.Add (new ReglaGeneracionBarbaraGeneral ());
		}

		public void Tick (TimeSpan t)
		{
			// Cronómetros
			foreach (ITickable x in Cronómetros)
				x.Tick (t);
			
			if (Pausado) // Si está pausado no hacer nada
				return;
			
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
				if (!x.Ciudades.Any () && !x.Armadas.Any ())
					x.Destruirse ();
			}
		}

		/// <summary>
		/// Devuelve las armadas bárbaras.
		/// Inseguro al anidar ciclos
		/// </summary>
		static IEnumerable<Armada> ArmadasBárbaras ()
		{
			return State.ArmadasExistentes ().Where (z => z.CivDueño.EsBárbaro);
		}

		#region IO

		public const string ArchivoData = "Data.bin";
		public const string ArchivoState = "game.state";

		/// <summary>
		/// Carga del archivo predeterminado.
		/// </summary>
		public static void CargaData ()
		{
			Juego.Data = Store.BinarySerialization.ReadFromBinaryFile<GameData> (ArchivoData);
		}

		#endregion

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
			var Ecos = new List<Ecosistema> ();

			Terreno T;
			Ecosistema Eco;
			Civilización C;
			Ciudad Cd;



			for (int i = 0; i < PrefsJuegoNuevo.NumTerrenos; i++)
			{
				Eco = GData.Ecosistemas.Elegir ();
				Ecos.Add (Eco);
				T = new Terreno (Eco);                               // Le asocio un terreno consistente con el ecosistema.
				Terrenos.Add (T);
				//State.Topologia.AgregaVertice(T, State.Topologia.Nodos[r.Next(State.Topologia.Nodos.Length)], 1 + (float)r.NextDouble());
			}

			//State.Topologia = Graficas.Grafica<Civ.Terreno>.GeneraGraficaAleatoria(Terrenos);
			Debug.WriteLine ("Construyendo topología");
			GState.Topología = new Grafo<Terreno, float> (Terrenos, true);
			ConstruirTopología (Terrenos);
			Debug.WriteLine ("Construyendo mapa");
			GState.Mapa = new Mapa (GState.Topología);

			// REMARK: Hay una razón para que esto esté después de construir el mapa.
			foreach (var x in Terrenos)
				x.AsignarPosición ();


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

				T = Terrs.Aleatorio (HerrGlobal.Rnd);
				Terrs.Remove (T);

				Cd = new Ciudad (C, T, PrefsJuegoNuevo.PoblacionInicial);
				C.AddCiudad (Cd);

				GState.Civs.Add (C);
			}

			// Construir las rutas óptimas
			GState.Rutas = new ConjuntoRutasÓptimas<Terreno> ();
			Debug.WriteLine ("Optimizar rutas");
			GState.Rutas.Calcular (GState.Topología);

			// Incluir el alimento inicial en cada ciudad
			foreach (var c in GState.CiudadesExistentes())
			{
				c.Almacén [Instancia.GData.RecursoAlimento] = PrefsJuegoNuevo.AlimentoInicial;
			}
		}

		public static void Cargar (string filename = ArchivoState)
		{
			try
			{
				Instancia = Store.BinarySerialization.ReadFromBinaryFile<Juego> (filename);
				Console.WriteLine ("Carga exitosa de archivo " + filename);
			}
			catch (Exception ex)
			{
				Console.WriteLine (DateTime.Now);
				Console.WriteLine ("No se puede cargar archivo de guardado, posiblemente corrupto.");
				Console.WriteLine (ex);

				try
				{
					var j = Store.BinarySerialization.ReadFromBinaryFile<object> (filename);
					Console.WriteLine ("Objeto en archivo: " + j);
					Console.WriteLine ("Del tipo " + j.GetType ());

				}
				catch (Exception exInn)
				{
					Console.WriteLine ("Tipo de objeto ajeno al proyecto.");
					Console.WriteLine (exInn);
				}

				Console.WriteLine ("Iniciando nuevo juego");
				Instancia.Inicializar ();
			}
			finally
			{
				Console.WriteLine (string.Format (
					"[{1}]Cargado de imagen exitoso: {0}",
					filename,
					DateTime.Now));
			}
		}

		public static void Guardar (string filename = ArchivoState)
		{
			Store.BinarySerialization.WriteToBinaryFile<Juego> (filename, Instancia);

			#if DEBUG
			Store.BinarySerialization.WriteToBinaryFile ("data", Instancia.GData);
			Store.BinarySerialization.WriteToBinaryFile ("state", Instancia.GState);
			try
			{
				Store.BinarySerialization.ReadFromBinaryFile<GameData> ("data");
				Store.BinarySerialization.ReadFromBinaryFile<GameState> ("state");
				Store.BinarySerialization.ReadFromBinaryFile<Juego> (filename);
			}
			catch (Exception ex)
			{
				Console.WriteLine ("No se puede guardar archivo de estado de juego. Desconocido");
				Console.WriteLine (ex.Message);
			}
			finally
			{
				Console.WriteLine (string.Format (
					"[{1}]Guardado de imagen exitoso: {0}",
					filename,
					DateTime.Now));
			}
			#endif
		}

		public void ConstruirTopología (IEnumerable<Terreno> lista)
		{
			foreach (var x in lista)
			{
				foreach (var y in new List<Terreno> (lista)) // Evitar múltiples enumeraciones de lista
				{
					if (HerrGlobal.Rnd.NextDouble () < PrefsJuegoNuevo.Compacidad)
					{
						GState.Topología [x, y] =
							PrefsJuegoNuevo.MinDistNodos + (float)HerrGlobal.Rnd.NextDouble () * (PrefsJuegoNuevo.MaxDistNodos - PrefsJuegoNuevo.MinDistNodos);
					}
				}
			}
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

			string baseNombre = nombres [HerrGlobal.Rnd.Next (nombres.Count)];
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

		#region Eventos

		/// <summary>
		/// Ocurre al pausar o despausar el juego.
		/// </summary>
		public event Action AlCambiarEstadoPausa;

		#endregion
	}
}