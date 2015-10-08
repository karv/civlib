using System;
using ListasExtra;
using System.Collections.Generic;
using Civ.Data;

namespace Civ
{
	/// <summary>
	/// Un almacén para Stacks (Inventory)
	/// </summary>
	public class AlmacénStack : ListaPeso<Recurso>, IAlmacén
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

		public AlmacénStack (Stack stack)
		{
			Stack = stack;
		}

		#endregion

		#region Almacén

		IEnumerable<Recurso> IAlmacénRead.recursos { get { return Keys; } }

		float IAlmacénRead.recurso (Recurso recurso)
		{
			return base [recurso];
		}

		void IAlmacén.SetRecurso (Recurso rec, float val)
		{
			System.Diagnostics.Debug.Assert (val >= 0);
			base [rec] = Math.Min (val, base [rec] + CargaRestante);
		}

		void IAlmacén.ChangeRecurso (Recurso rec, float delta)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}