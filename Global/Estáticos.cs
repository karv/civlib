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
        [DataMember (Name = "Data")]
		public static g_Data Data = new g_Data();
		public static g_State State = new g_State();

		private const string archivo = "Data.xml";

		/// <summary>
		/// Carga del archivo predeterminado.
		/// </summary>
		public static void CargaData ()
		{
			Data = Store.Store<g_Data>.Deserialize (archivo);
		}

		public static void GuardaData() 
		{
			Store.Store<g_Data>.Serialize (archivo, Data);
		}

		public static void GuardaData(string f)
		{
			Store.Store<g_Data>.Serialize (f, Data);
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
            List<Civ.IPosicion> Terrenos = new List<Civ.IPosicion>();
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
            State.Topologia = Graficas.Grafica<Civ.IPosicion>.GeneraGráficaAleatoria(Terrenos);

            // Asignar una ciudad de cada civilización en terrenos vacíos y distintos lugares.
            for (int i = 0; i < numCivsIniciales; i++)
            {
                C = new Civ.Civilizacion();
                C.Nombre = "Necesito un generador de nombres.";     //TODO: Un generador de nombres de civs.
                List<Civ.Terreno> Terrs = State.ObtenerListaTerrenosLibres();
                T = Terrs[r.Next(Terrs.Count)];         // Éste es un elemento aleatorio de un Terreno libre.

                Cd = new Civ.Ciudad("Ciudad inicial.", C, T);
                C.addCiudad(Cd);
            }
        }


        // constantes
        const int numTerrenosIniciales = 40;
        const int numCivsIniciales = 4;
	}
}
