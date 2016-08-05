using System;
using System.Collections.Generic;
using Civ.RAW;
using Civ.Global;

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

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Civ.ObjetosEstado.Edificio"/>.
		/// </summary>
		/// <returns>El nombre de la ciudad y el nombre del edificio</returns>
		public override string ToString ()
		{
			return CiudadDueño.Nombre + " - " + RAW.Nombre;
		}

		/// <summary>
		/// El RAW del edificio.
		/// </summary>
		public EdificioRAW RAW { get; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.ObjetosEstado.Edificio"/> class.
		/// </summary>
		/// <param name="nRAW">El RAW del edificio</param>
		public Edificio (EdificioRAW nRAW)
		{
			RAW = nRAW;
			Trabajos = new List<Trabajo> ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.ObjetosEstado.Edificio"/> class.
		/// </summary>
		/// <param name="nRAW">El RAW del edificio</param>
		/// <param name="nCiudad">La ciudad que posee este edificio</param>
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
		public void Tick (TiempoEventArgs t)
		{
			AlTickAntes?.Invoke (this, t);
			foreach (var x in RAW.Salida)
				CiudadDueño.Almacén [x.Key] += x.Value * (float)t.GameTime.TotalHours;

			foreach (var x in Trabajos)
			{
				x.Tick (t);
				if (float.IsNaN (CiudadDueño.AlimentoAlmacen))
					throw new Exception ();
			}
			AlTickDespués?.Invoke (this, t);
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
		public long Trabajadores
		{
			get
			{
				long ret = 0;
				foreach (var x in Trabajos)
					ret += x.Trabajadores;
				return ret;
			}
		}

		/// <summary>
		/// Devuelve en número de espacios para trabajadores restantes en este edificio.
		/// Ignora el estado de la ciudad.
		/// </summary>
		/// <value>The get espacios trabajadores.</value>
		public long EspaciosTrabajadores
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
		public long EspaciosTrabajadoresCiudad
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
		public long this [Trabajo trabajo]
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

		#region Eventos

		/// <summary>
		/// Ocurre antes del tick
		/// </summary>
		public event EventHandler<TiempoEventArgs> AlTickAntes;
		/// <summary>
		/// Ocurre después del tick
		/// </summary>
		public event EventHandler<TiempoEventArgs> AlTickDespués;

		#endregion

		/// <summary>
		/// Serves as a hash function for a <see cref="Civ.ObjetosEstado.Edificio"/> object.
		/// Es el hashcode de RAW, ya que el comparador se hace con el RAW (sólo se usa en Ciudad.Edicios,
		/// que es un HashSet.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode ()
		{
			return RAW.GetHashCode ();
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Civ.ObjetosEstado.Edificio"/>.
		/// Usa igualdad de RAW, con obj.RAW, si éste es Edificio.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Civ.ObjetosEstado.Edificio"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current
		/// <see cref="Civ.ObjetosEstado.Edificio"/>; otherwise, <c>false</c>.</returns>
		public override bool Equals (object obj)
		{
			var otro = obj as Edificio;
			return otro != null && otro.RAW.Equals (RAW);
		}
	}
}