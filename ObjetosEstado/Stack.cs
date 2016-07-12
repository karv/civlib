using System;
using Civ.Global;
using Civ.Combate;
using Civ.Almacén;
using Civ.Topología;
using Civ.RAW;
using System.Diagnostics;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Representa a una instancia de unidad.
	/// </summary>
	[Serializable]
	public class Stack : IPuntuado, IAtacante
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
				AlCambiarCantidad?.Invoke (this, EventArgs.Empty);
				if (_cantidad == 0)
					InvocaAlMorir ();
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Civ.ObjetosEstado.Stack"/>.
		/// </summary>
		/// <returns>Nombre de unidad y cantidad.</returns>
		public override string ToString ()
		{
			return string.Format ("({0}) {1}", Cantidad, Nombre);
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

		/// <summary>
		/// desvincula a la armada y reduce sus propiedades a cero.
		/// </summary>
		public void Destruir ()
		{
			ArmadaPerteneciente = null;
			HP = 0;
			Entrenamiento = 0;
			Cantidad = 0;
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
					InvocaAlMorir ();
				}
			}
		}

		/// <summary>
		/// Se debe ejecutar al ser víctima de un ataque.
		/// </summary>
		/// <param name="anal">El análisis de combate del ataque.</param>
		public void FueAtacado (IAnálisisBatalla anal)
		{
			ArmadaPerteneciente.FueAtacado (anal);
			AlSerAtacado?.Invoke (this, new CombateEventArgs (anal));
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

		/// <summary>
		/// Devuelve el peso del stack
		/// </summary>
		/// <value>The peso.</value>
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
			AlColonizar.Invoke (this, new CiudadEventArgs (ret));
			return ret;
		}

		#endregion

		#region Carga

		/// <summary>
		/// Recoge todo (lo que puede) recurso existente como DropStack en esta posición, 
		/// agregándolo a su Carga.
		/// </summary>
		public void RecogerTodo ()
		{
			if (Carga.CargaRestante <= 0)
				return;
			foreach (var x in Juego.State.Drops)
			{
				if (x.Posición.Equals (Posición))
				{
					foreach (var r in x.Recursos)
					{
						if (Carga.CargaRestante <= 0)
							return;
						float Cargar = Math.Min (Carga.CargaRestante, x [r]);
						Carga [r] += Cargar;
					}
				}
			}
		}

		#endregion

		#region IAtacante

		float IAtacante.ProponerDaño (IUnidadRAW unidad)
		{
			
			var cRAW = RAW as IUnidadRAWCombate;
			float ret;
			float mod = 0;
			ret = Fuerza * Cantidad / unidad.Defensa;

			foreach (var y in cRAW.Modificadores)
			{
				if (unidad.TieneFlag (y))
					mod += cRAW.getModificador (y);
			}
			return ret * (1 + mod);
		}



		IAnálisisBatalla IAtacante.CausarDaño (IDefensor Def, TimeSpan t)
		{
			var ret = new AnálisisBatalla (this, Def, t);
			ret.Ejecutar ();
			return ret;
		}

		/// <summary>
		/// Devuelve la dispersión de daño de este Stack,
		/// </summary>
		/// <value>Si tiene sentido, devuelve la Dispersión; si no, devuelve 0</value>
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
		/// Se ejecuta cuando Cantidad se vuelve cero.
		/// Retira este stack de la armada, e invoca el evento AlMorir
		/// </summary>
		protected void InvocaAlMorir ()
		{
			Debug.WriteLine ("Stack muerto: " + ToString (), "Stack");
			AbandonaArmada ();

			AlMorir?.Invoke (this, EventArgs.Empty);
		}

		/// <summary>
		/// Ocurre cuando todo el stack se pierde
		/// </summary>
		public event EventHandler AlMorir;

		/// <summary>
		/// Ocurre cuando una armada ataca este Stack
		/// </summary>
		public event EventHandler<CombateEventArgs> AlSerAtacado;

		/// <summary>
		/// Ocurre cuando hay un cambio en la cantidad
		/// </summary>
		public event EventHandler AlCambiarCantidad;

		/// <summary>
		/// Ocurre al colonizar una nueva ciudad.
		/// Primer parámetro es la ciudad colonizada.
		/// </summary>
		public event EventHandler<CiudadEventArgs> AlColonizar;

		#endregion
	}
}