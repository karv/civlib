using System;
using Global;
using Civ.Data;
using Civ.Combate;

namespace Civ
{
	/// <summary>
	/// Representa a una instancia de unidad.
	/// </summary>
	public class Stack: IPuntuado, IAtacante
	{
		#region General

		/// <summary>
		/// La clase a la que pertenece esta unidad.
		/// </summary>
		public readonly IUnidadRAW RAW;
		ulong _cantidad;

		/// <summary>
		/// Almacén de carga del stack
		/// </summary>
		public AlmacénStack Carga { get; }

		/// <summary>
		/// Devuelve la fuerza del RAW
		/// </summary>
		/// <value>The RAW fuerza.</value>
		float RAWFuerza
		{
			get
			{
				return (RAW as IUnidadRAWCombate)?.Ataque ?? 0;
			}
		}

		/// <summary>
		/// Una medida de la habilidad actual de pelea
		/// </summary>
		public float Vitalidad
		{
			get
			{
				return _cantidad * _HP * RAWFuerza;
			}
		}

		/// <summary>
		/// Es la fuerza que se usa en el cálculode daño
		/// NO toma en cuenta la cantidad.
		/// </summary>
		/// <value>The fuerza.</value>
		public float Fuerza
		{
			get
			{
				return RAWFuerza * (1 + Entrenamiento);
			}
		}

		/// <summary>
		/// Cantidad de unidades pertenecientes a este stack
		/// </summary>
		public ulong Cantidad
		{
			get
			{
				return _cantidad;
			}
			set
			{
				_cantidad = Math.Max (0, value);
				AlCambiarCantidad?.Invoke ();
				if (_cantidad == 0)
				{
					AbandonaArmada ();
					AlMorir?.Invoke ();
				}
			}
		}

		public override string ToString ()
		{
			return Nombre;
		}

		#endregion

		#region ctor

		/// <summary>
		/// Crea una instancia.
		/// </summary>
		/// <param name="uRAW">El RAW que tendrá esta unidad.</param>
		/// <param name="armada">Armada a la que pertenece esta unidad.</param>
		/// <param name="cantidad">Cantidad de unidades que pertenecen al stack </param>
		public Stack (IUnidadRAW uRAW, ulong cantidad, Armada armada)
		{
			Carga = new AlmacénStack (this);
			RAW = uRAW;
			_HP = 1;
			_cantidad = cantidad;
			_armadaPerteneciente = armada;
		}

		/// <summary>
		/// Crea una instancia.
		/// </summary>
		/// <param name="uRAW">El RAW que tendrá esta unidad.</param>
		/// <param name="ciudad">Ciudad donde se creará a esta unidad.</param>
		/// <param name="cantidad">Cantidad de unidades que pertenecen al stack </param>
		public Stack (IUnidadRAW uRAW, ulong cantidad, ICiudad ciudad)
			: this (uRAW,
			        cantidad,
			        ciudad.Defensa)
		{
		}

		#endregion

		#region Estado inherente

		/// <summary>
		/// Devuelve o establece el nombre de esta unidad.
		/// </summary>
		public string Nombre
		{
			get
			{
				return RAW.Nombre;
			}
		}

		float _Entrenamiento;

		/// <summary>
		/// Devuelve o establece el nivel de entrenamiento de esta unidad.
		/// Es un valor en [0, 1].
		/// </summary>
		public float Entrenamiento
		{
			get { return _Entrenamiento; }
			set { _Entrenamiento = Math.Max (Math.Min (1, value), 0); }
		}

		float _HP;

		/// <summary>
		/// Devuelve o establece la HP actual de la <c>Unidad</c>.
		/// Debe ser un flotante en [0,1]
		/// </summary>
		/// <value></value>
		public float HP
		{
			get
			{
				return _HP;
			}
			set
			{				
				_HP = Math.Max (Math.Min (1, value), 0);
				if (_HP <= 0)		// Si HP = 0, la unidad muere.
				{
					AlMorir?.Invoke ();
					AbandonaArmada ();
				}
			}
		}

		public void FueAtacado (IAnálisisCombate anal)
		{
			AlSerAtacado?.Invoke (anal);
		}

		/// <summary>
		/// Devuelve <c>true</c> sólo si esta unidad está muerta.
		/// </summary>
		public bool Muerto
		{
			get
			{
				return Vitalidad == 0;
			}
		}

		/// <summary>
		/// Devuelve la IPosición de esta unidad.
		/// O equivalentemente de la armada a la que pertenece.
		/// </summary>
		/// <value>The posición.</value>
		public Pseudoposición Posición
		{
			get
			{
				return ArmadaPerteneciente.Posición;
			}
		}

