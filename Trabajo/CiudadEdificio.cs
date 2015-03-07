using System;
using ListasExtra;

namespace Civ
{
	public partial class Trabajo
	{
		Edificio _EdificioBase;

		/// <summary>
		/// Devuelve el edificio base de esta instancia.
		/// </summary>
		/// <value>The edificio base.</value>
		public Edificio EdificioBase
		{
			get
			{
				return _EdificioBase;
			}
		}

		/// <summary>
		/// Devuelve la ciudad que posee esta instancia de trabajo.
		/// </summary>
		/// <value>The ciudad dueño.</value>
		public Ciudad CiudadDueño
		{
			get
			{
				return EdificioBase.CiudadDueño;
			}
		}

		/// <summary>
		/// Devuelve la civilización que posee este trabajo.
		/// </summary>
		/// <value>The civ dueño.</value>
		public Civilizacion CivDueño
		{
			get
			{
				return CiudadDueño.CivDueno;
			}
		}

		/// <summary>
		/// Devuelve la lista de recursos de la ciudad.
		/// </summary>
		/// <value>The almacén.</value>
		public AlmacenCiudad Almacén
		{
			get
			{
				return CiudadDueño.Almacén;
			}
		}
	}
}