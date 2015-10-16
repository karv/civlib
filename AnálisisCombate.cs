using System;
using Global;

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

		public string Análisis ()
		{
			return ToString ();
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
		}


		public AnálisisCombate (IAtacante atacante, IDefensor defensa)
		{
			var def = defensa.Defensa (atacante);
			Atacante = atacante;
			Defensor = def;

			float Daño = Atacante.ProponerDaño (Defensor.RAW);

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

