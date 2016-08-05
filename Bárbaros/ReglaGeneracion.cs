using System;
using System.Collections.Generic;
using Civ.Global;
using Graficas.Continuo;
using Civ.ObjetosEstado;
using Civ.Topología;
using Civ.RAW;
using System.Diagnostics;

namespace Civ.Bárbaros
{
	/// <summary>
	/// Representa una regla de generación de bárbaros
	/// </summary>
	[Obsolete ("Usar ReglaGeneraciónBárbaraGeneral")]
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
		public ICollection<Tuple<IUnidadRAW, long>> ClaseArmada;
		GameState _estado;

		#region IReglaGeneracion

		/// <summary>
		/// Revisa si se debe generar esta clase de armada
		/// </summary>
		/// <param name="estado">Estado del juego.</param>
		/// <returns><c>true</c>, if posible generar was esed, <c>false</c> otherwise.</returns>
		public bool EsPosibleGenerar (GameState estado)
		{
			float Puntuacion = estado.SumaPuntuacion ();
			_estado = estado;
			return Puntuacion < MaxPuntuación && Puntuacion > MinPuntuación;
		}

		/// <summary>
		/// Genera una armada
		/// </summary>
		/// <returns>La armada gnerada</returns>
		public Armada GenerarArmada ()
		{
			var ppos = new List<Terreno> (_estado.Topología.Nodos);
			var pos = ppos [HerrGlobal.Rnd.Next (ppos.Count)];
			var pto = new Punto<Terreno> (
				          Juego.State.Mapa,
				          pos);
			
			return GenerarArmada (new Pseudoposición (pto));
		}

		/// <summary>
		/// Genera una armada en una posición específica
		/// </summary>
		/// <returns>The armada.</returns>
		/// <param name="pos">Position.</param>
		public Armada GenerarArmada (Pseudoposición pos)
		{
			var cb = new CivilizacionBárbara ();

			var ret = new Armada (cb, pos);
			foreach (var x in ClaseArmada)
				ret.AgregaUnidad (x.Item1, x.Item2);

			#if DEBUG
			Debug.WriteLine (
				"Ha aparecido una armada bárbara en " + ret.Posición,
				"BarbGen");
			Debug.WriteLine ("Unidades");
			foreach (var x in ret.Unidades)
				Debug.WriteLine (x);
			Debug.WriteLine (string.Format (
				"Peso: {0}; Velocidad: {1}",
				ret.Peso,
				ret.Velocidad));
			#endif
			return ret;
		}

		#endregion
	}
}