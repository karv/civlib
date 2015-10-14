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

		/// <summary>
		/// Ocurre cuando cambia el almacén de un recurso
		/// Recurso, valor viejo, valor nuevo
		/// </summary>
		event EventHandler<CambioElementoEventArgs<Recurso, float>> IAlmacénRead.AlCambiar
		{
			add
			{
				AlCambiarValor += value;
			}
			remove
			{
				AlCambiarValor -= value;
			}
		}


		IEnumerable<Recurso> IAlmacénRead.recursos { get { return Keys; } }

		float IAlmacén.this [Recurso recurso]
		{
			get
			{
				return base [recurso];
			}
			set
			{
				base [recurso] = value;
			}
		}

		float IAlmacénRead.this [Recurso recurso]
		{
			get
			{
				return base [recurso];
			}
		}

		#endregion

	}
}