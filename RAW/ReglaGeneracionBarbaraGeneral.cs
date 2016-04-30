using Civ;
using Global;
using System.Collections.Generic;
using System;

namespace Civ.Bárbaros
{
	[Serializable]
	public class ReglaGeneracionBarbaraGeneral : IReglaGeneración
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

			var ppos = new List<Terreno> (_estado.Topología.Nodos);
			var pos = ppos [Juego.Rnd.Next (ppos.Count)];

			var ret = new Armada (cb, pos.Pos);

			while (Unidades.Count > 0 && PuntRestante >= 0)
			{
				var unid = Unidades [Juego.Rnd.Next (Unidades.Count)];
				Unidades.Remove (unid);
				ulong Cant = (ulong)(PuntRestante / unid.Puntuación);
				if (Cant <= 0)
					break;
				ret.AgregaUnidad (unid, Cant);
				PuntRestante -= Cant * unid.Puntuación;
			}

			return ret;
		}
	}
}