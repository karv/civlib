using System;
using ListasExtra;
using System.Collections.Generic;

namespace Civ
{
	/// <summary>
	/// Representa una instancia de ciudad.
	/// </summary>
	public partial class Ciudad
	{
		public override string ToString()
		{
			return Nombre;
		}

		/// <summary>
		/// Nombre de la ciudad.
		/// </summary>
		public string Nombre;
		Civilizacion _CivDueño;

		/// <summary>
		/// Devuelve o establece la civilización a la cual pertecene esta ciudad.
		/// </summary>
		/// <value>The civ dueño.</value>
		public Civilizacion CivDueno
		{
			get
			{
				return _CivDueño;
			}
			set
			{
				if (_CivDueño != null)
					_CivDueño.getCiudades.Remove(this);
				_CivDueño = value;
				if (_CivDueño != null)
					_CivDueño.getCiudades.Add(this);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Ciudad"/> class.
		/// </summary>
		/// <param name="Nom">Nombre de la ciudad.</param>
		/// <param name="Dueño">Civ a la que pertenece esta ciudad.</param>
		/// <param name="T">Terreno de contrucción de la ciudad.</param>
		public Ciudad(string Nom, Civilizacion Dueño, Terreno T)
		{
			Nombre = Nom;
			CivDueno = Dueño;
			T.CiudadConstruida = this;
			Terr = T;
			Almacen = new AlmacenCiudad(this);

			// Inicializar la armada
			Defensa = new Armada(CivDueno);
			Defensa.MaxPeso = float.PositiveInfinity;

			// Importar desde T.

			foreach (var x in T.Innatos)
			{
				// Si r.next < (algo):
				AgregaPropiedad(x);
			}
		}
		// Partial no asinado. 
		/// <summary>
		/// Terreno donde se contruye la ciudad.
		/// </summary>
		public Terreno Terr;
	}
}
