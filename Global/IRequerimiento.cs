using System.Collections.Generic;
using Civ.RAW;
using Civ.Ciencias;
using System;
using Civ.ObjetosEstado;

namespace Civ.Global
{
	/// <summary>
	/// Representa un objeto que se puede requerir.
	/// </summary>
	public interface IRequerimiento<T>
	{
		/// <summary>
		/// Si una ciudad satisface este requerimiento.
		/// </summary>
		/// <returns><c>true</c>, Si la ciudad <c>C</c> lo satisface , <c>false</c> si no.</returns>
		/// <param name="objeto">El objeto que intenta satisfacer este requerimiento.</param>
		bool LoSatisface (T objeto);
	}

	/// <summary>
	/// Requerimiento.
	/// </summary>
	[Serializable]
	public class Requerimiento
	{
		/// <summary>
		/// Requerimiento de ciencias
		/// </summary>
		public ICollection<Ciencia> Ciencias = new HashSet<Ciencia> ();
		/// <summary>
		/// Requerimiento de edificios
		/// </summary>
		public ICollection<EdificioRAW> Edificios = new HashSet<EdificioRAW> ();
		/// <summary>
		/// Porpiedades requeridas
		/// </summary>
		public ICollection<Propiedad> Propiedades = new HashSet<Propiedad> ();

		/// <summary>
		/// Junta todos los requeriemintos en una lista de IRequerimientos.
		/// </summary>
		/// <returns></returns>
		public IList<IRequerimiento<ICiudad>> Requiere ()
		{
			var ret = new List<IRequerimiento<ICiudad>> ();
			foreach (Ciencia x in Ciencias)
			{
				ret.Add (x);
			}
			foreach (EdificioRAW x in Edificios)
			{
				ret.Add (x);
			}

			return ret;
		}

		/// <summary>
		/// Agrega un requerimiento, inteligentemente selecionando en cuál lista debe agregarse.
		/// </summary>
		/// <param name="x">Objeto a agregar</param>
		public void Add (object x)
		{
			if (x.GetType ().IsAssignableFrom (typeof (Ciencia)))
				Ciencias.Add (x as Ciencia);
			else if (x.GetType ().IsAssignableFrom (typeof (EdificioRAW)))
				Edificios.Add (x as EdificioRAW);
			else if (x.GetType ().IsAssignableFrom (typeof (Propiedad)))
				Propiedades.Add (x as Propiedad);
			else
				throw new Exception ();
		}
	}
}