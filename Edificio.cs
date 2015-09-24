using System;
using System.Collections.Generic;

namespace Civ
{
	/// <summary>
	/// Representa una instancia de edificio en una ciudad.
	/// </summary>
	public class Edificio: ITickable
	{
		#region General

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
		public void Tick(float t)
		{
			if (RAW.Salida != null)
				foreach (var x in RAW.Salida)
				{
					CiudadDueño.Almacen[x.Key] += x.Value * t;
				}

			foreach (var x in Trabajos)
			{
				x.Tick(t);
				if (float.IsNaN(CiudadDueño.AlimentoAlmacen))
					throw new Exception();
			}

		}

		#endregion

		#region Trabajo

		readonly List<Trabajo> _Trabajo = new List<Trabajo>();

		/// <summary>
		/// Devuelve la lista de instancias de trabajo de este edificio
		/// </summary>
		/// <value>The _ trabajo.</value>
		public List<Trabajo> Trabajos
		{
			get
			{
				return _Trabajo;
			}
		}

		/// <summary>
		/// Devuelve el número de trabajadores ocupados en este edificio.
		/// </summary>
		/// <value>The get trabajadores.</value>
		public ulong Trabajadores
		{
			get
			{
				ulong ret = 0;
				foreach (var x in _Trabajo)
				{
					ret += x.Trabajadores;
				}
				return ret;
			}
		}

		/// <summary>
		/// Devuelve en número de espacios para trabajadores restantes en este edificio.
		/// Ignora el estado de la ciudad.
		/// </summary>
		/// <value>The get espacios trabajadores.</value>
		public ulong EspaciosTrabajadores
		{
			get
			{
				return RAW.MaxWorkers - Trabajadores;
			}
		}

		/// <summary>
		/// Devuelve en número de espacios para trabajadores restantes en este edificio.
		/// Tomando en cuenta el estado de la ciudad.
		/// </summary>
		/// <value>The get espacios trabajadores.</value>
		public ulong EspaciosTrabajadoresCiudad
		{
			get
			{
				return Math.Min(EspaciosTrabajadores, CiudadDueño.TrabajadoresDesocupados);
			}
		}
		// Trabajos
		/// <summary>
		/// Devuelve o establece el número de trabajadores en un trabajo
		/// </summary>
		/// <param name="trabajo">El trabajo</param>
		public ulong this [Trabajo trabajo]
		{
			get
			{
				return Trabajos.Contains(trabajo) ? trabajo.Trabajadores : 0;
			}
			set
			{
				if (Trabajos.Contains(trabajo))
				{
					trabajo.Trabajadores = value;
				}
			}
		}

		/// <summary>
		/// Devuelve la instancia de trabajo de un RAW de trabajo.
		/// Si no existe, la crea.
		/// </summary>
		/// <param name="trabajo">El RAW del trabajo.</param>
		public Trabajo this [TrabajoRAW trabajo]
		{
			get
			{
				return InstanciaTrabajo(trabajo);
			}
		}

		/// <summary>
		/// Devuelve la instancia de trabajo de un RAW de trabajo.
		/// Si no existe, la crea.
		/// </summary>
		/// <returns>The instancia trabajo.</returns>
		/// <param name="trabajo">El RAW de trabajo.</param>
		public Trabajo InstanciaTrabajo(TrabajoRAW trabajo)
		{
			foreach (var x in Trabajos)
			{
				if (x.RAW == trabajo)
					return x;
			}
			return new Trabajo(trabajo, this);
		}

		#endregion
	}
}