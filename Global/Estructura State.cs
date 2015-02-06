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
		public Grafica<Terreno> Topologia;
		private List<Civilizacion> _Civs = new List<Civilizacion>();

		/// <summary>
		/// Lista de civilizaciones en el juego. (Incluyendo las muertas)        
		/// </summary>        
		public List<Civilizacion> Civs      // Las vivas bien las podría obtener accesando la topología.
		{
			get { return _Civs; }
		}

		public g_State()
		{
			Topologia = new Grafica<Terreno>();
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

		/// <summary>
		/// Devuelve el número de edificios de un tipo determinado en el mundo
		/// </summary>
		/// <param name="Edif">Una clase de edificio.</param>
		/// <returns></returns>
		public int CuentaEdificios(EdificioRAW Edif)
		{
			int ret = 0;
			foreach (var x in Civs)
			{
				ret += x.CuentaEdificios(Edif);
			}
			return ret;
		}
	}
}

