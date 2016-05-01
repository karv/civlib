using System;
using Civ.RAW;

namespace Civ.Ciencias
{
	/// <summary>
	/// Lista de recursos que son requerimientos de una ciencia
	/// </summary>
	[Serializable]
	public class RequiereCiencia: ListasExtra.ListaPeso<Recurso>
	{
		public RequiereCiencia ()
			: base (new System.Collections.Generic.Dictionary<Recurso, float> ())
		{
		}
	}
}