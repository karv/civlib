namespace Civ
{
	/// <summary>
	/// Mantiene información sobre la distribución por edades de una población
	/// </summary>
	public struct InfoPoblacion
	{
		public readonly ulong PreProductiva;
		public readonly ulong Productiva;
		public readonly ulong PostProductiva;

		public ulong Total
		{
			get
			{
				return  PreProductiva + Productiva + PostProductiva;
			}
		}

		public InfoPoblacion (ulong pre, ulong prod, ulong post)
		{
			PreProductiva = pre;
			Productiva = prod;
			PostProductiva = post;
		}
	}
}