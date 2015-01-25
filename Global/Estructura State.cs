using System;
using System.Collections.Generic;
using Civ;
using Graficas;

namespace Global
{
	/// <summary>
	/// Representa el estado de un juego.
	/// </summary>
	public class g_State
	{
        /// <summary>
        /// La topología del mundo.
        /// </summary>
        public Grafica<IPosicion> Topologia;

        public g_State()
        {
            Topologia = new Grafica<IPosicion>();
            Topologia.EsSimetrico = true;
        }
	}
}

