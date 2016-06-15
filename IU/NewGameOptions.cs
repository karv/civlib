namespace Civ.Options
{
	/// <summary>
	/// Representa opciones de contruicción de un nuevo juego.
	/// </summary>
	public class NewGameOptions
	{
		// Opciones cívicas
		/// <summary>
		/// Alimento inicial por ciudad
		/// </summary>
		#if DEBUG
		public long AlimentoInicial = 1000;
		#else
		public long AlimentoInicial = 100;
		#endif
		/// <summary>
		/// Población inicial
		/// </summary>
		public int PoblacionInicial = 10;

		/// <summary>
		/// Peso militar máxipo al inicio
		/// </summary>
		public float MaxPesoInicial = 10;

		// Topología
		/// <summary>
		/// Número de terrenos
		/// </summary>
		public int NumTerrenos = 50;
		/// <summary>
		/// Número de civilizaciones
		/// </summary>
		public int NumCivs = 1;

		/// <summary>
		/// Probabilidad de que dos territorios sean vecinos.
		/// </summary>
		public float Compacidad = 0.15f;

		// Mínima y máxima distancia entre nodos.
		/// <summary>
		/// Mínima distancia entre terrenos
		/// </summary>
		public float MinDistNodos = 0.3f;
		/// <summary>
		/// Máxima distancia entre terrenos
		/// </summary>
		public float MaxDistNodos = 2f;
	}
}
