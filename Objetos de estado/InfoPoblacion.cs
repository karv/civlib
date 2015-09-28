namespace Civ
{
	/// <summary>
	/// Mantiene información sobre la distribución por edades de una población
	/// </summary>
	public struct InfoPoblacion
	{
		public ulong PreProductiva;
		public ulong Productiva;
		public ulong PostProductiva;

		public ulong Total
		{
			get
			{
				return  PreProductiva + Productiva + PostProductiva;
			}
		}

		public InfoPoblacion(ulong pre, ulong prod, ulong post)
		{
			PreProductiva = pre;
			Productiva = prod;
			PostProductiva = post;
		}
	}
}

