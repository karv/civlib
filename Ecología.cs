using System.Collections.Generic;

namespace Civ
{
	/// <summary>
	/// Representa la ecología del terreno.
	/// </summary>
	public class Ecología
	{
		public Dictionary<Recurso, RecursoEstado> RecursoEcologico = new Dictionary<Recurso, RecursoEstado> ();
	}

	public struct RecursoEstado
	{
		public float Cant;
		public float Max;
		public float Crec;
	}
}