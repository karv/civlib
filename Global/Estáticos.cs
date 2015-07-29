using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Xml;
using Civ;

namespace Global
{
	/// <summary>
	/// Los objetos globales.
	/// </summary>	
	public static class g_
	{
		static Prefs preferencias = new Prefs();
		[DataMember(Name = "Data")]
		public static g_Data Data = new g_Data();
		public static g_State State = new g_State();
		private const string archivo = "Data.xml";

		public static void Tick(float t = 1)
		{
			foreach (Civ.ITickable Civ in State.Civs)
			{
				Civ.Tick(t);
			}

			// Ticks de terreno
			foreach (var x in State.Topologia.Nodos)
			{
				x.Tick(t);
			}

			// Peleas entre armadas de Civs enemigas
			for (int i = 0; i < State.Civs.Count; i++)
			{
				Civ.Civilizacion civA = State.Civs[i];
				for (int j = 0; j < i; j++)
				{
					Civ.Civilizacion civB = State.Civs[j];
					if ((civA.Diplomacia.ContainsKey(civB) && civA.Diplomacia[civB].PermiteAtacar) ||
					    (civB.Diplomacia.ContainsKey(civA) && civB.Diplomacia[civA].PermiteAtacar))
					{
						foreach (var ArmA in civA.Armadas)
						{
							foreach (var ArmB in civB.Armadas)
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

			// Matar Civs sin ciudades.
			State.Civs.RemoveAll(x => (x.getCiudades.Count == 0));
		}

		/// <summary>
		/// Carga del archivo predeterminado.
		/// </summary>
		public static void CargaData()
		{
			Data = Store.Store<g_Data>.Deserialize(archivo);
		}

		public static void GuardaData()
		{
			Store.Store<g_Data>.Serialize(archivo, Data);
		}

		public static void GuardaData(string f)
		{
			Store.Store<g_Data>.Serialize(f, Data);
		}

		static Random r = new Random();

		/// <summary>
		/// Inicializa el g_State, a partir de el g_Data.
		/// Usarse cuando se quiera iniciar un juego.
		/// </summary>
		public static void InicializarJuego()
		{
			State = new g_State();

			// Hacer la topología
			List<Civ.Terreno> Terrenos = new List<Civ.Terreno>();
			State.Topologia = new Graficas.Grafica<Civ.Terreno>();
			State.Mapa = new Graficas.Continuo.Continuo<Civ.Terreno>(State.Topologia);

			Civ.Terreno T;
			Civ.Ecosistema Eco;
			Civ.Civilizacion C;
			Civ.Ciudad Cd;

			for (int i = 0; i < numTerrenosIniciales; i++)
			{
				Eco = Data.Ecosistemas[r.Next(Data.Ecosistemas.Count)]; // Selecciono un ecosistema al azar.
				T = new Civ.Terreno(Eco);                               // Le asocio un terreno consistente con el ecosistema.
				Terrenos.Add(T);
				//State.Topologia.AgregaVertice(T, State.Topologia.Nodos[r.Next(State.Topologia.Nodos.Length)], 1 + (float)r.NextDouble());
			}

			//State.Topologia = Graficas.Grafica<Civ.Terreno>.GeneraGraficaAleatoria(Terrenos);
			ConstruirTopologia(Terrenos);

			// Vaciar la topología en cada Terreno
			foreach (var x in State.Topologia.Vecinos.Keys)
			{
				Civ.Terreno a = (Civ.Terreno)x.Item1;
				Civ.Terreno b = (Civ.Terreno)x.Item2;
				a.Vecinos[b] = State.Topologia[a, b];
				b.Vecinos[a] = State.Topologia[b, a];
			}

			// Asignar una ciudad de cada civilización en terrenos vacíos y distintos lugares.
			for (int i = 0; i < numCivsIniciales; i++)
			{
				C = new Civ.Civilizacion();
				// C.Nombre = DateTime.Now.Millisecond.ToString();
				List<Civ.Terreno> Terrs = State.ObtenerListaTerrenosLibres();
				T = Terrs[r.Next(Terrs.Count)];         // Éste es un elemento aleatorio de un Terreno libre.

				Cd = new Civ.Ciudad(C, T, popInicial);
				C.addCiudad(Cd);

				State.Civs.Add(C);
			}

			// Incluir el alimento inicial en cada ciudad
			foreach (var c in State.getCiudades())
			{
				c.AlimentoAlmacen = preferencias.AlimentoInicial;
			}
		}
		// constantes
		const int numTerrenosIniciales = 40;
		const int numCivsIniciales = 4;
		const int popInicial = 10;
		const float compacidad = 0.15f;
		//Qu¨¦ tan probable es que dos nodos sean vecinos.
		const float minDistNodo = 0.3f;
		//M¨ªnima y m¨¢xima distancia entre nodos adyacentes de la topolog¨ªa.
		const float maxDistNodo = 2f;


		public static void ConstruirTopologia(IEnumerable<Civ.Terreno> lista)
		{
			foreach (var x in lista)
			{
				foreach (var y in lista)
				{
					if (r.NextDouble() < compacidad)
					{
						State.Topologia.AgregaVertice(x, y, minDistNodo + (float)r.NextDouble() * (maxDistNodo - minDistNodo));
					}
				}
			}
		}

		/// <summary>
		/// Devuelve un nombre de civilizaci¨®n ¨²nico
		/// </summary>
		/// <returns>The unique civ name.</returns>
		public static string getUniqueCivName()
		{
			// Copiar el contenido en una lista
			System.IO.StreamReader read = new System.IO.StreamReader("NombresCiv.txt");
			List<string> nombres = new List<string>();
			while (!read.EndOfStream)
			{
				nombres.Add(read.ReadLine());
			}

			string baseNombre = nombres[r.Next(nombres.Count)];
			string unique = HacerUnico(baseNombre, State.Civs.ConvertAll(c => c.Nombre));

			return unique;
		}

		/// <summary>
		/// Devuelve un nombre de civilizaci¨®n ¨²nico
		/// </summary>
		/// <returns>The unique civ name.</returns>
		public static string getUniqueCityName()
		{
			// Copiar el contenido en una lista
			System.IO.StreamReader read = new System.IO.StreamReader("NombresCiudad.txt");
			List<string> nombres = new List<string>();
			while (!read.EndOfStream)
			{
				nombres.Add(read.ReadLine());
			}

			string baseNombre = nombres[r.Next(nombres.Count)];
			string unique = HacerUnico(baseNombre, new List<Ciudad>(State.getCiudades()).ConvertAll(c => c.Nombre));

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
	}



	public class Prefs
	{
		public long AlimentoInicial = 100;
	}
}
