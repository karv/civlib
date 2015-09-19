using System;
using ListasExtra;
using Basic;
using System.Collections.Generic;
using Global;
using System.Diagnostics;

namespace Civ
{
	/// <summary>
	/// Representa a una instancia de unidad.
	/// </summary>
	public class Stack
	{
		#region General

		/// <summary>
		/// La clase a la que pertenece esta unidad.
		/// </summary>
		public readonly UnidadRAW RAW;
		ulong _cantidad;

		public float Vitalidad
		{
			get
			{
				return _cantidad * _HP * RAW.Fuerza;
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
				return RAW.Fuerza * (1 + Entrenamiento);
			}
		}

		public ulong Cantidad
		{
			get
			{
				return _cantidad;
			}
			set
			{
				_cantidad = Math.Max(0, value);
				if (_cantidad == 0)
					AbandonaArmada();
				//AlMorir.Invoke(this, null);
			}
		}

		public override string ToString()
		{
			return Nombre;
		}

		#endregion

		#region Eventos

		public event EventHandler AlMorir;

		#endregion

		#region ctor

		/// <summary>
		/// Crea una instancia.
		/// </summary>
		/// <param name="uRAW">El RAW que tendrá esta unidad.</param>
		/// <param name="A">Armada a la que pertenece esta unidad.</param>
		public Stack(UnidadRAW uRAW, ulong cantidad, Armada A)
		{
			RAW = uRAW;
			Nombre = uRAW.Nombre;
			_HP = 1;
			this._cantidad = cantidad;
			_ArmadaPerteneciente = A;
		}

		/// <summary>
		/// Crea una instancia.
		/// </summary>
		/// <param name="uRAW">El RAW que tendrá esta unidad.</param>
		/// <param name="C">Ciudad donde se creará a esta unidad.</param>
		public Stack(UnidadRAW uRAW, ulong cantidad, Ciudad C) : this(uRAW, cantidad, C.Defensa)
		{
		}

		#endregion

		#region Estado inherente

		/// <summary>
		/// Devuelve o establece el nombre de esta unidad.
		/// </summary>
		public string Nombre;
		float _Entrenamiento;

		/// <summary>
		/// Devuelve o establece el nivel de entrenamiento de esta unidad.
		/// Es un valor en [0, 1].
		/// </summary>
		public float Entrenamiento
		{
			get { return _Entrenamiento; }
			set { _Entrenamiento = Math.Max(Math.Min(1, value), 0); }
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
				_HP = Math.Max(Math.Min(1, value), 0);
				if (_HP == 0)		// Si HP = 0, la unidad muere.
					AbandonaArmada();
			}
		}

		/// <summary>
		/// Daña este stack 
		/// </summary>
		/// <param name="TotalDaño">Total daño.</param>
		/// <param name="Dispersion">Dispersion. En [0, 1]</param>
		public void Dañar(float TotalDaño, float Dispersion)
		{
			float DañoDirecto = TotalDaño * (1 - Dispersion);
			float DañoDisperso = TotalDaño * Dispersion;

			DañarDisperso(DañoDisperso);
			DañarDirecto(DañoDirecto);
		}

		void DañarDisperso(float Daño)
		{
			HP -= Daño / _cantidad;
		}

		void DañarDirecto(float Daño)
		{
			// Esto se supone que es el piso.
			double MuertosPct = Daño / HP; // Probabilidad de muerte
			ulong Muertos = (ulong)MuertosPct;
			MuertosPct -= Muertos;

			if (g_.r.NextDouble() < MuertosPct)
				Muertos++;

			Cantidad -= Muertos;
		}

		/// <summary>
		/// Devuelve <c>true</c> sólo si esta unidad está muerta.
		/// </summary>
		public bool Muerto
		{
			get
			{
				return HP == 0 || Cantidad == 0;
			}
		}

		/// <summary>
		/// Devuelve la IPosición de esta unidad.
		/// O equivalentemente de la armada a la que pertenece.
		/// </summary>
		/// <value>The posición.</value>
		public Pseudoposicion Posicion
		{
			get
			{
				return ArmadaPerteneciente.Posicion;
			}
		}

		#endregion

		#region Armada

