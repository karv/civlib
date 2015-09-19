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

		public Graficas.Continuo.Continuo<Terreno> Mapa;
		List<Civilizacion> _Civs = new List<Civilizacion>();

		public g_State()
		{
			Topologia = new Grafica<Terreno>();
			Topologia.EsSimetrico = true;
		}

		/// <summary>
		/// Lista de civilizaciones en el juego. (Incluyendo las muertas)        
		/// </summary>        
		public List<Civilizacion> Civs      // Las vivas bien las podría obtener accesando la topología.
		{
			get { return _Civs; }
		}

		/// <summary>
		/// Devuelve la lista de civilizaciones vivas (eq. en el mapa)
		/// </summary>
		public List<Civilizacion> CivsVivas()
		{
			List<Civilizacion> ret = new List<Civilizacion>();
			foreach (var x in Topologia.Nodos)
			{
				Civilizacion C = x.CiudadConstruida.CivDueno;
				if (!ret.Contains(C))
					ret.Add(C);
			}
			return ret;
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
				ret.Add(x);
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

		/// <summary>
		/// Devuelve una lista de ciudades existentes.
		/// </summary>
		/// <returns>The ciudades.</returns>
		public IEnumerable<Civ.Ciudad> getCiudades()
		{
			List<Civ.Ciudad> ret = new List<Civ.Ciudad>();
			foreach (var civil in Civs)
			{
				foreach (var c in civil.getCiudades)
				{
					ret.Add(c);
				}
			}
			return ret;
		}

		/// <summary>
		/// Devuelve una lista de armadas.
		/// </summary>
		public IList<Armada> getArmadas()
		{
			List<Armada> ret = new List<Armada>();
			foreach (var civ in _Civs)
			{
				foreach (var a in civ.Armadas)
				{
					ret.Add(a);
				}
			}
			return ret;
		}
	}
}