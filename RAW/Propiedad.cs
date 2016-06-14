using System.Collections.Generic;
using System;
using Civ.ObjetosEstado;
using Civ.Global;
using Civ.Almacén;

namespace Civ.RAW
{
	/// <summary>
	/// Representa una propiedad innata de un edificio.
	/// </summary>
	[Serializable]
	public class Propiedad : IRequerimiento<Ciudad>
	{
		/// <summary>
		/// Nombre de la propiedad.
		/// </summary>
		public string Nombre;

		public Propiedad ()
		{
			Salida = new HashSet<TasaProd> ();
			Iniciales = new Dictionary<Recurso, float> ();
		}

		/// <summary>
		/// Recursos que produce esta propiedad por turno.
		/// </summary>
		public ICollection<TasaProd> Salida { get; private set; }
		// IRequerimiento:
		bool IRequerimiento<Ciudad>.LoSatisface (Ciudad ciudad)
		{
			return ciudad.ExistePropiedad (this);
		}

		/// <summary>
		/// El tick de este edificio sobre una ciudad.
		/// </summary>
		/// <param name="almacén"><see cref="Civ.ObjetosEstado.ICiudad"/> donde hará un tick esta propiedad.</param>
		/// <param name="t">longitud del tick</param>
		public virtual void Tick (IAlmacén almacén, TimeSpan t)
		{
			foreach (TasaProd x in Salida)
			{
				x.Tick (almacén, t);
			}
		}

		public Dictionary<Recurso, float> Iniciales;

		public override string ToString ()
		{
			return Nombre;
		}
	}
}