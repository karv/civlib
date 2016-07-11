using System;
using Civ.ObjetosEstado;
using System.Collections.Generic;
using System.Text;

namespace Civ.Combate
{
	/// <summary>
	/// Representa un combate: conjunto de batallas que comparten armadas agresoras.
	/// </summary>
	public class AnálisisCombate : IAnálisisCombate
	{
		/// <summary>
		/// Une este análisis con otro.
		/// No modifica el otro.
		/// </summary>
		/// <param name="anal">Anal.</param>
		public void UnirCon (IAnálisisCombate anal)
		{
			foreach (var x in Batallas)
				UnirCon (x);
			Duración += anal.Duración;
		}

		/// <summary>
		/// Ejecuta cada batalla de este combate.
		/// Haciendo que se cause el efecto en los implicados.
		/// </summary>
		public void Ejecutar ()
		{
			foreach (var x in Batallas)
				x.Ejecutar ();
		}

		/// <summary>
		/// Calcula todo el daño directo.
		/// </summary>
		/// <returns>The daño directo.</returns>
		public float TotalDañoDirecto ()
		{
			var ret = 0f;
			foreach (var x in batallas)
				ret += x.DañoDirecto;
			return ret;
		}

		/// <summary>
		/// Calcula todo el daño disperso
		/// </summary>
		/// <returns>The daño disperso.</returns>
		public float TotalDañoDisperso ()
		{
			var ret = 0f;
			foreach (var x in batallas)
				ret += x.DañoDisperso;
			return ret;
		}

		/// <summary>
		/// Calcula el daño total en este combate.
		/// </summary>
		public float TotalDaño ()
		{
			return TotalDañoDirecto () + TotalDañoDisperso ();
		}

		/// <summary>
		/// Devuelve un <see cref="System.String"/> describiendo el combate y sus resultados.
		/// </summary>
		public string Análisis ()
		{
			var ret = new StringBuilder ();
			ret.AppendFormat (
				"({1})\n{0}\n\n({3}){2}\n",
				ArmadaYo,
				ArmadaYo.CivDueño,
				ArmadaOtro,
				ArmadaOtro.CivDueño);
			foreach (var x in batallas)
				ret.AppendLine ("\t" + x.Análisis ());
			ret.AppendLine ("Total\tdirecto " + TotalDañoDirecto ());
			ret.AppendLine ("\tdisperso " + TotalDañoDisperso ());
			ret.AppendLine ("\t\t" + TotalDaño ());
			return ret.ToString ();
		}

		/// <summary>
		/// Una un análisis de batalla a este análisis de combate
		/// </summary>
		/// <param name="btl">Análisis de batalla a unir</param>
		public void UnirCon (IAnálisisBatalla btl)
		{
			foreach (var x in Batallas)
			{
				if (x.EsUnibleCon (btl))
				{
					x.UnirCon (btl);
					return;
				}
			}
			Batallas.Add (btl);
		}

		/// <summary>
		/// Devuelve la duración de la batalla.
		/// </summary>
		/// <value>The duración.</value>
		public TimeSpan Duración { get; private set; }

		/// <summary>
		/// Revisa y devuelve un valor indicando si tiene sentido unir esta instancia con otra dada.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="anal">Anal.</param>
		public bool EsUnibleCon (IAnálisisCombate anal)
		{
			return ArmadaYo == anal.ArmadaYo && ArmadaOtro == anal.ArmadaOtro;
		}

		/// <summary>
		/// Una armadas involucradas
		/// </summary>
		public Armada ArmadaYo { get; }

		/// <summary>
		/// Una armadas involucradas
		/// </summary>
		public Armada ArmadaOtro { get; }


		readonly ICollection<IAnálisisBatalla> batallas = new List<IAnálisisBatalla> ();

		/// <summary>
		/// Gets the batallas.
		/// </summary>
		/// <value>The batallas.</value>
		public ICollection<IAnálisisBatalla> Batallas
		{
			get
			{
				return batallas;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="armada0">Armada0.</param>
		/// <param name="armada1">Armada1.</param>
		/// <param name="duración">Duración.</param>
		public AnálisisCombate (Armada armada0, Armada armada1, TimeSpan duración)
		{
			ArmadaYo = armada0;
			ArmadaOtro = armada1;
			Duración = duración;
		}
	}
}