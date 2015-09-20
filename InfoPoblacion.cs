using System;

namespace Civ
{
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

		public InfoPoblacion(ulong Pre, ulong Prod, ulong Post)
		{
			PreProductiva = Pre;
			Productiva = Prod;
			PostProductiva = Post;
		}
	}
}

