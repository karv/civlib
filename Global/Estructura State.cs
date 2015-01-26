using System;
using System.Collections.Generic;
using Civ;
using Graficas;

namespace Global
{
	/// <summary>
	/// Representa el estado de un juego.
	/// </summary>
	public class g_State
	{
        /// <summary>
        /// La topología del mundo.
        /// </summary>
        public Grafica<IPosicion> Topologia;


        private List<Civilizacion> _Civs;     

        /// <summary>
        /// Lista de civilizaciones en el juego. (Incluyendo las muertas)        
        /// </summary>        
        public List<Civilizacion> Civs      // Las vivas bien las podría obtener accesando la topología.
        {
            get { return _Civs; }
        }

        public g_State()
        {
            Topologia = new Grafica<IPosicion>();
            Topologia.EsSimetrico = true;
        }

        /// <summary>
        /// Obtiene la lista de <c>Terreno</c>s en el juego.
        /// </summary>
        /// <returns>Devuelve una lista enumerando a los <c>Terrenos</c>.</returns>
        public List<Terreno> ObtenerListaTerrenos()
        {
            List<Terreno> ret = new List<Terreno>();

            foreach (var x in Topologia.Nodos)
	        {
                if (x is Terreno)
                    ret.Add((Terreno)x);
	        }

            return ret;
        }

        /// <summary>
        /// Devuelve una lista de todos los terrenos desocupados en el juego.
        /// </summary>
        /// <returns></returns>
        public List<Terreno> ObtenerListaTerrenosLibres()
        {
            return ObtenerListaTerrenos().FindAll(x => x.CiudadConstruida == null);
        }
	}
}

