using System;
using System.Collections.Generic;

namespace Civ
{
	[Serializable]
	/// <summary>
	/// Mi estado diplomático con respecto a otra ICIvilización
	/// </summary>
	public class ControlDiplomacia : Dictionary<ICivilización, EstadoDiplomático>, IDiplomacia
	{
		/// <summary>
		/// Devuelve o establece si una civilización permite atacar a otras con las que no tiene diplomacia.
		/// </summary>
		public bool PermiteAtacarDesconocidos { get; set; }

		public bool PermiteAtacar (Armada arm)
		{
			EstadoDiplomático dip;
			var civ = arm.CivDueño;
			return TryGetValue (civ, out dip) ? dip.PermiteAtacar : PermiteAtacarDesconocidos;
		}

		public bool PermitePaso (Armada arm)
		{
			EstadoDiplomático dip;
			var civ = arm.CivDueño;
			return TryGetValue (civ, out dip) && dip.PermitePaso;
		}

		public event Action AlCambiarDiplomacia;

		public ControlDiplomacia ()
		{
			/*
			ItemsAdded += delegate(object sender,
			                       C5.ItemCountEventArgs<C5.KeyValuePair<ICivilización, EstadoDiplomático>> eventArgs)
			{
				AlCambiarDiplomacia?.Invoke ();
				eventArgs.Item.Value.AlCambiarPermisoAtacar += InvocarCambio;
			};

			ItemsRemoved += (sender,			                 eventArgs) => eventArgs.Item.Value.AlCambiarPermisoAtacar -= InvocarCambio;
			*/
		}

		void InvocarCambio ()
		{
			AlCambiarDiplomacia?.Invoke ();
		}
	}
}