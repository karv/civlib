using System.Collections.Generic;
using Civ.Data;
using System;

namespace Civ
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

	public class Requerimiento : Civ.Debug.IPlainSerializable
	{
		public ICollection<Ciencia> Ciencias = new C5.HashSet<Ciencia> ();
		public ICollection<EdificioRAW> Edificios = new C5.HashSet<EdificioRAW> ();
		public ICollection<Propiedad> Propiedades = new C5.HashSet<Propiedad> ();

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

		string Civ.Debug.IPlainSerializable.PlainSerialize (int tabs)
		{
			string tab = "";
			string ret = "";
			for (int i = 0; i < tabs; i++)
			{
				tab += "\t";
			}

			foreach (Civ.Debug.IPlainSerializable x in Requiere())
			{
				ret += x.PlainSerialize (tabs);
			}

			return ret;
		}
	}
}