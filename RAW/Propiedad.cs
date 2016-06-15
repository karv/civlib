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

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.RAW.Propiedad"/> class.
		/// </summary>
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

		/// <summary>
		/// Recursos que agrega a un ecosistema al inicio de una partida al tener esta propiedad
		/// </summary>
		public Dictionary<Recurso, float> Iniciales;

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Civ.RAW.Propiedad"/>.
		/// </summary>
		/// <returns>El nombre de la propiedad.</returns>
		public override string ToString ()
		{
			return Nombre;
		}
	}
}