using System.Collections.Generic;
using Civ.Ciencias;
using Civ.RAW;
using System.Linq;
using System;
using Civ.ObjetosEstado;
using Civ.Topología;
using System.IO;
using System.Diagnostics;

namespace Civ.Global
{
	/// <summary>
	/// Representa las opciones del juego.
	/// </summary>
	[Serializable]
	public class GameData
	{
		#region Listas

		/// <summary>
		/// Las ciencias
		/// </summary>
		public HashSet<Ciencia> Ciencias = new HashSet<Ciencia> ();
		/// <summary>
		/// Los edificios
		/// </summary>
		public HashSet<EdificioRAW> Edificios = new HashSet<EdificioRAW> ();
		/// <summary>
		/// Los recursos
		/// </summary>
		public HashSet<Recurso> Recursos = new HashSet<Recurso> ();
		/// <summary>
		/// Las unidades
		/// </summary>
		public HashSet<IUnidadRAW> Unidades = new HashSet<IUnidadRAW> ();

		/// <summary>
		/// Los ecosistemas
		/// </summary>
		public HashSet<Ecosistema> Ecosistemas = new HashSet<Ecosistema> ();

		#endregion

		#region Otros

		/// <summary>
		/// El recurso que sirve como alimento en una ciudad.
		/// </summary>
		public Recurso RecursoAlimento;

		/// 

		#endregion

		#region Contar y encontrar

		public IEnumerable<TrabajoRAW> Trabajos ()
		{
			foreach (var x in Edificios)
			{
				foreach (var y in x.Trabajos)
				{
					yield return y;
				}
			}
		}

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
		/// Devuelve un conjunto con todas las Propiedades en Data
		/// </summary>
		[Obsolete]
		public IEnumerable<Propiedad> ListarPropiedades ()
		{
			foreach (var x in Ecosistemas)
			{
				foreach (var y in x.PropPropiedad.Keys)
				{
					yield return y;
				}
			}
		}

		/// <summary>
		/// Devuelve el edificio con un nombre específico.
		/// </summary>
		/// <returns>The recurso.</returns>
		/// <param name="nombre">Nombre del edificio a buscar.</param>
		[Obsolete]
		public Propiedad EncuentraPropiedad (string nombre)
		{
			return ListarPropiedades ().FirstOrDefault (x => x.Nombre == nombre);
		}

		/// <summary>
		/// Devuelve el trabajo con un nombre específico.
		/// </summary>
		/// <returns>TrabajoRAW.</returns>
		/// <param name="nombre">Nombre del Trabajo a buscar.</param>
		public TrabajoRAW EncuentraTrabajo (string nombre)
		{
			return Trabajos ().FirstOrDefault (x => x.Nombre == nombre);
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

		#endregion

		#region Debug

		/// <summary>
		/// Revisa icono por icono para ver si todos están bien definidos.
		/// </summary>
		[Conditional ("DEBUG")]
		public void ProbarIntegridadIconos ()
		{
			foreach (var x in Recursos)
			{
				if (x.Img == null)
				{
					Debug.WriteLine (string.Format ("Recurso {0} con enlace a icono roto. Usando icono genérico.", 
						x.Nombre));
				}
				else if (!File.Exists ("img//" + x.Img))
					Debug.WriteLine (string.Format (
						"Imagen del recurso {0} Perdido",
						x.Nombre));
			}
		}

		#endregion
	}
}