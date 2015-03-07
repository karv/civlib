// Trabajos y trabajadores
using System;

namespace Civ
{
	public partial class Edificio
	{

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
	}
}