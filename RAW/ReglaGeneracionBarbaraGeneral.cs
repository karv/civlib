﻿using Civ;
using Global;
using System.Collections.Generic;

namespace Civ.Bárbaros
{
	public class ReglaGeneracionBarbaraGeneral:IReglaGeneracion
	{
		/// <summary>
		/// Coeficiente de puntuación máximo para armadas.
		/// ej. si la puntuación promedio del juego es 1000 y el coef es 0.3
		/// Se va a generar una armada de puntuación de a lo más, 
		/// y aproximadamente 300
		/// </summary>
		public float CoefPuntuacion = 0.3f;

		GameState _estado;

		public bool EsPosibleGenerar (GameState estado)
		{
			_estado = estado;
			return true;
		}

		/// <summary>
		/// Genera una armada
		/// </summary>
		/// <returns>The armada.</returns>
		public Armada GenerarArmada ()
		{
			float PuntRestante = CoefPuntuacion * _estado.SumaPuntuacion () / _estado.CivsVivas ().Count;

			var Unidades = new List<IUnidadRAW> (Juego.Data.Unidades);
			var cb = new CivilizacionBárbara ();

			var ppos = new List<Pseudoposición> (_estado.Topología.Nodos);
			Pseudoposición pos = ppos [Juego.Rnd.Next (ppos.Count)];

			var ret = new Armada (cb, pos);

			while (Unidades.Count > 0 && PuntRestante >= 0)
			{
				IUnidadRAW unid = Unidades [Juego.Rnd.Next (Unidades.Count)];
				Unidades.Remove (unid);
				ulong Cant = (ulong)(PuntRestante / unid.Puntuación);
				ret.AgregaUnidad (unid, Cant);
				PuntRestante -= Cant * unid.Puntuación;
			}

			return ret;
		}
	}
}