namespace Civ.Options
{
	/// <summary>
	/// Representa opciones de contruicción de un nuevo juego.
	/// </summary>
	public class NewGameOptions
	{
		// Opciones cívicas
		public long AlimentoInicial = 100;
		public int PoblacionInicial = 10;

		// Topología
		public int NumTerrenos = 10;
		public int NumCivs = 4;

		/// <summary>
		/// Probabilidad de que dos territorios sean vecinos.
		/// </summary>
		public float Compacidad = 0.15f;

		// Mínima y máxima distancia entre nodos.
		public float MinDistNodos = 0.3f;
		public float MaxDistNodos = 2f;

	}
}
