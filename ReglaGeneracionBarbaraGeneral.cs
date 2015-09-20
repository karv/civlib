using System;
using Civ;
using Global;
using System.Collections.Generic;

namespace Civ.Barbaros
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

		public ReglaGeneracionBarbaraGeneral()
		{
		}

		g_State _estado;

		public bool EsPosibleGenerar(g_State Estado)
		{
			this._estado = Estado;
			return true;
		}

		/// <summary>
		/// Genera una armada
		/// </summary>
		/// <returns>The armada.</returns>
		public Armada GenerarArmada()
		{
			float PuntRestante = CoefPuntuacion * _estado.SumaPuntuacion() / _estado.CivsVivas().Count;

			List<UnidadRAW> Unidades = new List<UnidadRAW>(g_.Data.Unidades);
			CivilizacionBarbara cb = new CivilizacionBarbara();

			List<Pseudoposicion> ppos = new List<Pseudoposicion>(_estado.Topologia.Nodos);
			Pseudoposicion pos = ppos[g_.r.Next(ppos.Count)];

			Armada ret = new Armada(cb, pos, false);

			while (Unidades.Count > 0 || PuntRestante <= 0)
			{
				UnidadRAW unid = Unidades[g_.r.Next(Unidades.Count)];
				Unidades.Remove(unid);
				ulong Cant = (ulong)(PuntRestante / ((IPuntuado)unid).Puntuacion);
				ret.AgregaUnidad(unid, Cant);
				PuntRestante -= Cant * ((IPuntuado)unid).Puntuacion;
			}

			return ret;
		}
	}
}

