using System.Collections.Generic;
using Civ;
using Graficas;
using Civ.Data;

namespace Global
{
	/// <summary>
	/// Representa el estado de un juego.
	/// </summary>
	public class GameState : IPuntuado
	{
		/// <summary>
		/// La topología del mundo.
		/// </summary>
		public Grafo<Terreno> Topología;

		/// <summary>
		/// Conjunto de rutas óptimas para Topología.
		/// </summary>
		public ConjuntoRutasÓptimas<Terreno> Rutas;

		/// <summary>
		/// Mapa continuo del mundo
		/// </summary>
		public Graficas.Continuo.Continuo<Terreno> Mapa;

		/// <summary>
		/// Lista de civilizaciones en el juego. (Incluyendo las muertas)        
		/// </summary>        
		public IList<ICivilización> Civs { get; }

		/// <summary>
		/// Objetos/recursos en el mapa
		/// </summary>
		/// <value>The drops.</value>
		public ICollection<DropStack> Drops { get; }

		/// <summary>
		/// Initializes a new instance of the class
		/// </summary>
		public GameState ()
		{
			Civs = new List<ICivilización> ();
			Drops = new List<DropStack> ();
			Topología = new Grafo<Terreno> ();
			Topología.EsSimétrico = true;
		}

		/// <summary>
		/// Devuelve una colección de civilizaciones vivas (eq. en el mapa)
		/// </summary>
		public ICollection<ICivilización> CivsVivas ()
		{
			var ret = new List<ICivilización> ();
			foreach (var x in Topología.Nodos)
			{
				ICivilización C = x.CiudadConstruida?.CivDueño;
				if (C != null && !ret.Contains (C))
					ret.Add (C);
			}
			return ret;
		}

		/// <summary>
		/// Obtiene la lista de <c>Terreno</c>s en el juego.
		/// </summary>
		/// <returns>Devuelve una colección enumerando a los <c>Terrenos</c>.</returns>
		public ICollection<Terreno> Terrenos ()
		{
			var ret = new List<Terreno> ();

			foreach (var x in Topología.Nodos)
			{
				ret.Add (x);
			}

			return ret;
		}

		/// <summary>
		/// Devuelve una lista de todos los terrenos desocupados en el juego.
		/// </summary>
		/// <returns></returns>
		public ICollection<Terreno> TerrenosLibres ()
		{
			var ret = new C5.ArrayList<Terreno> ();
			foreach (var x in Terrenos())
			{
				if (x.CiudadConstruida == null)
					ret.Add (x);
			}
			return ret;
		}

		/// <summary>
		/// Devuelve el número de edificios de un tipo determinado en el mundo
		/// </summary>
		/// <param name="edif">Una clase de edificio.</param>
		/// <returns></returns>
		public int CuentaEdificios (EdificioRAW edif)
		{
			int ret = 0;
			foreach (var x in Civs)
			{
				ret += x.CuentaEdificios (edif);
			}
			return ret;
		}

		/// <summary>
		/// Devuelve una lista de ciudades existentes.
		/// </summary>
		/// <returns>The ciudades.</returns>
		public ICollection<ICiudad> CiudadesExistentes ()
		{
			var ret = new List<ICiudad> ();
			foreach (var civil in Civs)
			{
				foreach (var c in civil.Ciudades)
				{
					ret.Add (c);
				}
			}
			return ret;
		}

		/// <summary>
		/// Devuelve una lista de armadas.
		/// </summary>
		public ICollection<Armada> ArmadasExistentes ()
		{
			var ret = new List<Armada> ();
			foreach (var civ in Civs)
			{
				foreach (var a in civ.Armadas)
				{
					ret.Add (a);
				}
			}
			return ret;
		}


		#region Estadísico

		/// <summary>
		/// Devuelve la puntuación total del juego
		/// </summary>
		/// <returns>The puntuacion.</returns>
		public float SumaPuntuacion ()
		{
			float ret = 0;
			foreach (IPuntuado x in CivsVivas ())
			{
				ret += x.Puntuación;
			}
			return ret;
		}

		float IPuntuado.Puntuación
		{
			get
			{
				return SumaPuntuacion ();
			}
		}

		#endregion
	}
}