		#endregion

		#region Armada

		Armada _armadaPerteneciente;

		/// <summary>
		/// Devuelve la armada a la que pertenece esta unidad.
		/// No se use para cambiar de armada. Siempre es mejor hacerlo desde la clase <c>Armada</c>.
		/// </summary>
		/// <value>Si no existe tal armada, devuelve <c>null</c></value>
		public Armada ArmadaPerteneciente
		{
			get
			{
				return _armadaPerteneciente;
			}
			set
			{
				if (ArmadaPerteneciente != value)
				{
					AbandonaArmada ();
					_armadaPerteneciente = value;
				}
			}
		}

		/// <summary>
		/// Abandona la armada.
		/// </summary>
		public void AbandonaArmada ()
		{
			if (ArmadaPerteneciente != null)
			{
				ArmadaPerteneciente.QuitarUnidad (this);
			}
		}

		#endregion

		#region Merge/split

		/// <summary>
		/// Une dos stacks del mismo tipo
		/// </summary>
		/// <param name="other">Other.</param>
		public void MergeFrom (Stack other)
		{
			if (RAW.Equals (other.RAW))
			{
				_cantidad += other._cantidad;

				// Desmantelar a other
				// No llamar a AlMorir cuando pasa esto
				other._cantidad = 0;
				//other.RAW = null;
			}
			else
				throw new Exception ("No se pueden unir Stacks de diferente tipo, Use clase Armada.");
		}

		/// <summary>
		/// Une dos Stacks dell mismo tipo en la primera.
		/// </summary>
		/// <param name="left">Left.</param>
		/// <param name="right">Right.</param>
		public static Stack Merge (Stack left, Stack right)
		{
			left.MergeFrom (right);
			return left;
		}

		public float Peso
		{
			get
			{
				return RAW.Peso * Cantidad;
			}
		}

		#endregion

		#region IPuntuado

		float IPuntuado.Puntuación
		{ 
			get
			{ 
				return RAW.Puntuación * Cantidad; 
			} 
		}

		#endregion

		#region Settler

		/// <summary>
		/// Revisa si esta unidad puede colonizar en este terreno
		/// </summary>
		/// <value><c>true</c> if puede colonizar aqui; otherwise, <c>false</c>.</value>
		public bool PuedeColonizarAqui
		{
			get
			{
				var col = RAW as IUnidadRAWColoniza;
				if (col != null)
				{
					return (ArmadaPerteneciente.Orden is Orden.OrdenEstacionado) &&
					(col.PuedeColonizar (this));
				}
				return false;
			}
		}

		/// <summary>
		/// Coloniza en el terreno actual.
		/// </summary>
		public ICiudad Colonizar ()
		{
			var ret = (RAW as IUnidadRAWColoniza)?.Coloniza (this);
			AlColonizar.Invoke (ret);
			return ret;
		}

		#endregion

		#region Carga

		public void RecogerTodo ()
		{
			if (Carga.CargaRestante <= 0)
				return;
			foreach (var x in Juego.State.Drops)
			{
				if (x.Posición.Equals (Posición))
				{
					foreach (var r in x.Almacén.Keys)
					{
						if (Carga.CargaRestante <= 0)
							return;
						float Cargar = Math.Min (Carga.CargaRestante, x.Almacén [r]);
						Carga [r] += Cargar;
					}
				}
			}
		}

		#endregion

		#region IAtacante

		float IAtacante.ProponerDaño (IUnidadRAW Unidad)
		{
			
			var cRAW = RAW as IUnidadRAWCombate;
			float ret;
			float mod = 0;
			ret = Fuerza * Cantidad / Unidad.Defensa;

			foreach (var y in cRAW.Modificadores)
			{
				if (Unidad.TieneFlag (y))
					mod += cRAW.getModificador (y);
			}
			return ret * (1 + mod);
		}



		IAnálisisCombate IAtacante.CausarDaño (IDefensor Def, TimeSpan t)
		{
			var ret = new AnálisisCombate (this, Def, t);
			ret.Ejecutar ();
			return ret;
		}

		public float Dispersión
		{
			get
			{
				var cbt = RAW as IUnidadRAWCombate;
				return cbt?.Dispersión ?? 0;
			}
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre cuando todo el stack se pierde
		/// </summary>
		public event Action AlMorir;

		/// <summary>
		/// Ocurre cuando una armada ataca este Stack
		/// </summary>
		public event Action<IAnálisisCombate> AlSerAtacado;

		/// <summary>
		/// Ocurre cuando hay un cambio en la cantidad
		/// </summary>
		public event Action AlCambiarCantidad;

		public event Action<ICiudad> AlColonizar;

		#endregion

	}
}