		Armada _ArmadaPerteneciente;

		/// <summary>
		/// Devuelve la armada a la que pertenece esta unidad.
		/// No se use para cambiar de armada. Siempre es mejor hacerlo desde la clase <c>Armada</c>.
		/// </summary>
		/// <value>Si no existe tal armada, devuelve <c>null</c></value>
		public Armada ArmadaPerteneciente
		{
			get
			{
				return _ArmadaPerteneciente;
			}
			set
			{
				if (ArmadaPerteneciente != value)
				{
					AbandonaArmada();
					_ArmadaPerteneciente = value;
				}
			}
		}

		/// <summary>
		/// Abandona la armada.
		/// </summary>
		public void AbandonaArmada()
		{
			if (ArmadaPerteneciente != null)
			{
				ArmadaPerteneciente.QuitarUnidad(this);
			}
		}

		#endregion

		#region Daño

		/// <summary>
		/// Devuelve el daño máximo que haría esta unidad contra U.
		/// </summary>
		/// <param name="U">Unidad con quien comparar</param>
		public float DañoPropuesto(Stack U)
		{
			float ret;
			float mod = 0;
			ret = Fuerza * Cantidad / U.Fuerza;
			foreach (var x in U.RAW.Flags)
			{
				mod += RAW.Mods[x];
			}
			return ret * (1 + mod);
		}

		/// <summary>
		/// Devuelve la unidad de una armada, tal que this propone el menor daño.
		/// </summary>
		public Stack MenorDaño(Armada A)
		{
			float minDaño = float.PositiveInfinity;
			float currDaño;
			Stack ret = null;
			foreach (var x in A.Unidades)
			{
				currDaño = DañoPropuesto(x);
				if (currDaño <= minDaño)
					ret = x;
			}
			Debug.Assert(ret != null);
			return ret;
		}

		/// <summary>
		/// Causa el daño a la unidad U que le corresponde.
		/// </summary>
		/// <param name="arm">Armada a quién dañar</param>
		/// <param name="raw">RAW de la armada a dañar</param>
		/// <param name="Atacante">Quien ataca</param>
		/// <param name="t">Tiempo</param>
		public void CausaDaño(Armada arm, UnidadRAW raw, Stack Atacante, float t)
		{
			Stack U = arm[raw];
			float Daño = DañoPropuesto(U) * t;
			arm.DañarStack(raw, Atacante, -Daño);
		}

		#endregion

		#region Merge/split

		/// <summary>
		/// Une dos stacks del mismo tipo
		/// </summary>
		/// <param name="other">Other.</param>
		public void MergeFrom(Stack other) //TODO actualizar ahora que Cantidad está siendo utilizada
		{
			if (RAW.Equals(other.RAW))
			{
				this._cantidad += other._cantidad;
				other._cantidad = 0;
			}
			else
				throw new Exception("No se pueden unir Stacks de diferente tipo, Use clase Armada.");
		}

		/// <summary>
		/// Une dos Stacks dell mismo tipo en la primera.
		/// </summary>
		/// <param name="left">Left.</param>
		/// <param name="right">Right.</param>
		public static Stack Merge(Stack left, Stack right)
		{
			left.MergeFrom(right);
			return left;
		}

		public float Peso
		{
			get
			{
				return RAW.Peso * _cantidad;
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
				return ArmadaPerteneciente.Orden is Orden.OrdenEstacionado &&
				RAW.PuedeColonizar &&
				Posicion.enTerreno &&
				Posicion.A.CiudadConstruida == null;
			}
		}

		/// <summary>
		/// Coloniza en el terreno actual.
		/// </summary>
		public Ciudad Colonizar()
		{
			if (!PuedeColonizarAqui)
				return null;
			
			Ciudad ret = new Ciudad(ArmadaPerteneciente.CivDueño, 
				             Posicion.A, 
				             RAW.colonizacion.Value.poblacionACiudad * _cantidad);

			// Al usuario
			this.ArmadaPerteneciente.CivDueño.AgregaMensaje("ciudad {0} construida en {1}", ret, ret.Terr);
			// Deshacer el stack
			AbandonaArmada();

			return ret;

		}

		#endregion
	}
}