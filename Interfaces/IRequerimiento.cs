using System;

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
		bool LoSatisface (Ciudad C);
	}
}

