using System;
using System.Collections.Generic;
using Basic;

namespace Civ
{
	public partial class Civilizacion
	{
		/// <summary>
		/// Nombre de la <see cref="Civ.Civilización"/>.
		/// </summary>
		public string Nombre;
		// **** Economía
		/// <summary>
		/// Devuelve la cantidad que existe en la civilización de un cierto recurso.
		/// </summary>
		/// <returns>Devuelve la suma de la cantidad que existe de algún recurso sobre cada ciudad.</returns>
		/// <param name="R">Recurso que se quiere contar</param>
		public float ObtenerGlobalRecurso(Recurso R)
		{
			float ret = 0;
			foreach (var x in Ciudades)
			{
				ret += x.Almacén[R];
			}
			return ret;
		}

		List<Armada> _Armadas = new List<Armada>();

		/// <summary>
		/// Devuelve la lista de armadas de la civ.
		/// </summary>
		/// <value>la list que enlista a las larmadas de esta civ.</value>
		public List<Armada> Armadas
		{
			get
			{
				return _Armadas;
			}
		}

		Dictionary<Civilizacion, EstadoDiplomatico> _Diplomacia = new Dictionary<Civilizacion, EstadoDiplomatico>();

		/// <summary>
		/// Devuelve el estado diplomático de esta Civilización.
		/// </summary>
		/// <value>The _ diplomacia.</value>
		public Dictionary<Civilizacion, EstadoDiplomatico> Diplomacia
		{
			get
			{
				return _Diplomacia;
			}
		}
	}
}