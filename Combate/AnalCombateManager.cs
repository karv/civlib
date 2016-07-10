using System.Collections.Generic;

namespace Civ.Combate
{
	/// <summary>
	/// Se engarga de unir y organizar los <see cref="Civ.Combate.IAnálisisCombate"/> generados surante una pelea.
	/// </summary>
	public class AnalCombateManager
	{
		readonly HashSet<IAnálisisCombate> combatesData = new HashSet<IAnálisisCombate> ();

		/// <summary>
		/// Une, o agrega si es necesario, un análisis de combate a la colección.
		/// </summary>
		public void AddOrMerge (IAnálisisCombate combate)
		{
			foreach (var x in combatesData)
			{
				if (x.EsUnibleCon (combate))
				{
					x.UnirCon (combate);
					return;
				}
			}
		}
	}
}