using Civ.Global;
using System.Collections.Generic;
using System;
using Civ.ObjetosEstado;
using Civ.Topología;
using Civ.RAW;

namespace Civ.Bárbaros
{
	/// <summary>
	/// Un algoritmo de generación de bárbaros basado en puntuación
	/// </summary>
	[Serializable]
	public class ReglaGeneracionBarbaraGeneral : IReglaGeneración
	{
		#region Opciones

		/// <summary>
		/// Coeficiente de puntuación máximo para armadas.
		/// ej. si la puntuación promedio del juego es 1000 y el coef es 0.3
		/// Se va a generar una armada de puntuación de a lo más, 
		/// y aproximadamente 300
		/// </summary>
		public float CoefPuntuacion = 0.3f;
		/// 

		#endregion

		#region Interno

		GameState _estado;

		#endregion

		#region Generación

		/// <summary>
		/// Revisa si se debe generar esta clase de armada y debe ser considerado como candidato a regla
		/// </summary>
		/// <param name="estado">Estado del juego.</param>
		public bool EsPosibleGenerar (GameState estado)
		{
			_estado = estado;
			return true;
		}

		/// <summary>
		/// Genera una armada en una posición específica
		/// </summary>
		/// <returns>The armada.</returns>
		/// <param name="pos">Position.</param>
		public Armada GenerarArmada (Pseudoposición pos)
		{
			float PuntRestante = CoefPuntuacion * _estado.SumaPuntuacion () / _estado.CivsVivas ().Count;

			var Unidades = new List<IUnidadRAW> (Juego.Data.Unidades);
			var cb = new CivilizacionBárbara ();

			var ret = new Armada (cb, pos);

			while (Unidades.Count > 0 && PuntRestante >= 0)
			{
				var unid = Unidades [HerrGlobal.Rnd.Next (Unidades.Count)];
				Unidades.Remove (unid);
				ulong Cant = (ulong)(PuntRestante / unid.Puntuación);
				if (Cant <= 0)
					break;
				ret.AgregaUnidad (unid, Cant);
				PuntRestante -= Cant * unid.Puntuación;
			}

			if (ret.Peso == 0)
			{
				ret.Eliminar ();
				return null;
			}
			Juego.Instancia.GState.Civs.Add (cb);

			return ret;
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
			var pos = ppos [HerrGlobal.Rnd.Next (ppos.Count)];

			var ret = new Armada (cb, pos.Pos);

			while (Unidades.Count > 0 && PuntRestante >= 0)
			{
				var unid = Unidades [HerrGlobal.Rnd.Next (Unidades.Count)];
				Unidades.Remove (unid);
				ulong Cant = (ulong)(PuntRestante / unid.Puntuación);
				if (Cant <= 0)
					break;
				ret.AgregaUnidad (unid, Cant);
				PuntRestante -= Cant * unid.Puntuación;
			}

			if (ret.Peso == 0)
			{
				ret.Eliminar ();
				return null;
			}
			Juego.Instancia.GState.Civs.Add (cb);

			return ret;
		}

		#endregion
	}
}