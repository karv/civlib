using System;
using System.Collections.Generic;
using Civ;
using Gr�ficas;

namespace Global
{
	/// <summary>
	/// Representa el estado de un juego.
	/// </summary>
	public class g_State
	{
        /// <summary>
        /// La topolog�a del mundo.
        /// </summary>
        public Gr�fica<IPosici�n> Topolog�a;

        public g_State()
        {
            Topolog�a = new Gr�fica<IPosici�n>();
            Topolog�a.EsSim�trico = true;
        }
	}
}

