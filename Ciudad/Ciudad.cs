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
		public Civilizacion CivDueño
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
			CivDueño = Dueño;
			T.CiudadConstruida = this;
			Terr = T;

			// Inicializar la armada
			Defensa = new Armada();
			Defensa.MaxPeso = float.PositiveInfinity;

			// Importar desde T.

			foreach (var x in T.Innatos)
			{
				// Si r.next < (algo):
				AgregaPropiedad(x);
			}

			foreach (var x in T.Eco.RecursoEcologico.Keys)
			{
				Almacén[x] = T.Eco.RecursoEcologico[x].Cant;
			}
		}
		// Partial no asinado. 
		/// <summary>
		/// Terreno donde se contruye la ciudad.
		/// </summary>
		public Terreno Terr;

		/// <summary>
		/// Hacer que la ciudad tenga al menos un número de trabajadores libres. Liberando por prioridad.
		/// </summary>
		/// <param name="n"></param>
		public void LiberarTrabajadores(ulong n)
		{

			List<Trabajo> L = ObtenerListaTrabajos;
			L.Sort((x, y) => x.Prioridad < y.Prioridad ? -1 : 1); // Ordenar por prioridad.
			while (L.Count > 0 && getTrabajadoresDesocupados < n && getTrabajadoresDesocupados != getPoblación)
			{
				L[0].Trabajadores = 0;
				L.RemoveAt(0);
			}
		}
	}
}

