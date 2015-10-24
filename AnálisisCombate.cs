using System;
using Global;
using System.Collections.Generic;

namespace Civ.Combate
{
	public class AnálisisCombate : IAnálisisCombate
	{
		static Random _r
		{
			get
			{
				return Juego.Rnd;
			}
		}

		public IAtacante Atacante { get; set; }

		public Stack Defensor { get; set; }

		public IEnumerable<string> Mods { get; }

		IAtacante IAnálisisCombate.Atacante
		{
			get
			{
				return Atacante;
			}
		}

		Stack IAnálisisCombate.Defensor
		{
			get
			{
				return Defensor;
			}
		}

		public float DañoDisperso { get; set; }

		public float DañoDirecto { get; set; }

		public TimeSpan Tiempo { get; }

		public string Análisis ()
		{
			return ToString ();
		}

		public override string ToString ()
		{
			return string.Format (
				"[AnálisisCombate: Atacante={0}, Defensor={1}, Mods={2}, DañoDisperso={3}, DañoDirecto={4}]",
				Atacante,
				Defensor,
				Mods,
				DañoDisperso,
				DañoDirecto);
		}


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

		public AnálisisCombate (IAtacante atacante, IDefensor defensa, TimeSpan t)
		{
			Defensor = defensa.Defensa (atacante);
			Atacante = atacante;
			Tiempo = t;

			var Daño = Atacante.ProponerDaño (Defensor.RAW) * (float)t.TotalHours;

			DañoDirecto = Daño * (1 - Dispersión);
			DañoDisperso = Daño * Dispersión;


		}

		/// <summary>
		/// Ejecuta este combate
		/// </summary>
		public void Ejecutar ()
		{
			Dañar ();
		}

		public float Dispersión
		{
			get
			{
				return Atacante.Dispersión;
			}
		}

	}
}

