using System;
using System.Collections.Generic;
using Civ.ObjetosEstado;

namespace Civ.Orden
{
	/// <summary>
	/// Representa una serie de órdenes
	/// </summary>
	[Serializable]
	public class OrdenSerie : List<IOrden>, IOrden
	{
		/// <summary>
		/// La orden actual
		/// </summary>
		public IOrden Actual
		{
			get
			{
				return this [0];
			}
		}

		/// <summary>
		/// Agrega una orden a la lista.
		/// </summary>
		/// <param name="orden">Orden.</param>
		public void Encolar (IOrden orden)
		{
			if (orden.ArmadaEjecutante != ArmadaEjecutante)
				throw new Exception ("Encolar órdenes debe ser de la misma armada.");
			Add (orden);
		}

		public OrdenSerie (Armada armada)
			: base ()
		{
			ArmadaEjecutante = armada;
		}

		/// <summary>
		/// Ejecuta la orden
		/// Devuelve true si la orden ha sido terminada.
		/// </summary>
		/// <param name="t">Tiempo</param>
		public bool Ejecutar (TimeSpan t)
		{
			bool ret = Actual.Ejecutar (t);
			if (ret)
			{
				AlAcabarUnaOrden?.Invoke ();
				if (Count > 1)
				{
					RemoveAt (0);
				}
				else
				{
					AlTerminar?.Invoke ();
					return true;
				}
			}
			return false;
		}

		#region IOrden

		/// <summary>
		/// Devuelve la armada de esta orden
		/// </summary>
		/// <value>The armada.</value>
		public Armada ArmadaEjecutante { get; }

		#endregion

		/// <summary>
		/// Ocurre al acabar una orden de la cola
		/// </summary>
		public event Action AlAcabarUnaOrden;

		/// <summary>
		/// Ocurre al terminar toda la cola.
		/// </summary>
		public event Action AlTerminar;
	}
}