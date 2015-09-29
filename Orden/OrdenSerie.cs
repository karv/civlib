using System.Collections.Generic;
using System;

namespace Civ.Orden
{
	/// <summary>
	/// Representa una serie de órdenes
	/// </summary>
	public class OrdenSerie:Orden
	{
		/// <summary>
		/// La cola de órdenes
		/// </summary>
		/// <value>The cola orden.</value>
		protected Queue<Orden> ColaOrden { get; }

		/// <summary>
		/// La orden actual
		/// </summary>
		public Orden Actual
		{
			get
			{
				return ColaOrden.Peek();
			}
		}

		/// <summary>
		/// Agrega una orden a la lista.
		/// </summary>
		/// <param name="orden">Orden.</param>
		public void Encolar(Orden orden)
		{
			if (orden.Armada != Armada)
				throw new Exception("Encolar órdenes debe ser de la misma armada.");
			ColaOrden.Enqueue(orden);
		}

		public OrdenSerie(Armada armada)
		{
			Armada = armada;
			ColaOrden = new Queue<Orden>();
		}

		/// <summary>
		/// Ejecuta la orden
		/// Devuelve true si la orden ha sido terminada.
		/// </summary>
		/// <param name="t">Tiempo</param>
		public override bool Ejecutar(TimeSpan t)
		{
			bool ret = Actual.Ejecutar(t);
			if (ret)
			{
				AlAcabarUnaOrden?.Invoke(this, null);
				if (ColaOrden.Count > 1)
				{
					ColaOrden.Dequeue();
				}
				else
				{
					AlTerminar?.Invoke(this, null);
					return true;
				}
			}
			return false;
		}

		public event EventHandler AlAcabarUnaOrden;
		public event EventHandler AlTerminar;
	}
}