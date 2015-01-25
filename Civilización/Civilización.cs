using System;
using System.Collections.Generic;
using Basic;

namespace Civ
{
	public partial class Civilizacion
	{
		/// <summary>
		/// Nombre de la <see cref="Str.Civilización"/>.
		/// </summary>
		public string Nombre;


			// Economía
		/// <summary>
		/// Devuelve la cantidad que existe en la civilización de un cierto recurso.
		/// </summary>
		/// <returns>Devuelve la suma de la cantidad que existe de algún recurso sobre cada ciudad.</returns>
		/// <param name="R">Recurso que se quiere contar</param>
		public float ObtenerGlobalRecurso(Recurso R)
		{
			float ret = 0;
			foreach (var x in Ciudades) {
				ret += x.Almacén [R];
			}
			return ret;
		}

        // TODO: Hacer clase que controle esto:

        /// <summary>
        /// Lista de mensajes de eventos para el usuario.
        /// </summary>
        public List<string> Msj = new List<string>();

	}
}

