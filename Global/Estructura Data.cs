using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Global
{
	/// <summary>
	/// Representa las opciones del juego.
	/// </summary>
	[DataContract(IsReference = true, Namespace = "http://schemas.datacontract.org/2004/07/Civ", Name = "Data")]
	public class g_Data
	{
		[DataMember(Name = "Ciencias", Order = 3)]
		public List<Civ.Ciencia> Ciencias = new List<Civ.Ciencia>();
		[DataMember(Name = "Edificios", Order = 4)]
		public List<Civ.EdificioRAW> Edificios = new List<Civ.EdificioRAW>();
		[DataMember(Name = "Recursos", Order = 0)]
		public List<Civ.Recurso> Recursos = new List<Civ.Recurso>();
		[DataMember(Name = "Trabajos", Order = 5)]
		public List<Civ.TrabajoRAW> Trabajos = new List<Civ.TrabajoRAW>();
		[DataMember(Name = "Unidades", Order = 6)]
		public List<Civ.UnidadRAW> Unidades = new List<Civ.UnidadRAW>();
		[DataMember(Name = "Propiedades", Order = 1)]
		public List<Civ.Propiedad> Propiedades = new List<Civ.Propiedad>();
		[DataMember(Name = "Ecosistemas", Order = 2)]
		public List<Civ.Ecosistema> Ecosistemas = new List<Civ.Ecosistema>();
		/// <summary>
		/// El recurso que sirve como alimento en una ciudad.
		/// </summary>
		[DataMember(Name = "Alimento", Order = 7)]
		public Civ.Recurso RecursoAlimento;


		/// <summary>
		/// Revisa si existe una edificio con un nombre específico.
		/// </summary>
		/// <returns><c>true</c>, si existe un edificio con ese nombre, <c>false</c> otherwise.</returns>
		/// <param name="NombreRecurso">Nombre del eidficio.</param>
		public bool ExisteEdificio(string NombreEdificio)
		{
			foreach (var x in Edificios)
			{
				if (x.Nombre == NombreEdificio)
					return true;
			}
			return false;
		}

		public bool ExisteEdificio(Civ.EdificioRAW Edificio)
		{
			foreach (Civ.EdificioRAW x in Edificios)
			{
				if (x == Edificio)
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
		/// <param name="NombreRecurso">Nombre del recurso.</param>
		public bool ExisteRecurso(string NombreRecurso)
		{
			foreach (var x in Recursos)
			{
				if (x.Nombre == NombreRecurso)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Revisa si existe una ciencia con un nombre específico.
		/// </summary>
		/// <returns><c>true</c>, si existe una ciencia con ese nombre, <c>false</c> otherwise.</returns>
		/// <param name="NombreCiencia">Nombre ciencia.</param>
		public bool ExisteCiencia(string NombreCiencia)
		{
			foreach (var x in Ciencias)
			{
				if (x.Nombre == NombreCiencia)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Devuelve el edificio con un nombre específico.
		/// </summary>
		/// <returns>The recurso.</returns>
		/// <param name="nombre">Nombre del edificio a buscar.</param>
		public Civ.EdificioRAW EncuentraEdificio(string nombre)
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
		/// <param name="Nombre">Nombre del edificio a buscar.</param>
		public Civ.Propiedad EncuentraPropiedad(string Nombre)
		{
			foreach (var x in Propiedades)
			{
				if (x.Nombre == Nombre)
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
		public Civ.TrabajoRAW EncuentraTrabajo(string nombre)
		{
			return Trabajos.Find(x => x.Nombre == nombre);
		}

		/// <summary>
		/// Devuelve un arreglo de recursos que son científicos
		/// </summary>
		/// <returns>The lista recursos científicos.</returns>
		public Civ.Recurso[] ObtenerRecursosCientificos()
		{
			return Recursos.FindAll(y => y.EsCientifico).ToArray();
		}

		/// <summary>
		/// Devuelve la lista de edificios autocontruibles.
		/// </summary>
		/// <returns>The autoconstruibles.</returns>
		public List<Civ.EdificioRAW> EdificiosAutoconstruibles()
		{
			return Edificios.FindAll(x => x.EsAutoConstruible);
		}

		/// <summary>
		/// Devuelve todos los <see cref="Civ.IRequerimiento"/>s.
		/// </summary>
		[System.Xml.Serialization.XmlIgnore()]
		public List<Civ.IRequerimiento> Reqs
		{
			get
			{
				List<Civ.IRequerimiento> ret = new List<Civ.IRequerimiento>();
				foreach (Civ.IRequerimiento x in Edificios)
				{
					ret.Add(x);
				}
				foreach (Civ.IRequerimiento x in Ciencias)
				{
					ret.Add(x);
				}
				return ret;
			}
		}
	}
}