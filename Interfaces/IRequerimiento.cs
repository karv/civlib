using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Civ
{
	public interface IRequerimiento<T>
	{
		/// <summary>
		/// Si una ciudad satisface este requerimiento.
		/// </summary>
		/// <returns><c>true</c>, Si la ciudad <c>C</c> lo satisface , <c>false</c> si no.</returns>
		/// <param name="C">La ciudad que intenta satisfacer este requerimiento.</param>
		bool LoSatisface(T C);
	}

	[DataContract(Name = "Requerimiento")]
	public class Requerimiento : CivLibrary.Debug.IPlainSerializable
	{
		[DataMember]
		public List<Ciencia> Ciencias = new List<Ciencia>();
		[DataMember]
		public List<EdificioRAW> Edificios = new List<EdificioRAW>();
		[DataMember]
		public List<Propiedad> Propiedades = new List<Propiedad>();

		/// <summary>
		/// Junta todos los requeriemintos en una lista de IRequerimientos.
		/// </summary>
		/// <returns></returns>
		public List<IRequerimiento<ICiudad>> Requiere()
		{
			List<IRequerimiento<ICiudad>> ret = new List<IRequerimiento<ICiudad>>();
			foreach (Ciencia x in Ciencias)
			{
				ret.Add(x);
			}
			foreach (EdificioRAW x in Edificios)
			{
				ret.Add(x);
			}

			return ret;
		}

		string CivLibrary.Debug.IPlainSerializable.PlainSerialize(int tabs)
		{
			string tab = "";
			string ret = "";
			for (int i = 0; i < tabs; i++)
			{
				tab += "\t";
			}

			foreach (CivLibrary.Debug.IPlainSerializable x in Requiere())
			{
				ret += x.PlainSerialize(tabs);
			}

			return ret;
		}
	}
}