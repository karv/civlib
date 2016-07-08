using System;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Estado diplomatico entre dos cavilizaciones.
	/// </summary>
	[Serializable]
	public class EstadoDiplomático
	{
		#region Permisos militares y territoriales

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
				AlCambiarPermisoAtacar?.Invoke (this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Permite esta unidad el paso a otra
		/// </summary>
		/// <value><c>true</c> isi permite paso; si no, <c>false</c>.</value>
		public bool PermitePaso
		{
			get
			{
				return _permitePaso;
			}
			set
			{
				_permitePaso = value;
				AlCambiarPermisoPaso?.Invoke (this, EventArgs.Empty);
			}
		}

		#endregion

		#region General

		/// <summary>
		/// Devuelve o establece su una Civilización puede hacer diplomacia a voluntad con otra.
		/// </summary>
		public bool PuedeHacerDiplomacia;

		#endregion

		#region Eventos

		/// <summary>
		/// Occurs when al cambiar permiso atacar.
		/// </summary>
		public event EventHandler AlCambiarPermisoAtacar;
		/// <summary>
		/// Occurs when al cambiar permiso paso.
		/// </summary>
		public event EventHandler AlCambiarPermisoPaso;

		#endregion
	}
}