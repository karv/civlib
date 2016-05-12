using System;
using System.Collections.Generic;
using Civ.RAW;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Representa una instancia de edificio en una ciudad.
	/// </summary>
	[Serializable]
	public class Edificio : ITickable, IPuntuado
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

		public override string ToString ()
		{
			return CiudadDueño.Nombre + " - " + RAW.Nombre;
		}

		/// <summary>
		/// El RAW del edificio.
		/// </summary>
		public EdificioRAW RAW { get; }

		public Edificio (EdificioRAW nRAW)
		{
			RAW = nRAW;
			Trabajos = new List<Trabajo> ();
		}

		public Edificio (EdificioRAW nRAW, Ciudad nCiudad)
			: this (nRAW)
		{
			if (nCiudad.ExisteEdificio (nRAW))
				throw new Exception (string.Format (
					"Error. En la ciudad {1} se quiere construir un edificio {0}, pero ya existe tal edificio.",
					nRAW,
					nCiudad));
			CiudadDueño = nCiudad;
			CiudadDueño.Edificios.Add (this);
		}

		/// <summary>
		/// Devuelve o establece la ciudad a la que pertenece este edificio.
		/// </summary>
		/// <value></value>
		public Ciudad CiudadDueño { get; }

		/// <summary>
		/// Produce un tick productivo hereditario.
		/// </summary>
		public void Tick (TimeSpan t)
		{
			AlTickAntes?.Invoke (t);
			if (RAW.Salida != null)
				foreach (var x in RAW.Salida)
				{
					CiudadDueño.Almacén [x.Key] += x.Value * (float)t.TotalHours;
				}

			foreach (var x in Trabajos)
			{
				x.Tick (t);
				if (float.IsNaN (CiudadDueño.AlimentoAlmacen))
					throw new Exception ();
			}
			AlTickDespués?.Invoke (t);
		}

		#endregion

		#region Trabajo

		/// <summary>
		/// Devuelve la lista de instancias de trabajo de este edificio
		/// </summary>
		/// <value>The _ trabajo.</value>
		public IList<Trabajo> Trabajos { get; }

		/// <summary>
		/// Devuelve el número de trabajadores ocupados en este edificio.
		/// </summary>
		/// <value>The get trabajadores.</value>
		public ulong Trabajadores
		{
			get
			{
				ulong ret = 0;
				foreach (var x in Trabajos)
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
				return Math.Min (EspaciosTrabajadores, CiudadDueño.TrabajadoresDesocupados);
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
				return Trabajos.Contains (trabajo) ? trabajo.Trabajadores : 0;
			}
			set
			{
				if (Trabajos.Contains (trabajo))
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
				return InstanciaTrabajo (trabajo);
			}
		}

		/// <summary>
		/// Devuelve la instancia de trabajo de un RAW de trabajo.
		/// Si no existe, la crea.
		/// </summary>
		/// <returns>The instancia trabajo.</returns>
		/// <param name="trabajo">El RAW de trabajo.</param>
		public Trabajo InstanciaTrabajo (TrabajoRAW trabajo)
		{
			foreach (var x in Trabajos)
			{
				if (x.RAW == trabajo)
					return x;
			}
			return new Trabajo (trabajo, this);
		}

		#endregion

		#region Puntuado

		const float CoefPunt = 1.1f;

		float IPuntuado.Puntuación
		{
			get
			{
				var ret = 0f;
				foreach (var x in RAW.ReqRecursos)
				{
					ret += x.Key.Valor * x.Value;
				}
				return ret * CoefPunt;
			}
		}

		#endregion

		public event Action<TimeSpan> AlTickAntes;

		public event Action<TimeSpan> AlTickDespués;
	}
}