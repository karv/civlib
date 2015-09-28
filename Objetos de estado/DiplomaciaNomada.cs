namespace Civ
{
	/// <summary>
	/// Clase de diplomacia para civilizaciones nómadas
	/// </summary>
	public class DiplomaciaNomada:IDiplomacia
	{
		public bool PermiteAtacar(Armada arm)
		{
			return true;
		}
	}
}