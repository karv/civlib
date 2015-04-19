using System;
using System.Collections.Generic;

namespace Civ
{
	/// <summary>
	/// Representa una instancia de edificio en una ciudad.
	/// </summary>
	public partial class Edificio: ITickable
	{
		/// <summary>
		/// Devuelve el nombre del (RAW del) edificio.
		/// </summary>
		public string Nombre
		{
			get
			{
				return RAW.Nombre;
			}
		}

		public override string ToString()
		{
			return CiudadDueño.Nombre + " - " + RAW.Nombre;
		}

		/// <summary>
		/// El RAW del edificio.
		/// </summary>
		public readonly EdificioRAW RAW;
		Ciudad _Ciudad;

		public Edificio(EdificioRAW nRAW)
		{
			RAW = nRAW;
		}

		public Edificio(EdificioRAW nRAW, Ciudad nCiudad)
			: this(nRAW)
		{
			if (nCiudad.ExisteEdificio(nRAW))
				throw new Exception(string.Format("Error. En la ciudad {1} se quiere construir un edificio {0}, pero ya existe tal edificio.", nRAW, nCiudad));
			_Ciudad = nCiudad;
			_Ciudad.Edificios.Add(this);
		}

		/// <summary>
		/// Devuelve o establece la ciudad a la que pertenece este edificio.
		/// </summary>
		/// <value></value>
		public Ciudad CiudadDueño
		{
			get
			{
				return _Ciudad;
			}
		}

		/// <summary>
		/// Produce un tick productivo hereditario.
		/// </summary>
		public void Tick(float t = 1)
		{
			foreach (var x in RAW.Salida)
			{
				CiudadDueño.Almacen[x.Key] += x.Value * t;
			}

			foreach (var x in Trabajos)
			{
				x.Tick(t);
			}
		}
	}
}