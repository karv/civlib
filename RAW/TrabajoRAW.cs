using ListasExtra;
using System.Collections.Generic;
using System;
using Civ.ObjetosEstado;
using Civ.Global;

namespace Civ.RAW
{
	/// <summary>
	/// Representa un trabajo en un edificioRAW
	/// </summary>	
	[Serializable]
	public class TrabajoRAW
	{
		public TrabajoRAW ()
		{
			EntradaBase = new ListaPeso<Recurso> ();
			SalidaBase = new ListaPeso<Recurso> ();
		}

		/// <summary>
		/// Nombre
		/// </summary>
		public string Nombre;
		/// <summary>
		/// EdificioRAW vinculado a este trabajo.
		/// </summary>
		public EdificioRAW Edificio;

		/// <summary>
		/// Recursos producidos por trabajador*turno (Base)
		/// </summary>
		public ListaPeso<Recurso> SalidaBase { get; }

		/// <summary>
		/// Recursos consumidos por trabajador*turno (Base)
		/// </summary>
		public ListaPeso<Recurso> EntradaBase { get; }

		public override string ToString ()
		{
			return string.Format ("{0} @ {1}", Nombre, Edificio);
		}
		// Requiere
		/// <summary>
		/// Lista de requerimientos.
		/// </summary>
		public Requerimiento Requiere = new Requerimiento ();

		/// <summary>
		/// Devuelve la lista de requerimientos.
		/// </summary>
		/// <value>El IRequerimiento</value> 
		public ICollection<IRequerimiento<ICiudad>> Reqs ()
		{
			return Requiere.Requiere ();
		}
	}
}