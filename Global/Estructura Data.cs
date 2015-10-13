using Civ;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Civ.Data;

namespace Global
{
	/// <summary>
	/// Representa las opciones del juego.
	/// </summary>
	[DataContract (
		IsReference = true,
		Namespace = "http://schemas.datacontract.org/2004/07/Civ",
		Name = "Data")]
	public class GameData
	{
		[DataMember (Name = "Ciencias", Order = 3)]
		public C5.HashSet<Ciencia> Ciencias = new C5.HashSet<Ciencia> ();
		[DataMember (Name = "Edificios", Order = 4)]
		public C5.HashSet<EdificioRAW> Edificios = new C5.HashSet<EdificioRAW> ();
		[DataMember (Name = "Recursos", Order = 0)]
		public C5.HashSet<Recurso> Recursos = new C5.HashSet<Recurso> ();
		[DataMember (Name = "Trabajos", Order = 5)]
		public C5.HashSet<TrabajoRAW> Trabajos = new C5.HashSet<TrabajoRAW> ();
		[DataMember (Name = "Unidades", Order = 6)]
		public ListaUnidades Unidades = new ListaUnidades ();
		[DataMember (Name = "Propiedades", Order = 1)]
		public C5.HashSet<Propiedad> Propiedades = new C5.HashSet<Propiedad> ();
		[DataMember (Name = "Ecosistemas", Order = 2)]
		public C5.HashSet<Ecosistema> Ecosistemas = new C5.HashSet<Ecosistema> ();
		/// <summary>
		/// El recurso que sirve como alimento en una ciudad.
		/// </summary>
		[DataMember (Name = "Alimento", Order = 7)]
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
			TrabajoRAW ret;
			return Trabajos.Find (x => x.Nombre == nombre, out ret) ? ret : null;
		}

		/// <summary>
		/// Devuelve un arreglo de recursos que son científicos
		/// </summary>
		/// <returns>The lista recursos científicos.</returns>
		public IEnumerable<Recurso> ObtenerRecursosCientificos ()
		{
			return Recursos.Filter (x => x.EsCientifico);
		}

		/// <summary>
		/// Devuelve la lista de edificios autocontruibles.
		/// </summary>
		/// <returns>The autoconstruibles.</returns>
		public IEnumerable<EdificioRAW> EdificiosAutoconstruibles ()
		{
			return Edificios.Filter (x => x.EsAutoConstruible);
		}

		/// <summary>
		/// Devuelve todos los IRequerimientos.
		/// </summary>
		[System.Xml.Serialization.XmlIgnore]
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
	}
}