using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Civ
{
	public interface IRequerimiento
	{
		/// <summary>
		/// Un método que a cada IRequerimiento le (debe) asocia un único string
		/// </summary>
		string ObtenerId();

		/// <summary>
		/// Si una ciudad satisface este requerimiento.
		/// </summary>
		/// <returns><c>true</c>, Si la ciudad <c>C</c> lo satisface , <c>false</c> si no.</returns>
		/// <param name="C">La ciudad que intenta satisfacer este requerimiento.</param>
		bool LoSatisface(Ciudad C);
	}

	[DataContract(Name = "Requerimiento")]
	public class Requerimiento      // Para juntar los Reqs en una clase. ¿Olvidarse de IRequerimiento?
	{
		[DataMember]
		public List<Ciencia> Ciencias = new List<Ciencia>();
		[DataMember]
		public List<EdificioRAW> Edificios = new List<EdificioRAW>();

		/// <summary>
		/// Junta todos los requeriemintos en una lista de IRequerimientos.
		/// </summary>
		/// <returns></returns>
		public System.Collections.Generic.List<IRequerimiento> Requiere()
		{
			List<IRequerimiento> ret = new List<IRequerimiento>();
			foreach (Ciencia x in Ciencias) { ret.Add(x); }
			foreach (EdificioRAW x in Edificios) { ret.Add(x); }

			return ret;
		}


	}
}

