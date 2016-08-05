using System;
using Civ.ObjetosEstado;

namespace Civ.Almacén
{
	/// <summary>
	/// Un almacén para Stacks (Inventory)
	/// </summary>
	[Serializable]
	public class AlmacénStack : AlmacénGenérico
	{
		#region Internos

		/// <summary>
		/// Devuelve el stack correspondiente
		/// </summary>
		public readonly Stack Stack;

		#endregion

		#region Carga

		/// <summary>
		/// Devuelve la cantidad máxima de peso que puede cargar
		/// </summary>
		/// <value>The max peso.</value>
		public float MaxCarga
		{
			get
			{
				return Stack.Cantidad * Stack.RAW.MaxCarga;
			}
		}

		/// <summary>
		/// Devuelve el peso actual que carga este stack
		/// </summary>
		/// <value>The carca actual.</value>
		public float CargaActual
		{
			get
			{
				return SumaTotal ();
			}
		}

		/// <summary>
		/// Devuelve la carga libre del stack
		/// </summary>
		/// <value>The carga restante.</value>
		public float CargaRestante
		{
			get
			{
				return MaxCarga - CargaActual;
			}
		}

		#endregion

		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Almacén.AlmacénStack"/> class.
		/// </summary>
		/// <param name="stack">Stack.</param>
		public AlmacénStack (Stack stack)
		{
			Stack = stack;
		}

		#endregion

	}
}