using System;
using ListasExtra;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Civ
{
	/// <summary>
	/// Representa una instancia trabajo en una instancia de edificio.
	/// </summary>
	public class Trabajo: ITickable
	{
		#region General

		public override string ToString()
		{
			return string.Format("{0} trabajadores haciendo {1} en {2} de la ciudad {3}", Trabajadores, RAW.Nombre, EdificioBase.Nombre, CiudadDueño.Nombre);
		}

		public Trabajo(TrabajoRAW nRAW, Edificio EBase)
		{
			_RAW = nRAW;
			_EdificioBase = EBase;
			_EdificioBase.Trabajos.Add(this);
			Trabajadores = 0;
		}

		public Trabajo(TrabajoRAW nRAW, Ciudad ciudad) : this(nRAW, ciudad.EncuentraInstanciaEdificio(nRAW.Edificio))
		{
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

		/// <summary>
		/// Prioridad del trabajo.
		/// Por ahora se usa exclusivamente para saber qué trabajadores se deben liberar cuando se requiera.
		/// </summary>
		public float Prioridad;
		///

		#endregion

		#region Ciudad Edificio
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
		public AlmacenCiudad Almacen
		{
			get
			{
				return CiudadDueño.Almacen;
			}
		}

		#endregion

		#region ITickable

		/// <summary>
		/// Ejecuta un tick de tiempo
		/// </summary>
		public void Tick(float t = 1)
		{
			if (Trabajadores > 0)
			{
				// Obtener eficiencia (generada por la disponibilidad de recursos)
				float PctProd = 1;
				foreach (var x in RAW.EntradaBase.Keys)
				{
					PctProd = Math.Min(PctProd, Almacen[x] / (RAW.EntradaBase[x] * Trabajadores * t));
				}

				// Consumir recursos
				foreach (var x in RAW.EntradaBase.Keys)
				{
					Almacen.changeRecurso(x, -RAW.EntradaBase[x] * Trabajadores * PctProd * t);
				}


				// Producir recursos
				foreach (var x in RAW.SalidaBase.Keys)
				{
					Almacen.changeRecurso(x, RAW.SalidaBase[x] * Trabajadores * PctProd * t);
				}
			}
		}

		#endregion

		#region Trabajadores

		ulong _Trabajadores;

		/// <summary>
		/// Devuelve o establece el número de trabajadores ocupados en este trabajo.
		/// </summary>
		/// <value>The trabajadores.</value>
		public ulong Trabajadores
		{
			get
			{
				return _Trabajadores;
			}
			set
			{
				ulong realValue;

				_Trabajadores = 0;
				realValue = (ulong)Math.Min(value, EdificioBase.getEspaciosTrabajadoresCiudad);
				_Trabajadores = realValue;
			}
		}

		/// <summary>
		/// Devuelve el máximo número de trabajadores que tienen espacio en este trabajo actualmente.
		/// </summary>
		/// <value>The max trabajadores.</value>
		public ulong MaxTrabajadores
		{
			get
			{
				return EdificioBase.getEspaciosTrabajadoresCiudad + Trabajadores;
			}
		}

		#endregion
	}
}