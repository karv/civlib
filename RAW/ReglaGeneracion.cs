using System;
using System.Collections.Generic;
using Global;

namespace Civ.Bárbaros
{
	/// <summary>
	/// Representa una regla de generación de bárbaros
	/// </summary>
	public class ReglaGeneraciónPuntuación : IReglaGeneración
	{
		/// <summary>
		/// Puntuación mínica para generar armada
		/// </summary>
		public float MinPuntuación;
		/// <summary>
		/// Puntuación máxima para generar armada
		/// </summary>
		public float MaxPuntuación;

		/// <summary>
		/// Armada que podría generar.
		/// </summary>
		public ICollection<Tuple<IUnidadRAW, ulong>> ClaseArmada;
		GameState _estado;

		#region IReglaGeneracion

		/// <summary>
		/// Revisa si se debe generar esta clase de armada
		/// </summary>
		/// <param name="estado">Estado del juego.</param>
		/// <returns><c>true</c>, if posible generar was esed, <c>false</c> otherwise.</returns>
		/// <param name="estado">Estado.</param>
		public bool EsPosibleGenerar (GameState estado)
		{
			float Puntuacion = estado.SumaPuntuacion ();
			_estado = estado;
			return Puntuacion < MaxPuntuación && Puntuacion > MinPuntuación;
		}

		public Armada GenerarArmada ()
		{
			var cb = new CivilizacionBárbara ();

			var ppos = new List<Pseudoposición> (_estado.Topología.Nodos);
			Pseudoposición pos = ppos [Juego.Rnd.Next (ppos.Count)];

			var ret = new Armada (cb, pos);
			foreach (var x in ClaseArmada)
			{
				ret.AgregaUnidad (x.Item1, x.Item2);
			}

			return ret;
		}

		#endregion
	}
}