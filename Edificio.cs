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
		public void Tick(float t = 1)
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

		System.Collections.Generic.List<Trabajo> _Trabajo = new System.Collections.Generic.List<Trabajo>();

		/// <summary>
		/// Devuelve la lista de instancias de trabajo de este edificio
		/// </summary>
		/// <value>The _ trabajo.</value>
		public System.Collections.Generic.List<Trabajo> Trabajos
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
		public ulong getTrabajadores
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
		public ulong getEspaciosTrabajadores
		{
			get
			{
				return RAW.MaxWorkers - getTrabajadores;
			}
		}

		/// <summary>
		/// Devuelve en número de espacios para trabajadores restantes en este edificio.
		/// Tomando en cuenta el estado de la ciudad.
		/// </summary>
		/// <value>The get espacios trabajadores.</value>
		public ulong getEspaciosTrabajadoresCiudad
		{
			get
			{
				return (ulong)Math.Min(getEspaciosTrabajadores, CiudadDueño.getTrabajadoresDesocupados);
			}
		}
		// Trabajos
		/// <summary>
		/// Devuelve o establece el número de trabajadores en un trabajo
		/// </summary>
		/// <param name="Trab">El trabajo</param>
		public ulong this [Trabajo Trab]
		{
			get
			{
				if (Trabajos.Contains(Trab))
				{
					return Trab.Trabajadores;
				}
				else
					return 0;
			}
			set
			{
				if (Trabajos.Contains(Trab))
				{
					Trab.Trabajadores = value;
				}
			}
		}

		/// <summary>
		/// Devuelve la instancia de trabajo de un RAW de trabajo.
		/// Si no existe, la crea.
		/// </summary>
		/// <param name="Trab">El RAW del trabajo.</param>
		public Trabajo this [TrabajoRAW Trab]
		{
			get
			{
				return getInstanciaTrabajo(Trab);
			}
		}

		/// <summary>
		/// Devuelve la instancia de trabajo de un RAW de trabajo.
		/// Si no existe, la crea.
		/// </summary>
		/// <returns>The instancia trabajo.</returns>
		/// <param name="Trab">El RAW de trabajo.</param>
		public Trabajo getInstanciaTrabajo(TrabajoRAW Trab)
		{
			foreach (var x in Trabajos)
			{
				if (x.RAW == Trab)
					return x;
			}
			return new Trabajo(Trab, this);
		}

		#endregion
	}
}