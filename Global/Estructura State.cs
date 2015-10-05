using System.Collections.Generic;
using Civ;
using Graficas;

namespace Global
{
	/// <summary>
	/// Representa el estado de un juego.
	/// </summary>
	public class GameState
	{
		/// <summary>
		/// La topología del mundo.
		/// </summary>
		public Grafo<Terreno> Topologia;

		public Graficas.Continuo.Continuo<Terreno> Mapa;
		List<ICivilizacion> _Civs = new List<ICivilizacion>();
		List<DropStack> _Drops = new List<DropStack>();

		public GameState()
		{
			Topologia = new Grafo<Terreno>();
			Topologia.EsSimetrico = true;
		}

		/// <summary>
		/// Lista de civilizaciones en el juego. (Incluyendo las muertas)        
		/// </summary>        
		public List<ICivilizacion> Civs
		{
			get { return _Civs; }
		}

		/// <summary>
		/// Devuelve una lista de civilizaciones vivas (eq. en el mapa)
		/// </summary>
		public List<ICivilizacion> CivsVivas()
		{
			var ret = new List<ICivilizacion>();
			foreach (var x in Topologia.Nodos)
			{
				ICivilizacion C = x.CiudadConstruida?.CivDueno;
				if (C != null && !ret.Contains(C))
					ret.Add(C);
			}
			return ret;
		}

		public List<DropStack> Drops { get { return _Drops; } }

		/// <summary>
		/// Obtiene la lista de <c>Terreno</c>s en el juego.
		/// </summary>
		/// <returns>Devuelve una lista enumerando a los <c>Terrenos</c>.</returns>
		public List<Terreno> ObtenerListaTerrenos()
		{
			var ret = new List<Terreno>();

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
		/// <param name="edif">Una clase de edificio.</param>
		/// <returns></returns>
		public int CuentaEdificios(EdificioRAW edif)
		{
			int ret = 0;
			foreach (var x in Civs)
			{
				ret += x.CuentaEdificios(edif);
			}
			return ret;
		}

		/// <summary>
		/// Devuelve una lista de ciudades existentes.
		/// </summary>
		/// <returns>The ciudades.</returns>
		public List<ICiudad> CiudadesExistentes()
		{
			var ret = new List<ICiudad>();
			foreach (var civil in Civs)
			{
				foreach (var c in civil.Ciudades)
				{
					ret.Add(c);
				}
			}
			return ret;
		}

		/// <summary>
		/// Devuelve una lista de armadas.
		/// </summary>
		public List<Armada> ArmadasExistentes()
		{
			var ret = new List<Armada>();
			foreach (var civ in _Civs)
			{
				foreach (var a in civ.Armadas)
				{
					ret.Add(a);
				}
			}
			return ret;
		}


		#region Estad¨ªstico

		/// <summary>
		/// Devuelve la puntuaci¨®n total del juego
		/// </summary>
		/// <returns>The puntuacion.</returns>
		public float SumaPuntuacion()
		{
			float ret = 0;
			foreach (IPuntuado x in CivsVivas ())
			{
				ret += x.Puntuacion;
			}
			return ret;
		}

		#endregion
	}
}