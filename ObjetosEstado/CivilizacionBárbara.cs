﻿using System.Collections.Generic;
using System;
using Civ.Almacén;
using Civ.RAW;
using Civ.Ciencias;
using Civ.Global;
using System.Diagnostics;
using System.Linq;

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
		/// Devuelve o establece la armada de estos bárbaros
		/// </summary>
		/// <value>The armada.</value>
		public Armada Armada { get; set; }

		IList<Armada> ICivilización.Armadas
		{
			get
			{
				Armada [] ret = { Armada };
				return ret;
			}
		}

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

		void ICivilización.AgregaMensaje (IU.IMensaje mensaje)
		{
		}

		void ITickable.Tick (TimeEventArgs t)
		{
			AlTickAntes?.Invoke (this, t);

			Armada.Tick (t);
			#if DEBUG
			if (Armada.Peso > 63253)
				Debug.WriteLine ("Civ bárbara extra fuerte", "Bárbaro enloquecido");
			#endif
			AlTickDespués?.Invoke (this, t);
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
			((IDisposable)Armada).Dispose ();
		}

		/// <summary>
		/// Revisa si esta civilización está en realidad muerta
		/// </summary>
		/// <returns><c>true</c>, if destruirme was deboed, <c>false</c> otherwise.</returns>
		public bool DeboDestruirme ()
		{
			return Armada?.Unidades.Any () ?? true;
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre antes del tick
		/// </summary>
		public event EventHandler AlTickAntes;

		/// <summary>
		/// Ocurre después del tick
		/// </summary>
		public event EventHandler AlTickDespués;

		#endregion
	}
}