using System.Collections.Generic;
using System;
using Civ.Almacén;
using Civ.RAW;
using Civ.Ciencias;
using Civ.Global;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Representa una civilización bárbara;
	/// sin cultura, producción ni diplimacia.
	/// </summary>
	[Serializable]
	public class CivilizacionBárbara : ICivilización
	{
		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.ObjetosEstado.CivilizacionBárbara"/> class.
		/// </summary>
		public CivilizacionBárbara ()
		{
			Diplomacia = new DiplomaciaNómada ();
			Armadas = new HashSet<Armada> ();
		}

		#endregion

		#region Diplomático

		/// <summary>
		/// Devuelve si esta civilización está marcada como bárbara
		/// </summary>
		/// <value><c>true</c> if es bárbaro; otherwise, <c>false</c>.</value>
		public bool EsBárbaro
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Devuelve el modelo diplomático.
		/// </summary>
		public IDiplomacia Diplomacia { get; }

		#endregion

		#region Militar

		float ICivilización.MaxPeso	{ get { return float.PositiveInfinity; } }

		#endregion

		#region General

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

		#endregion

		#region Encontrar y contar

		/// <summary>
		/// Cuenta el número de edificios existentes en alguna ciudad de esta civilización
		/// </summary>
		/// <returns>The edificios.</returns>
		/// <param name="edif">Edif.</param>
		public int CuentaEdificios (EdificioRAW edif)
		{
			return 0;
		}

		#endregion

		#region Implementación ICivilización

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

		/// <summary>
		/// Devuelve los avances científicos/culturales que posee la civilización.
		/// Como es bárbara, devuelve una colección vacía.
		/// </summary>
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

			foreach (var x in new List<Armada> (Armadas))
			{
				x.Tick (t);
				if (x.Peso > 63253)
				{
					Console.WriteLine ("Wat?");
				}
			}
			AlTickDespués?.Invoke (t);
		}

		float IPuntuado.Puntuación
		{
			get
			{
				return 0;
			}
		}

		/// <summary>
		/// Devuelve el almacén global.
		/// Como es bárbara devuelve <c>null</c>
		/// </summary>
		public AlmacénCiv Almacén
		{
			get
			{
				return null;
			}
		}

		/// <summary>
		/// Elimina esta civilización del juego
		/// </summary>
		public void Destruirse ()
		{
			Juego.State.Civs.Remove (this);
			foreach (var x in Armadas)
				((IDisposable)x.Posición).Dispose ();
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre antes del tick
		/// </summary>
		public event Action<TimeSpan> AlTickAntes;

		/// <summary>
		/// Ocurre después del tick
		/// </summary>
		public event Action<TimeSpan> AlTickDespués;

		#endregion
	}
}