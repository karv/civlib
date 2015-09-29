using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Civ;
using Civ.Options;
using Civ.Barbaros;

namespace Global
{
	/// <summary>
	/// Los objetos globales.
	/// </summary>	
	public static class Juego
	{
		static NewGameOptions PrefsJuegoNuevo = new NewGameOptions();
		public static GeneradorArmadasBarbaras BarbGen = new GeneradorArmadasBarbaras();
		[DataMember(Name = "Data")]
		public static GameData Data = new GameData();
		public static GameState State = new GameState();

		static Juego()
		{
			BarbGen.Reglas.Add(new ReglaGeneracionBarbaraGeneral());
		}

		public static void Tick(TimeSpan t)
		{
			foreach (ITickable Civ in State.Civs)
			{
				Civ.Tick(t);
			}

			// Ticks de terreno
			foreach (var x in State.Topologia.Nodos)
			{
				x.Tick(t);
			}

			// Peleas entre armadas de Civs enemigas
			for (int i = 1; i < State.Civs.Count; i++)
			{
				ICivilizacion civA = State.Civs[i];
				for (int j = 0; j < i; j++)
				{
					ICivilizacion civB = State.Civs[j];
					{
						foreach (var ArmA in civA.Armadas)
						{
							foreach (var ArmB in civB.Armadas)
							{
								if ((civA.Diplomacia.PermiteAtacar(ArmB)) ||
								    (civB.Diplomacia.PermiteAtacar(ArmA)))
								{
									if (ArmA.Posicion.Equals(ArmB.Posicion))
									{
										ArmA.Pelea(ArmB, t);
									}
								}
							}
						}
					}
				}
			}

			// Matar Civs sin ciudades.
			State.Civs.RemoveAll(x => (x.Ciudades.Count == 0));

			// Generar b¨¢rbaros
			BarbGen.Tick(t);
		}

		#region IO

		const string archivo = "Data.xml";

		/// <summary>
		/// Carga del archivo predeterminado.
		/// </summary>
		public static void CargaData()
		{
			System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Data.xml");
			Data = Store.Store<GameData>.Deserialize(stream);
			stream.Dispose();
		}

		public static void GuardaData()
		{
			Store.Store<GameData>.Serialize(archivo, Data);
		}

		public static void GuardaData(string f)
		{
			Store.Store<GameData>.Serialize(f, Data);
		}

		#endregion

		/// <summary>
		/// Un Random compartido
		/// </summary>
		public static Random Rnd = new Random();

		/// <summary>
		/// Inicializa el g_State, a partir de el g_Data.
		/// Usarse cuando se quiera iniciar un juego.
		/// </summary>
		public static void InicializarJuego()
		{
			//State = new GameState();

			// Hacer la topología
			var Terrenos = new List<Terreno>();
			State.Mapa = new Graficas.Continuo.Continuo<Terreno>(State.Topologia);

			Terreno T;
			Ecosistema Eco;
			Civilizacion C;
			Ciudad Cd;

			for (int i = 0; i < PrefsJuegoNuevo.NumTerrenos; i++)
			{
				Eco = Data.Ecosistemas[Rnd.Next(Data.Ecosistemas.Count)]; // Selecciono un ecosistema al azar.
				T = new Terreno(Eco);                               // Le asocio un terreno consistente con el ecosistema.
				Terrenos.Add(T);
				//State.Topologia.AgregaVertice(T, State.Topologia.Nodos[r.Next(State.Topologia.Nodos.Length)], 1 + (float)r.NextDouble());
			}

			//State.Topologia = Graficas.Grafica<Civ.Terreno>.GeneraGraficaAleatoria(Terrenos);
			ConstruirTopologia(Terrenos);
			/*
			// Vaciar la topología en cada Terreno
			foreach (var x in State.Topologia.Nodos)
			{
				Terreno a = x;
				Terreno b = x;
				a.Vecinos[b] = State.Topologia[a, b];
				b.Vecinos[a] = State.Topologia[b, a];
			}
			*/
			// Asignar una ciudad de cada civilización en terrenos vacíos y distintos lugares.
			List<Terreno> Terrs = State.ObtenerListaTerrenosLibres();
			for (int i = 0; i < PrefsJuegoNuevo.NumCivs; i++)
			{
				C = new Civilizacion();
				T = Terrs[Rnd.Next(Terrs.Count)];         // Éste es un elemento aleatorio de un Terreno libre.
				Terrs.Remove(T);

				Cd = new Ciudad(C, T, PrefsJuegoNuevo.PoblacionInicial);
				C.AddCiudad(Cd);

				State.Civs.Add(C);
			}

			// Incluir el alimento inicial en cada ciudad
			foreach (var c in State.CiudadesExistentes())
			{
				c.AlimentoAlmacen = PrefsJuegoNuevo.AlimentoInicial;
			}
		}

