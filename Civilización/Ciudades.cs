using System;
using System.Collections.Generic;

namespace Civ
{
	public partial class Civilizacion
	{
		/// <summary>
		/// Lista de ciudades.
		/// </summary>
		List<Ciudad> Ciudades = new List<Ciudad>();

		/// <summary>
		/// Devuelve la lista de ciudades que pertenecen a esta <see cref="Civ.Civilización"/>.
		/// </summary>
		/// <value>The get ciudades.</value>
		public List<Ciudad> getCiudades
		{
			get
			{
				return Ciudades;
			}
		}
		/// <summary>
		/// Agrega una ciudad a esta civ.
		/// </summary>
		/// <param name="C">C.</param>
		public void addCiudad(Ciudad C)
		{
			if (C.CivDueño != this)
				C.CivDueño = this;
		}

		/// <summary>
		/// Quita una ciudad de la civilización, haciendo que quede sin <c>CivDueño</c>.
		/// </summary>
		/// <param name="C">Ciudad a quitar.</param>
		public void removeCiudad(Ciudad C)
		{
			if (C.CivDueño == this)
				C.CivDueño = null;
		}
		/// <summary>
		/// Agrega una nueva ciudad a esta civ.
		/// </summary>
		/// <returns>Devuelve la ciudad que se agregó.</returns>
		/// <param name="Nom">Nombre de la ciudad.</param>
		public Ciudad addCiudad(string Nom, Terreno T)
		{
			Ciudad C = new Ciudad(Nom, this, T);
			return C;
		}

        /// <summary>
        /// Cuenta el número de edificios que existen en la ciudad.
        /// </summary>
        /// <param name="Edif"></param>
        /// <returns></returns>
        public int CuentaEdificios(EdificioRAW Edif)
        {
            int ret = 0;
            foreach (var x in Ciudades)
            {
                ret += x.NumEdificios(Edif);
            }
            return ret;
        }
	}
}

