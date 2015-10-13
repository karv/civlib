using System;
using System.Collections.Generic;
using Global;

namespace Civ.Barbaros
{
	/// <summary>
	/// Representa una regla de generación de bárbaros
	/// </summary>
	public class ReglaGeneracionPuntuacion : IReglaGeneracion
	{
		public ReglaGeneracionPuntuacion()
		{
		}

		public float MinPuntuacion;
		public float MaxPuntuacion;

		public ICollection<Tuple<UnidadRAWCombate, ulong>> ClaseArmada;
		g_State _estado;

		#region IReglaGeneracion

		public bool EsPosibleGenerar(g_State Estado)
		{
			float Puntuacion = Estado.SumaPuntuacion();
			_estado = Estado;
			return Puntuacion < MaxPuntuacion && Puntuacion > MinPuntuacion;
		}

		public Armada GenerarArmada()
		{
			CivilizacionBarbara cb = new CivilizacionBarbara();

			List<Pseudoposicion> ppos = new List<Pseudoposicion>(_estado.Topologia.Nodos);
			Pseudoposicion pos = ppos[g_.r.Next(ppos.Count)];

			Armada ret = new Armada(cb, pos, false);
			foreach (var x in ClaseArmada)
			{
				ret.AgregaUnidad(x.Item1, x.Item2);
			}

			return ret;
		}

		#endregion
	}
}

