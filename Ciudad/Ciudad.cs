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
			Defensa = new Armada(CivDueno, true);
			Defensa.MaxPeso = float.PositiveInfinity;
			Defensa.Posicion = (Pseudoposicion)T;

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

		/// <summary>
		/// Devuelve un nuevo diccionario cuyas entradas son el número de unidades que puede construir la ciudad, por cada unidad.
		/// </summary>
		/// <returns>The construibles.</returns>
		public Dictionary<UnidadRAW, ulong> UnidadesConstruibles()
		{
			Dictionary <UnidadRAW, ulong> ret = new Dictionary<UnidadRAW, ulong>();

			foreach (var x in Global.g_.Data.Unidades)
			{
				if (x.ReqCiencia == null || CivDueno.Avances.Contains(x.ReqCiencia))
				{
					ret.Add(x, UnidadesConstruibles(x));
				}
			}
			return ret;
		}

		/// <summary>
		/// Devuelve la cantidad de unidades que puede construir esta ciudad de una unidadRAW específica.
		/// Tiene en cuenta sólo los recursos y la población desocupada.
		/// </summary>
		/// <returns>The construibles.</returns>
		/// <param name="unid">Unid.</param>
		public ulong UnidadesConstruibles(UnidadRAW unid)
		{
			ulong max = getTrabajadoresDesocupados;
			foreach (var y in unid.Reqs)
			{
				// ¿Cuántas unidades puede hacer, limitando por recursos?
				max = (ulong)Math.Min(Almacen[y.Key] / y.Value, max);
			}
			return max;
		}
	}
}