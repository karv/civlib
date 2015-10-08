using System;
using Civ.Data;

namespace Civ
{
	/// <summary>
	/// Representa una instancia trabajo en una instancia de edificio.
	/// </summary>
	public class Trabajo: ITickable
	{
		#region General

		public override string ToString ()
		{
			return string.Format (
				"{0} trabajadores haciendo {1} en {2} de la ciudad {3}",
				Trabajadores,
				RAW.Nombre,
				EdificioBase.Nombre,
				CiudadDueño.Nombre);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Trabajo"/> class.
		/// </summary>
		/// <param name="nRAW">Clase de trabajo</param>
		/// <param name="eBase">Edificio base</param>
		public Trabajo (TrabajoRAW nRAW, Edificio eBase)
		{
			RAW = nRAW;
			EdificioBase = eBase;
			EdificioBase.Trabajos.Add (this);
			Trabajadores = 0;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Trabajo"/> class.
		/// Busca el edificio requerido en la ciudad especificada
		/// </summary>
		/// <param name="nRAW">Clase de trabajo</param>
		/// <param name="ciudad">Ciudad</param>
		public Trabajo (TrabajoRAW nRAW, Ciudad ciudad)
			: this (nRAW, ciudad.EncuentraInstanciaEdificio (nRAW.Edificio))
		{
		}

		/// <summary>
		/// Devuelve el tipo de trabajo de esta instancia.
		/// </summary>
		/// <value>The RA.</value>
		public TrabajoRAW RAW { get; }

		/// <summary>
		/// Prioridad del trabajo.
		/// Por ahora se usa exclusivamente para saber qué trabajadores se deben liberar cuando se requiera.
		/// </summary>
		public float Prioridad;

		#endregion

		#region Ciudad Edificio

		/// <summary>
		/// Devuelve el edificio base de esta instancia.
		/// </summary>
		/// <value>The edificio base.</value>
		public Edificio EdificioBase { get; }

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
		public ICivilización CivDueño
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
				return CiudadDueño.Almacen;
			}
		}

		#endregion

		#region ITickable

		/// <summary>
		/// Ejecuta un tick de tiempo
		/// </summary>
		public void Tick (TimeSpan t)
		{
			if (Trabajadores > 0)
			{
				// Obtener eficiencia (generada por la disponibilidad de recursos)
				float PctProd = GetEficiencia (t);

				// Consumir recursos
				foreach (var x in RAW.EntradaBase.Keys)
				{
					Almacén [x] -= RAW.EntradaBase [x] * Trabajadores * PctProd * (float)t.TotalHours;
				}


				// Producir recursos
				foreach (var x in RAW.SalidaBase.Keys)
				{
					Almacén [x] += RAW.SalidaBase [x] * Trabajadores * PctProd * (float)t.TotalHours;
				}
			}
		}

		/// <summary>
		/// Devuelve la eficiencia de este trabajo.
		/// </summary>
		/// <returns>The eficiencia.</returns>
		public float GetEficiencia (TimeSpan t)
		{
			float PctProd = 1;
			foreach (var x in RAW.EntradaBase.Keys)
			{
				PctProd = Math.Min (
					PctProd,
					Almacén [x] / (RAW.EntradaBase [x] * Trabajadores * (float)t.TotalHours));
			}
			return PctProd;
		}


		#endregion

		#region Trabajadores

		ulong _trabajadores;

		/// <summary>
		/// Devuelve o establece el número de trabajadores ocupados en este trabajo.
		/// </summary>
		/// <value>The trabajadores.</value>
		public ulong Trabajadores
		{
			get
			{
				return _trabajadores;
			}
			set
			{
				_trabajadores = 0;
				ulong realValue = Math.Min (value, EdificioBase.EspaciosTrabajadoresCiudad);
				_trabajadores = realValue;
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
				return EdificioBase.EspaciosTrabajadoresCiudad + Trabajadores;
			}
		}

		#endregion
	}
}