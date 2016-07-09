using System.Collections.Generic;
using System;
using Civ.Almacén;
using Civ.RAW;
using Civ.Ciencias;
using Civ.Global;
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
		#region ArmadaList

		[Serializable]
		class ArmadaSet : IList<Armada>
		{
			Armada Arm;

			public int IndexOf (Armada item)
			{
				return item.Equals (Arm) ? 0 : -1;
			}

			public void Insert (int index, Armada item)
			{
				Arm = item;
			}

			public void RemoveAt (int index)
			{
				Arm = null;
			}

			public void Add (Armada item)
			{
				Arm = item;
			}

			public void Clear ()
			{
				Arm = null;
			}

			public bool Contains (Armada item)
			{
				return Arm.Equals (item);
			}

			public void CopyTo (Armada [] array, int arrayIndex)
			{
				throw new NotImplementedException ();
			}

			public bool Remove (Armada item)
			{
				if (Arm?.Equals (item) ?? false)
				{
					Arm = null;
					return true;
				}
				return false;
			}

			public IEnumerator<Armada> GetEnumerator ()
			{
				if (Arm != null)
					yield return Arm;
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
			{
				if (Arm != null)
					yield return Arm;
			}

			public Armada this [int index]
			{
				get
				{
					return Arm;
				}
				set
				{
					Arm = value;
				}
			}

			public int Count
			{
				get
				{
					return Arm == null ? 0 : 1;
				}
			}

			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}
		}

		#endregion

		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.ObjetosEstado.CivilizacionBárbara"/> class.
		/// </summary>
		public CivilizacionBárbara ()
		{
			Armadas = new ArmadaSet ();
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
				return "Bárbaros" + _id;
			}
		}

		readonly int _id = HerrGlobal.Rnd.Next (1000);

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Civ.ObjetosEstado.CivilizacionBárbara"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Civ.ObjetosEstado.CivilizacionBárbara"/>.</returns>
		public override string ToString ()
		{
			return Nombre;
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="Civ.ObjetosEstado.CivilizacionBárbara"/> object.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode ()
		{
			return _id;
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
		public Armada Armada
		{
			get
			{
				return Armadas [0];
			}
			set
			{
				Armadas [0] = value;
			}
		}

		ArmadaSet Armadas;

		IList<Armada> ICivilización.Armadas
		{
			get
			{
				return Armadas;
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

			Armada?.Tick (t);
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
			if (Armada != null)
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