using System.Collections.Generic;
using System;

namespace Civ.Orden
{
	// TODO Probar

	/// <summary>
	/// Representa una serie de órdenes
	/// </summary>
	public class OrdenSerie:Orden
	{
		Queue<Orden> ColaOrden { get; }

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
		/// <param name="armada">Armada</param>
		public override bool Ejecutar(TimeSpan t)
		{
			bool ret = Actual.Ejecutar(t);
			if (ret)
			{
				if (ColaOrden.Count > 0)
					ColaOrden.Dequeue();
				else
				{
					return true;
				}
			}
			return false;
		}
	}
}