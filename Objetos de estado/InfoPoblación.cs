namespace Civ
{
	/// <summary>
	/// Mantiene información sobre la distribución por edades de una población
	/// </summary>
	public struct InfoPoblación
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

		public InfoPoblación (ulong pre, ulong prod, ulong post)
		{
			PreProductiva = pre;
			Productiva = prod;
			PostProductiva = post;
		}
	}
}