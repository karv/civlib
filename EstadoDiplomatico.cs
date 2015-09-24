namespace Civ
{
	/// <summary>
	/// Estado diplomatico entre dos cavilizaciones.
	/// </summary>
	public class EstadoDiplomatico
	{
		/// <summary>
		/// Devuelve o establece si se le permite a una Civilización atacar a otra.
		/// </summary>
		public bool PermiteAtacar = true;
		/// <summary>
		/// Devuelve o establece su una Civilización puede hacer diplomacia a voluntad con otra.
		/// </summary>
		public bool PuedeHacerDiplomacia;
	}
}