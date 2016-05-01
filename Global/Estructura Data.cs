using Civ;
using System.Collections.Generic;
using Civ.Ciencias;
using Civ.RAW;
using System.Linq;
using System;
using Civ.ObjetosEstado;

namespace Civ.Global
{
	/// <summary>
	/// Representa las opciones del juego.
	/// </summary>
	[Serializable]
	public class GameData
	{
		public HashSet<Ciencia> Ciencias = new HashSet<Ciencia> ();
		public HashSet<EdificioRAW> Edificios = new HashSet<EdificioRAW> ();
		public HashSet<Recurso> Recursos = new HashSet<Recurso> ();
		public HashSet<TrabajoRAW> Trabajos = new HashSet<TrabajoRAW> ();
		public HashSet<IUnidadRAW> Unidades = new HashSet<IUnidadRAW> ();
		public HashSet<Propiedad> Propiedades = new HashSet<Propiedad> ();
		public HashSet<Ecosistema> Ecosistemas = new HashSet<Ecosistema> ();

		/// <summary>
		/// El recurso que sirve como alimento en una ciudad.
		/// </summary>
		public Recurso RecursoAlimento;

		/// <summary>
		/// Revisa si existe una edificio con un nombre específico.
		/// </summary>
		/// <returns><c>true</c>, si existe un edificio con ese nombre, <c>false</c> otherwise.</returns>
		/// <param name="nombreEdificio">Nombre del eidficio.</param>
		public bool ExisteEdificio (string nombreEdificio)
		{
			foreach (var x in Edificios)
			{
				if (x.Nombre == nombreEdificio)
					return true;
			}
			return false;
		}

		public bool ExisteEdificio (EdificioRAW edificio)
		{
			foreach (EdificioRAW x in Edificios)
			{
				if (x == edificio)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Revisa si existe una ciencia con un nombre específico.
		/// </summary>
		/// <returns><c>true</c>, si existe una ciencia con ese nombre, <c>false</c> otherwise.</returns>
		/// <param name="nombreRecurso">Nombre del recurso.</param>
		public bool ExisteRecurso (string nombreRecurso)
		{
			foreach (var x in Recursos)
			{
				if (x.Nombre == nombreRecurso)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Revisa si existe una ciencia con un nombre específico.
		/// </summary>
		/// <returns><c>true</c>, si existe una ciencia con ese nombre, <c>false</c> otherwise.</returns>
		/// <param name="nombreCiencia">Nombre ciencia.</param>
		public bool ExisteCiencia (string nombreCiencia)
		{
			foreach (var x in Ciencias)
			{
				if (x.Nombre == nombreCiencia)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Devuelve el edificio con un nombre específico.
		/// </summary>
		/// <returns>The recurso.</returns>
		/// <param name="nombre">Nombre del edificio a buscar.</param>
		public EdificioRAW EncuentraEdificio (string nombre)
		{
			foreach (var x in Edificios)
			{
				if (x.Nombre == nombre)
				{
					return x;
				}
			}
			return null;
		}

		/// <summary>
		/// Devuelve el edificio con un nombre específico.
		/// </summary>
		/// <returns>The recurso.</returns>
		/// <param name="nombre">Nombre del edificio a buscar.</param>
		public Propiedad EncuentraPropiedad (string nombre)
		{
			foreach (var x in Propiedades)
			{
				if (x.Nombre == nombre)
				{
					return x;
				}
			}
			return null;
		}

		/// <summary>
		/// Devuelve el trabajo con un nombre específico.
		/// </summary>
		/// <returns>TrabajoRAW.</returns>
		/// <param name="nombre">Nombre del Trabajo a buscar.</param>
		public TrabajoRAW EncuentraTrabajo (string nombre)
		{
			return Trabajos.FirstOrDefault (x => x.Nombre == nombre);
		}

		/// <summary>
		/// Devuelve un arreglo de recursos que son científicos
		/// </summary>
		/// <returns>The lista recursos científicos.</returns>
		public IEnumerable<Recurso> ObtenerRecursosCientificos ()
		{
			return Recursos.Where (x => x.EsCientifico);
		}

		/// <summary>
		/// Devuelve la lista de edificios autocontruibles.
		/// </summary>
		/// <returns>The autoconstruibles.</returns>
		public IEnumerable<EdificioRAW> EdificiosAutoconstruibles ()
		{
			return Edificios.Where (x => x.EsAutoConstruible);
		}

		/// <summary>
		/// Devuelve todos los IRequerimientos.
		/// </summary>
		public ICollection<IRequerimiento<ICiudad>> Reqs
		{
			get
			{
				var ret = new List<IRequerimiento<ICiudad>> ();
				foreach (IRequerimiento<ICiudad> x in Edificios)
				{
					ret.Add (x);
				}
				foreach (IRequerimiento<ICiudad> x in Ciencias)
				{
					ret.Add (x);
				}
				return ret;
			}
		}

		/// <summary>
		/// Devuelve una collección de edificios que pueden ser construídos por una ciudad específica.
		/// </summary>
		/// <returns>The edificios construíbles.</returns>
		/// <param name="ciudad">Ciudad para la cual se quiere saber cuáles edificios son construíbles</param>
		public IEnumerable<EdificioRAW> ObtenerEdificiosConstruíbles (ICiudad ciudad)
		{
			return Edificios.Where (ciudad.PuedeConstruir);
		}
	}
}