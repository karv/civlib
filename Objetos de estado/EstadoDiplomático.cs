using System;

namespace Civ
{
	/// <summary>
	/// Estado diplomatico entre dos cavilizaciones.
	/// </summary>
	public class EstadoDiplomático
	{
		bool _PermiteAtacar = true;

		/// <summary>
		/// Devuelve o establece si se le permite a una Civilización atacar a otra.
		/// </summary>
		public bool PermiteAtacar
		{
			get
			{
				return _PermiteAtacar;
			}
			set
			{
				_PermiteAtacar = value;
				AlCambiarPermisoAtacar?.Invoke ();
			}
		}

		/// <summary>
		/// Devuelve o establece su una Civilización puede hacer diplomacia a voluntad con otra.
		/// </summary>
		public bool PuedeHacerDiplomacia;

		public event Action AlCambiarPermisoAtacar;
	}
}