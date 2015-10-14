namespace Civ.Data
{
	/// <summary>
	/// Lista de recursos que son requerimientos de una ciencia
	/// </summary>
	public class RequiereCiencia: ListasExtra.ListaPeso<Recurso>
	{
		public RequiereCiencia ()
			: base (new System.Collections.Generic.Dictionary<Recurso, float> ())
		{
		}
	}
}