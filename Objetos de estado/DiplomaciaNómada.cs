namespace Civ
{
	/// <summary>
	/// Clase de diplomacia para civilizaciones nómadas
	/// </summary>
	public class DiplomaciaNómada:IDiplomacia
	{
		public bool PermiteAtacar (Armada arm)
		{
			return true;
		}
	}
}