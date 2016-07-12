using System;
using System.Collections.Generic;
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
	/// <summary>
	/// Clase estática con variables de herramienta globales, como un Random
	/// </summary>
	public static class HerrGlobal
	{
		/// <summary>
		/// Generador aleatorio
		/// </summary>
		public static Random Rnd = new Random ();
	}

	/// <summary>
	/// Los objetos globales.
	/// </summary>	
	[Serializable]
	public class Juego : IInicializable
	{
		#region El juego

		/// <summary>
		/// La instancia del juego.
		/// </summary>
		public static Juego Instancia = new Juego ();

		/// <summary>
		/// Las opciones que se utilizan al momento de crear un juego
		/// </summary>
		[NonSerialized]
		public static NewGameOptions PrefsJuegoNuevo = new NewGameOptions ();

		#endregion

		#region Pausa

		[NonSerialized]
		bool _pausado;

		/// <summary>
		/// Devuelve o establece si el juego está pausado.
		/// </summary>
		public bool Pausado
		{
			get
			{
				return _pausado;
			}
			set
			{
				_pausado = value;
				if (!value)
					timer = DateTime.Now; // Al despausar se reestablece el timer
				AlCambiarEstadoPausa?.Invoke (this, EventArgs.Empty);
			}
		}

		#endregion

		#region Autoguardado

		[NonSerialized]
		Cronómetro _cronoAutoguardado;

		/// <summary>
		/// Devuelve o establece el tiempo entre autoguardados.
		/// </summary>
		/// <remarks>El valor TimeSpam.Zero hace que no exista función autoguardado.</remarks>
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

		/// <summary>
		/// Ejecuta un autoguardado.
		/// </summary>
		public void EjecutarAutoguardado (object sender, EventArgs args)
		{
			const string file_output = "auto.sav";
			Debug.WriteLine ("Iniciando autoguardado", "autosave");
			Pausado = true;
			GState.Guardar (file_output);
			Pausado = false;
			Debug.WriteLine ("Autoguardado exitoso en " + file_output, "autosave");
		}

		#endregion

		#region Data

		/// <summary>
		/// Generador de armadas bárbaras
		/// </summary>
		[NonSerialized]
		public GeneradorArmadasBarbaras BarbGen;
		/// <summary>
		/// Data del juego actual
		/// </summary>
		public GameData GData = new GameData ();
		/// <summary>
		/// Estado del juego actual
		/// </summary>
		public GameState GState = new GameState ();

		/// <summary>
		/// Devuelve la base de datos de reglas del juego
		/// </summary>
		/// <value>The data.</value>
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

		/// <summary>
		/// Devuelve el estado del juego
		/// </summary>
		/// <value>The state.</value>
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

		#endregion

		#region Herramientas

		// Es hashset porque no quiero repeticiones
		/// <summary>
		/// Cronómetros para eventos cronoetrados de un cliente.
		/// </summary>
		[NonSerialized]
		public HashSet<Cronómetro> Cronómetros;

		#endregion

		#region ctor

		[OnSerialized]
		void Defaults ()
		{
			BarbGen = new GeneradorArmadasBarbaras ();
			BarbGen.Reglas.Add (new ReglaGeneracionBarbaraGeneral ());

			Cronómetros = new HashSet<Cronómetro> ();
			_cronoAutoguardado = new Cronómetro ();
			_cronoAutoguardado.AlLlamar += EjecutarAutoguardado;
			Cronómetros.Add (_cronoAutoguardado);
		}

		Juego ()
		{
			Defaults ();
		}

		/// <summary>
		/// Inicializa el juego antes de ejecutarse
		/// </summary>
		public void Inicializar ()
		{
			Defaults ();
			State.PendientesMorir = new HashSet<ICivilización> ();
			foreach (var civ in State.Civs)
				civ.Inicializar ();
			
		}

		/// <summary>
		/// Inicializa el g_State, a partir de el g_Data.
		/// Usarse cuando se quiera iniciar un juego.
		/// </summary>
		public void InicializarNuevoJuego ()
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

		/// <summary>
		/// Carga el juego desde un archivo.
		/// </summary>
		/// <param name="filename">Archivo</param>
		public static void Cargar (string filename = ArchivoState)
		{
			try
			{
				Instancia = Store.BinarySerialization.ReadFromBinaryFile<Juego> (filename);
				Instancia.Defaults ();
				Debug.WriteLine ("Carga exitosa de archivo " + filename, "IO");
			}
			catch (Exception ex)
			{
				Debug.WriteLine (
					"No se puede cargar archivo de guardado, posiblemente corrupto.",
					"IO");
				Debug.WriteLine (ex);

				try
				{
					var j = Store.BinarySerialization.ReadFromBinaryFile<object> (filename);
					Debug.WriteLine ("Objeto en archivo: " + j, "IO");
					Debug.WriteLine ("Del tipo " + j.GetType ());

				}
				catch (Exception exInn)
				{
					Console.WriteLine ("Tipo de objeto ajeno al proyecto o formato desconocido.");
					Console.WriteLine (exInn);
				}

				Console.WriteLine ("Iniciando nuevo juego");
				Instancia.InicializarNuevoJuego ();
			}
			finally
			{
				Console.WriteLine (string.Format (
					"Cargado de imagen exitoso: {0}",
					filename), "IO");
			}
		}

		/// <summary>
		/// Guarda el juego a un archivo.
		/// </summary>
		/// <param name="filename">Archivo</param>
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

		/// <summary>
		/// Construye el grafo de la topología del juego.
		/// </summary>
		/// <param name="lista">Lista de terrenos</param>
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

		/// <summary>
		/// Ejecutar las suscripciones a eventos.
		/// </summary>
		void suscripciones ()
		{
			foreach (var x in GState.CiudadesExistentes ())
				x.AlCambiarDueño += EliminarMuertos;
			foreach (var x in GState.ArmadasExistentes ())
				x.AlVaciarse += EliminarMuertos;
		}

		#endregion

		#region Información

		/// <summary>
		/// Devuelve las armadas bárbaras.
		/// Inseguro al anidar ciclos
		/// </summary>
		static IEnumerable<Armada> ArmadasBárbaras ()
		{
			return State.ArmadasExistentes ().Where (z => z.CivDueño.EsBárbaro);
		}

		#endregion

		#region IO

		/// <summary>
		/// Archivo que se usa para cargar la base de datos
		/// </summary>
		public const string ArchivoData = "Data.bin";
		/// <summary>
		/// Archivo que se usa para guardar/cargar estado del juego.
		/// </summary>
		public const string ArchivoState = "game.state";

		/// <summary>
		/// Carga del archivo predeterminado.
		/// </summary>
		public static void CargaData ()
		{
			Juego.Data = Store.BinarySerialization.ReadFromBinaryFile<GameData> (ArchivoData);
		}

		#endregion

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

		#region Ticks

		[NonSerialized]
		DateTime timer = DateTime.Now;
		/// <summary>
		/// Coeficiente de velocidad del juego.
		/// </summary>
		public const float MultiplicadorVelocidad = 120;

		/// <summary>
		/// Devuelve si el juego se está terminando o está terminado; o establece si debe terminarse.
		/// </summary>
		public bool Terminar;

		/// <summary>
		/// Ejecuta un ciclo
		/// </summary>
		/// <returns>La duración del tick en tiempo real.</returns>
		public TimeSpan Ciclo ()
		{
			var tiempo = DateTime.Now - timer;
			timer = DateTime.Now;
			var timeArgs = new TimeEventArgs (tiempo.TotalHours, MultiplicadorVelocidad);

			// Cronómetros
			foreach (Cronómetro x in Cronómetros)
			{
				// Cronómetros pausados no dan Tick
				if (Pausado && x.SePausa)
					continue;

				// Cronometrar tiempo real o tiempo modificado, dependiendo
				// de los parámetros del cronómetro.
				x.Tick (tiempo);
			}			

			// Console.WriteLine (t);
			Tick (timeArgs);

			if (Juego.State.Civs.Count == 0)
				throw new Exception ("Ya se acabó el juego :3");

			return tiempo;
		}

		/// <summary>
		/// Entra al ciclo principal del juego.
		/// </summary>
		public void Ejecutar ()
		{
			Inicializar ();
			suscripciones ();

			var barb = BarbGen.Armada (State.Civs [0].Ciudades [0].Posición ());
			Debug.WriteLine (barb, "BarbGen");
			barb.Posición.AlColisionar += obj => Debug.WriteLine (
				"Armada genérica chocando con obj", "Armada genérica");

			timer = DateTime.Now;
			while (!Terminar)
			{
				var ccl = Ciclo ();
				EntreCiclos?.Invoke (this, ccl);
			}
			AlTerminar?.Invoke (this, null);
		}

		/// <summary>
		/// Un ciclo
		/// </summary>
		/// <param name="t">Tiempo</param>
		public void Tick (TimeEventArgs t)
		{

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

			try
			{
				// Peleas entre armadas de Civs enemigas
				for (int i = 1; i < GState.Civs.Count; i++)
				{
					ICivilización civA = GState.Civs [i];
					for (int j = 0; j < i; j++)
					{
						ICivilización civB = GState.Civs [j];
						for (int k = 0; k < civA.Armadas.Count; k++)
						{
							var armA = civA.Armadas [k];
							for (int l = 0; l < civB.Armadas.Count; l++)
							{
								var armB = civB.Armadas [l];
								if ((civA.Diplomacia.PermiteAtacar (armB)) ||
								    (civB.Diplomacia.PermiteAtacar (armA)))
								{
									if (armA.Posición.Coincide (armB.Posición))
									{
										armA.Pelea (armB, t.GameTime);
										armB.Pelea (armA, t.GameTime);
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine (ex.Message, "Batalla");
			}

			// Generar bárbaros
			BarbGen.Tick (t);
		}


		/// <summary>
		/// Elimina civilizaciones muertas
		/// </summary>
		void EliminarMuertos (object sender, EventArgs e)
		{
			State.PendientesMorir.UnionWith (GState.Civs.Where (z => z.DeboDestruirme ()));
			foreach (var x in State.PendientesMorir)
			{
				x.Destruirse ();
				State.Civs.Remove (x);
			}
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre al pausar o despausar el juego.
		/// </summary>
		public event EventHandler AlCambiarEstadoPausa;
		/// <summary>
		/// Ocurre al terminar el juego
		/// </summary>
		public event EventHandler AlTerminar;
		/// <summary>
		/// Se ejecuta al final de cada ciclo.
		/// </summary>
		public event EventHandler<TimeSpan> EntreCiclos;

		#endregion
	}
}