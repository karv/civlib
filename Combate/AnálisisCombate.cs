﻿using System;
using Civ.Global;
using Civ.ObjetosEstado;
using System.Diagnostics;
using System.Collections.Generic;

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

		public void Ejecutar ()
		{
			foreach (var x in Batallas)
				x.Ejecutar ();
		}

		public string Análisis ()
		{
			throw new NotImplementedException ();
		}

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

		/// <summary>
		/// Gets the batallas.
		/// </summary>
		/// <value>The batallas.</value>
		public ICollection<IAnálisisBatalla> Batallas
		{
			get
			{
				throw new NotImplementedException ();
			}
		}

		public AnálisisCombate (Armada armada0, Armada armada1, TimeSpan duración)
		{
			ArmadaYo = armada0;
			ArmadaOtro = armada1;
			Duración = duración;
		}
	}

	/// <summary>
	/// Representa los resultados de un tick de combate
	/// </summary>
	[Serializable]
	public class AnálisisBatalla : IAnálisisBatalla
	{
		#region Info

		/// <summary>
		/// Devuelve o establece el atacante.
		/// </summary>
		public IAtacante Atacante { get; set; }

		/// <summary>
		/// Devuelve o establece el defensor.
		/// </summary>
		/// <value>The defensor.</value>
		public Stack Defensor { get; set; }

		#endregion

		#region Análisis

		IAtacante IAnálisisBatalla.Atacante
		{
			get
			{
				return Atacante;
			}
		}

		Stack IAnálisisBatalla.Defensor
		{
			get
			{
				return Defensor;
			}
		}

		/// <summary>
		/// Devuelve el análisis en forma de <see cref="System.String"/>
		/// </summary>
		public string Análisis ()
		{
			return ToString ();
		}

		/// <summary>
		/// Une este análisis con otro.
		/// No modifica el otro.
		/// </summary>
		/// <param name="anal">Anal.</param>
		public void UnirCon (IAnálisisBatalla anal)
		{
			Tiempo += anal.Duración;
			DañoDirecto += anal.DañoDirecto;
			DañoDisperso += anal.DañoDisperso;
		}

		/// <summary>
		/// Revisa y devuelve un valor indicando si tiene sentido unir esta instancia con otra dada.
		/// </summary>
		public bool EsUnibleCon (IAnálisisBatalla anal)
		{
			return Atacante == anal.Atacante && Defensor == anal.Defensor;
		}

		/// <summary>
		/// Devuelve la duración del combate.
		/// </summary>
		public TimeSpan Duración
		{
			get
			{
				return Tiempo;
			}
		}

		#endregion

		#region Contexto

		static Random _r
		{
			get
			{
				return HerrGlobal.Rnd;
			}
		}

		/// <summary>
		/// Devuelve o establece el daño disperso que se causó
		/// </summary>
		/// <value>The daño disperso.</value>
		public float DañoDisperso { get; set; }

		/// <summary>
		/// Devuelve o establece el daño directo que se causó
		/// </summary>
		/// <value>The daño directo.</value>
		public float DañoDirecto { get; set; }

		/// <summary>
		/// Devuelve la duración del tick del combate
		/// </summary>
		/// <value>The tiempo.</value>
		public TimeSpan Tiempo { get; private set; }

		/// <summary>
		/// Devuelve el coeficiente de dispersión del atacante.
		/// </summary>
		public float Dispersión
		{
			get
			{
				return Atacante.Dispersión;
			}
		}

		#endregion

		#region General

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Civ.Combate.AnálisisCombate"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Civ.Combate.AnálisisCombate"/>.</returns>
		public override string ToString ()
		{
			return string.Format (
				"[AnálisisCombate: Atacante={0}, Defensor={1}, DañoDisperso={2}, DañoDirecto={3}]",
				Atacante,
				Defensor,
				DañoDisperso,
				DañoDirecto);
		}

		/// <summary>
		/// Ejecuta este combate
		/// </summary>
		public void Ejecutar ()
		{
			if (Defensor == null)
				return;
			Dañar ();
		}

		#endregion

		#region Interacción y daño

		void DañarDirecto ()
		{
			// Esto se supone que es el piso.
			double MuertosPct = DañoDirecto / Defensor.HP; // Probabilidad de muerte
			ulong Muertos = (ulong)MuertosPct;
			MuertosPct -= Muertos;

			if (_r.NextDouble () < MuertosPct)
				Muertos++;

			Defensor.Cantidad -= Muertos;
		}

		void DañarDisperso ()
		{
			Defensor.HP -= DañoDisperso / Defensor.Cantidad;
		}

		/// <summary>
		/// Daña este stack 
		/// </summary>
		void Dañar ()
		{
			DañarDisperso ();
			DañarDirecto ();
			Defensor.FueAtacado (this);
		}

		#endregion

		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Combate.AnálisisCombate"/> class.
		/// </summary>
		/// <param name="atacante">Atacante.</param>
		/// <param name="defensa">Defensa.</param>
		/// <param name="t">Duración del tick de combate</param>
		public AnálisisBatalla (IAtacante atacante, IDefensor defensa, TimeSpan t)
		{
			Defensor = defensa.Defensa (atacante);
			Atacante = atacante;
			Tiempo = t;

			if (Defensor == null)
			{
				Debug.WriteLine ("Defensor nulo; no hay combate.", "Pelea");
				return;
			}

			var Daño = Atacante.ProponerDaño (Defensor.RAW) * (float)t.TotalHours;

			DañoDirecto = Daño * (1 - Dispersión);
			DañoDisperso = Daño * Dispersión;
		}

		#endregion
	}
}