using System;

namespace Civ
{
	public class ControlDiplomacia : C5.HashDictionary<ICivilización, EstadoDiplomático>, IDiplomacia
	{
		/// <summary>
		/// Devuelve o establece si una civilización permite atacar a otras con las que no tiene diplomacia.
		/// </summary>
		public bool PermiteAtacarDesconocidos { get; set; }

		public bool PermiteAtacar (Armada arm)
		{
			EstadoDiplomático dip;
			var civ = arm.CivDueño;
			return Find (ref civ, out dip) ? dip.PermiteAtacar : PermiteAtacarDesconocidos;
		}

		//TEST
		public event Action AlCambiarDiplomacia;

		public ControlDiplomacia ()
		{
			// Analysis disable PossibleAssignmentToReadonlyField
			ItemsAdded += delegate(object sender,
			                       C5.ItemCountEventArgs<C5.KeyValuePair<ICivilización, EstadoDiplomático>> eventArgs)
			{
				AlCambiarDiplomacia?.Invoke ();
				eventArgs.Item.Value.AlCambiarPermisoAtacar += InvocarCambio;
			};

			ItemsRemoved += (sender,
			                 eventArgs) => eventArgs.Item.Value.AlCambiarPermisoAtacar -= InvocarCambio;
			// Analysis restore PossibleAssignmentToReadonlyField
		}

		void InvocarCambio ()
		{
			AlCambiarDiplomacia?.Invoke ();
		}
	}
}