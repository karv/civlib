using System;

namespace Civ
{
	/// <summary>
	/// Estado diplomatico entre dos cavilizaciones.
	/// </summary>
	public class EstadoDiplomático
	{
		bool _permiteAtacar = true;
		bool _permitePaso = true;

		/// <summary>
		/// Devuelve o establece si se le permite a una Civilización atacar a otra.
		/// </summary>
		public bool PermiteAtacar
		{
			get
			{
				return _permiteAtacar;
			}
			set
			{
				_permiteAtacar = value;
				AlCambiarPermisoAtacar?.Invoke ();
			}
		}

		public bool PermitePaso
		{
			get
			{
				return _permitePaso;
			}
			set
			{
				_permitePaso = value;
				AlCambiarPermisoPaso?.Invoke ();
			}
		}

		/// <summary>
		/// Devuelve o establece su una Civilización puede hacer diplomacia a voluntad con otra.
		/// </summary>
		public bool PuedeHacerDiplomacia;

		public event Action AlCambiarPermisoAtacar;
		public event Action AlCambiarPermisoPaso;
	}
}