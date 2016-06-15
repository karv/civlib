using System;
using System.Collections.Generic;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Mi estado diplomático con respecto a otra ICIvilización
	/// </summary>
	[Serializable]
	public class ControlDiplomacia : Dictionary<ICivilización, EstadoDiplomático>, IDiplomacia
	{
		#region Militar y territorial

		/// <summary>
		/// Devuelve o establece si una civilización permite atacar a otras con las que no tiene diplomacia.
		/// </summary>
		public bool PermiteAtacarDesconocidos { get; set; }

		/// <summary>
		/// Si se le permite atacar a cierta armada.
		/// </summary>
		/// <returns><c>true</c>, if atacar was permited, <c>false</c> otherwise.</returns>
		/// <param name="arm">Arm.</param>
		public bool PermiteAtacar (Armada arm)
		{
			EstadoDiplomático dip;
			var civ = arm.CivDueño;
			return TryGetValue (civ, out dip) ? dip.PermiteAtacar : PermiteAtacarDesconocidos;
		}

		/// <summary>
		/// Si se le permite el paso a una armada dada
		/// </summary>
		/// <returns><c>true</c>, si puede pasar sin abrir fuego, <c>false</c> otherwise.</returns>
		/// <param name="arm">Armada</param>
		public bool PermitePaso (Armada arm)
		{
			EstadoDiplomático dip;
			var civ = arm.CivDueño;
			return TryGetValue (civ, out dip) && dip.PermitePaso;
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre al cambiar las especificaciones diplomáticas
		/// </summary>
		public event Action AlCambiarDiplomacia;

		#endregion

		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.ObjetosEstado.ControlDiplomacia"/> class.
		/// </summary>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.ObjetosEstado.ControlDiplomacia"/> class.
		/// Para serializar
		/// </summary>
		/// <param name="info">Info.</param>
		/// <param name="context">Context.</param>
		protected ControlDiplomacia (System.Runtime.Serialization.SerializationInfo info,
		                             System.Runtime.Serialization.StreamingContext context)
			: base (info, context)
		{
		}

		#endregion

		void InvocarCambio ()
		{
			AlCambiarDiplomacia?.Invoke ();
		}
	}
}