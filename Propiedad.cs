using System;
using System.Collections.Generic;

namespace Civ
{
	/// <summary>
	/// Representa una propiedad: un edificio que produce sin trabajadores.
	/// </summary>
	public class Propiedad: IRequerimiento
	{
		public Propiedad ()
		{
		}

		/// <summary>
		/// Nombre de la propiedad.
		/// </summary>
		public string Nombre;

		List<IRequerimiento> _Reqs = new List<IRequerimiento>();
		/// <summary>
		/// Lista de requerimientos para esta propiedad.
		/// </summary>
		/// <value>The reqs.</value>
		public List<IRequerimiento> Reqs {
			get {
				return _Reqs;
			}
		}

		// IRequerimiento
		string IRequerimiento.ObtenerId()
		{
			return Nombre;
		}
		bool IRequerimiento.LoSatisface (Ciudad C)
		{
			return C.ExistePropiedad (this);
		}

		//TODO: La salida de producción.
	}
}

