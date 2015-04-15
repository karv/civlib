using System;
using ListasExtra;
using Basic;
using System.Collections.Generic;

namespace Civ
{
	/// <summary>
	/// Representa a una instancia de unidad.
	/// </summary>
	public class Unidad
	{
		/// <summary>
		/// La clase a la que pertenece esta unidad.
		/// </summary>
		public readonly UnidadRAW RAW;

		/// <summary>
		/// Crea una instancia.
		/// </summary>
		/// <param name="uRAW">El RAW que tendrá esta unidad.</param>
		/// <param name="A">Armada a la que pertenece esta unidad.</param>
		public Unidad(UnidadRAW uRAW, Armada A)
		{
			RAW = uRAW;
			Nombre = uRAW.Nombre;
			_HP = 1;
			_ArmadaPerteneciente = A;
		}

		/// <summary>
		/// Crea una instancia.
		/// </summary>
		/// <param name="uRAW">El RAW que tendrá esta unidad.</param>
		/// <param name="C">Ciudad donde se creará a esta unidad.</param>
		public Unidad(UnidadRAW uRAW, Ciudad C):this(uRAW, C.Defensa)
		{
		}

		public override string ToString()
		{
			return Nombre;
		}

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
		/// Devuelve el peso, relativo a Armada, de la unidad.
		/// </summary>
		/// <value>The peso.</value>
		public float Peso
		{
			get
			{
				return RAW.Peso;
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

		/// <summary>
		/// Devuelve el daño máximo que haría esta unidad contra U.
		/// </summary>
		/// <param name="U">Unidad con quien comparar</param>
		public float DañoPropuesto(Unidad U)
		{
			float ret;
			float mod = 0;
			ret = RAW.Fuerza * HP / U.RAW.Fuerza;
			foreach (var x in U.RAW.Flags)
			{
				mod += RAW.Mods[x];
			}
			return ret * (1 + mod);
		}

		/// <summary>
		/// Devuelve la unidad de una armada, tal que this propone el menor daño.
		/// </summary>
		public Unidad MenorDaño(Armada A)
		{
			float minDaño = float.PositiveInfinity;
			float currDaño;
			Unidad ret = null;
			foreach (var x in A.Unidades)
			{
				currDaño = DañoPropuesto(x);
				if (currDaño < minDaño)
					ret = x;
			}
			return ret;
		}

		/// <summary>
		/// Causa el daño a la unidad U que le corresponde.
		/// </summary>
		/// <param name="U">Unidad a quien dañar</param>
		/// <param name="t">Tiempo</param>
		public void CausaDaño(Unidad U, float t)
		{
			float Daño = DañoPropuesto(U) * t;
			U.HP -= Daño;
		}

		/// <summary>
		/// Devuelve <c>true</c> sólo si esta unidad está muerta.
		/// </summary>
		public bool Muerto
		{
			get
			{
				return HP == 0;
			}
		}
	}
}