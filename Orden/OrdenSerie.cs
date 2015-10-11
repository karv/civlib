using System;

namespace Civ.Orden
{
	/// <summary>
	/// Representa una serie de órdenes
	/// </summary>
	public class OrdenSerie:C5.ArrayList<IOrden>, IOrden
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
			if (orden.Armada != Armada)
				throw new Exception ("Encolar órdenes debe ser de la misma armada.");
			Enqueue (orden);
		}

		public OrdenSerie (Armada armada)
			: base ()
		{
			this.Armada = armada;
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
					Dequeue ();
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
		public Armada Armada { get; }

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