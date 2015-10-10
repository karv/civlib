using System.Runtime.Serialization;
using System.Collections.Generic;
using Civ.Data;

namespace Civ
{
	public interface IRequerimiento<T>
	{
		/// <summary>
		/// Si una ciudad satisface este requerimiento.
		/// </summary>
		/// <returns><c>true</c>, Si la ciudad <c>C</c> lo satisface , <c>false</c> si no.</returns>
		/// <param name="objeto">El objeto que intenta satisfacer este requerimiento.</param>
		bool LoSatisface (T objeto);
	}

	[DataContract (Name = "Requerimiento")]
	public class Requerimiento : Civ.Debug.IPlainSerializable
	{
		[DataMember]
		public ICollection<Ciencia> Ciencias = new C5.HashSet<Ciencia> ();
		[DataMember]
		public ICollection<EdificioRAW> Edificios = new C5.HashSet<EdificioRAW> ();
		[DataMember]
		public ICollection<Propiedad> Propiedades = new C5.HashSet<Propiedad> ();

		/// <summary>
		/// Junta todos los requeriemintos en una lista de IRequerimientos.
		/// </summary>
		/// <returns></returns>
		public List<IRequerimiento<ICiudad>> Requiere ()
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