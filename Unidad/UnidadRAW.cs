using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ListasExtra;

namespace Civ
{
    /// <summary>
    /// Representa una clase de unidad
    /// </summary>
    public class UnidadRAW
    {
        /// <summary>
        /// El nombre de la clase de unidad.
        /// </summary>
        public string Nombre;

        /// <summary>
        /// Lista de modificadores de combate de la unidad.
        /// </summary>
        public ListaPeso<string> Mods = new ListaPeso<string>(); 

        /// <summary>
        /// Fuerza de la unidad.
        /// </summary>
        public float Fuerza;

        /// <summary>
        /// Flags.
        /// </summary>
        public List<string> Flags = new List<string>();

        // Reqs
        /// <summary>
        /// Requerimientos para crearse.
        /// </summary>
        public ListaPeso<Recurso> Reqs = new ListaPeso<Recurso>();

        /// <summary>
        /// Población productiva que requiere para entrenar.
        /// </summary>
        public ulong CostePoblación;

		/// <summary>
		/// Representa el coste de espacio de esta unidad en una armada.
		/// </summary>
		public float Peso;
    }
}
