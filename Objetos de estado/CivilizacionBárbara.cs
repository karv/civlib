using System.Collections.Generic;
using Civ.Data;
using System;

namespace Civ
{
	public class CivilizacionBárbara: ICivilización
	{
		public CivilizacionBárbara ()
		{
			Diplomacia = new DiplomaciaNómada ();
			Armadas = new C5.HashSet<Armada> ();
		}

		float ICivilización.MaxPeso

		{ get { return float.PositiveInfinity; } }

		/// <summary>
		/// Devuelve el modelo diplomático.
		/// </summary>
		public IDiplomacia Diplomacia { get; }

		/// <summary>
		/// Nombre de la civilización
		/// </summary>
		/// <value>The nombre.</value>
		public string Nombre
		{
			get
			{
				return "Bárbaros";
			}
		}

		/// <summary>
		/// Cuenta el número de edificios existentes en alguna ciudad de esta civilización
		/// </summary>
		/// <returns>The edificios.</returns>
		/// <param name="edif">Edif.</param>
		public int CuentaEdificios (EdificioRAW edif)
		{
			return 0;
		}

		/// <summary>
		/// Devuelve una lista con las ciudades de la civilización
		/// </summary>
		/// <value>The ciudades.</value>
		public IList<ICiudad> Ciudades
		{
			get
			{
				return new Ciudad[0];
			}
		}

		/// <summary>
		/// Devuelve una colección con las armadas
		/// </summary>
		/// <value>The armadas.</value>
		public ICollection<Armada> Armadas { get; }

		public ICollection<Ciencia> Avances
		{
			get
			{
				return new Ciencia[0];
			}
		}

		void ICivilización.AgregaMensaje (IU.Mensaje mensaje)
		{
		}

		void ITickable.Tick (TimeSpan t)
		{
			AlTickAntes?.Invoke (t);
			AlTickDespués?.Invoke (t);
		}

		float IPuntuado.Puntuación
		{
			get
			{
				return 0;
			}
		}

		public AlmacénCiv Almacén
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Ocurre antes del tick
		/// </summary>
		public event Action<TimeSpan> AlTickAntes;

		/// <summary>
		/// Ocurre después del tick
		/// </summary>
		public event Action<TimeSpan> AlTickDespués;
	}
}