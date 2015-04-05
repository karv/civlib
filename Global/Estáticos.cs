using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Global
{
	/// <summary>
	/// Los objetos globales.
	/// </summary>	
	public static class g_
	{

		[DataMember(Name = "Data")]
		public static g_Data Data = new g_Data();
		public static g_State State = new g_State();
		private const string archivo = "Data.xml";

		public static void Tick(float t = 1)
		{
			foreach (var Civ in State.Civs)
			{
				Civ.doTick(t);
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

		/// <summary>
		/// Inicializa el g_State, a partir de el g_Data.
		/// Usarse cuando se quiera iniciar un juego.
		/// </summary>
		public static void InicializarJuego()
		{
			Random r = new Random();
			State = new g_State();

			// Hacer la topología
			List<Civ.Terreno> Terrenos = new List<Civ.Terreno>();
			Civ.Terreno T;
			Civ.Ecosistema Eco;
			Civ.Civilizacion C;
			Civ.Ciudad Cd;

			for (int i = 0; i < numTerrenosIniciales; i++)
			{
				Eco = Data.Ecosistemas[r.Next(Data.Ecosistemas.Count)]; // Selecciono un ecosistema al azar.
				T = new Civ.Terreno(Eco);                               // Le asocio un terreno consistente con el ecosistema.
				Terrenos.Add(T);                                        // Lo enlisto.
			}
			State.Topologia = Graficas.Grafica<Civ.Terreno>.GeneraGraficaAleatoria(Terrenos);

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
				C.Nombre = "Necesito un generador de nombres.";     //TODO: Un generador de nombres de civs.
				List<Civ.Terreno> Terrs = State.ObtenerListaTerrenosLibres();
				T = Terrs[r.Next(Terrs.Count)];         // Éste es un elemento aleatorio de un Terreno libre.

				Cd = new Civ.Ciudad("Ciudad inicial.", C, T);
				C.addCiudad(Cd);

				State.Civs.Add(C);
			}
		}
		// constantes
		const int numTerrenosIniciales = 40;
		const int numCivsIniciales = 4;
	}
}
