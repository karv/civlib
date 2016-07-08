using System;
using Civ.RAW;
using Civ.Almacén;
using Civ.Global;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Representa una instancia trabajo en una instancia de edificio.
	/// </summary>
	[Serializable]
	public class Trabajo : ITickable
	{
		#region General

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Civ.ObjetosEstado.Trabajo"/>.
		/// </summary>
		/// <returns>Nombre y datos de empleados</returns>
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
		/// Initializes a new instance of the <see cref="Civ.ObjetosEstado.Trabajo"/> class.
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
		/// Initializes a new instance of the <see cref="Civ.ObjetosEstado.Trabajo"/> class.
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
				return CiudadDueño.CivDueño;
			}
		}

		/// <summary>
		/// Devuelve la lista de recursos de la ciudad.
		/// </summary>
		/// <value>The almacén.</value>
		public IAlmacén Almacén
		{
			get
			{
				return CiudadDueño.Almacén;
			}
		}

		#endregion

		#region ITickable

		/// <summary>
		/// Ejecuta un tick de tiempo
		/// </summary>
		public void Tick (TimeEventArgs t)
		{
			if (Trabajadores > 0)
			{
				AlTickAntes?.Invoke (this, t);
				// Obtener eficiencia (generada por la disponibilidad de recursos)
				float PctProd = GetEficiencia (t.GameTime);

				// Consumir recursos
				foreach (var x in RAW.EntradaBase.Keys)
				{
					Almacén [x] -= RAW.EntradaBase [x] * Trabajadores * PctProd * (float)t.GameTime.TotalHours;
				}


				// Producir recursos
				foreach (var x in RAW.SalidaBase.Keys)
				{
					Almacén [x] += RAW.SalidaBase [x] * Trabajadores * PctProd * (float)t.GameTime.TotalHours;
				}
			}
			AlTickDespués?.Invoke (this, t);
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
				AlCambiarTrabajadores?.Invoke (this, EventArgs.Empty);
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

		#region Eventos

		/// <summary>
		/// Ocurre antes del tick
		/// </summary>
		public event EventHandler AlTickAntes;

		/// <summary>
		/// Ocurre después del tick
		/// </summary>
		public event EventHandler AlTickDespués;

		/// <summary>
		/// Ocurre cuando se cambian los trabajadores
		/// </summary>
		public event EventHandler AlCambiarTrabajadores;

		#endregion
	}
}