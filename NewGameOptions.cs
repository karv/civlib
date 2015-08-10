namespace Civ.Options
{
	/// <summary>
	/// Representa opciones de contruicción de un nuevo juego.
	/// </summary>
	public class NewGameOptions
	{
		// Opciones cívicas
		public long AlimentoInicial = 100;
		public int poblacionInicial = 10;

		// Topología
		public int numTerrenos = 40;
		public int numCivs = 4;

		/// <summary>
		/// Probabilidad de que dos territorios sean vecinos.
		/// </summary>
		public float compacidad = 0.15f;

		// Mínima y máxima distancia entre nodos.
		public float minDistNodos = 0.3f;
		public float maxDistNodos = 2f;

	}
}