		public static void ConstruirTopologia(IEnumerable<Terreno> lista)
		{
			foreach (var x in lista)
			{
				foreach (var y in new List<Terreno> (lista)) // Evitar m¨²ltiples enumeraciones de lista
				{
					if (Rnd.NextDouble() < PrefsJuegoNuevo.Compacidad)
					{
						State.Topologia[x, y] =
							PrefsJuegoNuevo.MinDistNodos + (float)Rnd.NextDouble() * (PrefsJuegoNuevo.MaxDistNodos - PrefsJuegoNuevo.MinDistNodos);
					}
				}
			}
		}

		#region Unicidad de nombres

		/// <summary>
		/// Devuelve un nombre de civilizaci¨®n ¨²nico
		/// </summary>
		/// <returns>The unique civ name.</returns>
		public static string NombreCivUnico()
		{
			// Copiar el contenido en una lista
			System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("NombresCiv.txt");
			var read = new System.IO.StreamReader(stream);
			var nombres = new List<string>();
			while (!read.EndOfStream)
			{
				nombres.Add(read.ReadLine());
			}
			stream.Dispose();
			read.Dispose();

			string baseNombre = nombres[Rnd.Next(nombres.Count)];
			string unique = HacerUnico(baseNombre, State.Civs.ConvertAll(c => c.Nombre));

			return unique;
		}

		/// <summary>
		/// Devuelve un nombre de civilizaci¨®n ¨²nico
		/// </summary>
		/// <returns>The unique civ name.</returns>
		public static string NombreCiudadUnico()
		{
			// Copiar el contenido en una lista
			System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("NombresCiudad.txt");
			var read = new System.IO.StreamReader(stream);
			var nombres = new List<string>();
			while (!read.EndOfStream)
			{
				nombres.Add(read.ReadLine());
			}
			stream.Dispose();
			read.Dispose();

			string baseNombre = nombres[Rnd.Next(nombres.Count)];
			string unique = HacerUnico(baseNombre, new List<ICiudad>(State.CiudadesExistentes()).ConvertAll(c => c.Nombre));

			return unique;
		}


		/// <summary>
		/// Devuelve un string que se genera al agregar un entero al final 
		/// de tal forma que el nombre no est¨¦ en una lista determinada.
		/// </summary>
		/// <returns>The unico.</returns>
		/// <param name="str">String base</param>
		/// <param name="universo">Lista de strings que debe evitar devolver</param>
		/// <param name="enumInicial">N¨²mero entero con el que se empieza la enumeraci¨®n en caso de repetici¨®n</param>
		static string HacerUnico(string str, ICollection<string> universo, int enumInicial = 0)
		{
			if (!universo.Contains(str))
				return str;

			string strtmp;
			int i = enumInicial;

			while (true)
			{
				strtmp = str + (i++);
				if (!universo.Contains(strtmp))
					return strtmp;
			}
		}

		#endregion
	}
}
