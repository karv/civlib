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
        public List<Basic.Par<string, float>> Mods;
        
        public float Mod (string Entrada)
        {
            foreach (var x in Mods)
            {
                if (x.x == Entrada) return x.y;
            }
            return 0;
        }

        /// <summary>
        /// Fuerza de la unidad.
        /// </summary>
        public float Fuerza;

        /// <summary>
        /// Flags.
        /// </summary>
        public List<string> Flags;

        // Reqs
        /// <summary>
        /// Requerimientos para crearse.
        /// </summary>
        public List<Basic.Par<string, float>> Reqs;

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
