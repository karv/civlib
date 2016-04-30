using System;

namespace Civ.Orden
{
	[Serializable]
	public class OrdenRecoger: IOrden
	{
		/// <summary>
		/// Devuelve la armada de esta orden
		/// </summary>
		/// <value>The armada.</value>
		public Armada ArmadaEjecutante { get; }

		/// <summary>
		/// Devuelve o establece el stack que quere tomar.
		/// </summary>
		public DropStack StackTarget { get; set; }

		/// <summary>
		/// Devuelve la posición de donde va a dejar el stack
		/// </summary>
		/// <value>The origen.</value>
		public Pseudoposición Origen { get; }

		// Meta órdenes
		IOrden _actual;

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Orden.OrdenRecoger"/> class.
		/// </summary>
		/// <param name="armada">Armada asociada a esta orden</param>
		/// <param name="target">El DropStack que recogerá </param>
		public OrdenRecoger (Armada armada, DropStack target)
		{
			ArmadaEjecutante = armada;
			Origen = ArmadaEjecutante.Posición.Clonar (ArmadaEjecutante.Posición.Objeto);

			StackTarget = target;
			_actual = new OrdenIrALugar (ArmadaEjecutante, StackTarget.Posición);
		}

		/// <summary>
		/// Ejecutar the specified t and armada.
		/// Devuelve true si la orden ha sido terminada.
		/// </summary>
		public bool Ejecutar (TimeSpan t)
		{
			bool retOrdenPasada = _actual.Ejecutar (t);

			// Si ya llegó al origen, ya terminó toda la orden.
			if (ArmadaEjecutante.Posición.Equals (Origen))
			{
				AlRegresar?.Invoke ();
				return true;
			}
			// Si llegó a dónde se encuentran los recursos
			if (retOrdenPasada)
			{
				AlLlegar?.Invoke ();
				// Recoger todo lo que se encuentra allá
				foreach (var s in ArmadaEjecutante.Unidades)
				{
					s.RecogerTodo ();
				}
				_actual = new OrdenIrALugar (ArmadaEjecutante, Origen);
			}

			// Aún no acaba
			return false;
		}

		#region Eventos

		/// <summary>
		/// Ocurre al llegar al DropStack
		/// </summary>
		public event Action AlLlegar;

		/// <summary>
		/// Ocurre al regresar a casa
		/// </summary>
		public event Action AlRegresar;

		#endregion
	}
}