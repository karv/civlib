namespace Civ.Options
{
	/// <summary>
	/// Representa opciones de contruicción de un nuevo juego.
	/// </summary>
	public class NewGameOptions
	{
		// Opciones cívicas
		#if DEBUG
		public long AlimentoInicial = 1000;
		#else
		public long AlimentoInicial = 100;
		#endif
		public int PoblacionInicial = 10;

		public float MaxPesoInicial = 10;

		// Topología
		public int NumTerrenos = 50;
		public int NumCivs = 1;

		/// <summary>
		/// Probabilidad de que dos territorios sean vecinos.
		/// </summary>
		public float Compacidad = 0.15f;

		// Mínima y máxima distancia entre nodos.
		public float MinDistNodos = 0.3f;
		public float MaxDistNodos = 2f;

	}
}
