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
		public Unidad(UnidadRAW uRAW)
		{
			RAW = uRAW;
			Nombre = uRAW.Nombre;
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

		public IPosicion Posición;

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
	}
}
