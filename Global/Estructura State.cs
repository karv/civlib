using System;
using System.Collections.Generic;
using Civ;
using Gráficas;

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
        public Gráfica<IPosición> Topología;

        public g_State()
        {
            Topología = new Gráfica<IPosición>();
            Topología.EsSimétrico = true;
        }
	}
}

