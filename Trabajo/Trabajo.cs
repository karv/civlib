using System;
using ListasExtra;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Civ
{

	/// <summary>
	/// Representa una instancia trabajo en una instancia de edificio.
	/// </summary>
	public partial class Trabajo
	{
		public override string ToString()
		{
			return string.Format("{0} trabajadores haciendo {1} en {2} de la ciudad {3}", Trabajadores, RAW.Nombre, EdificioBase.Nombre, CiudadDueño.Nombre);
		}

		public Trabajo(TrabajoRAW nRAW, Edificio EBase)
		{
			_RAW = nRAW;
			_EdificioBase = EBase;
			_EdificioBase.Trabajos.Add(this);
		}

		TrabajoRAW _RAW;

		/// <summary>
		/// Devuelve el tipo de trabajo de esta instancia.
		/// </summary>
		/// <value>The RA.</value>
		public TrabajoRAW RAW
		{
			get
			{
				return _RAW;
			}
		}

		// Lo nuevo
		public float Prioridad;

	}
}