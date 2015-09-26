namespace Civ.Orden
{
	public class OrdenRecoger: Orden
	{
		public DropStack StackTarget{ get; set; }

		public readonly Pseudoposicion Origen;
		// Meta órdenes
		Orden _actual;

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Orden.OrdenRecoger"/> class.
		/// </summary>
		/// <param name="origen">Origen de la orden. Al lugar donde va a regresar con los recursos</param>
		public OrdenRecoger(Pseudoposicion origen)
		{
			Origen = origen;
			_actual = new OrdenIr(StackTarget.Posicion());
		}

		/// <summary>
		/// Ejecutar the specified t and armada.
		/// Devuelve true si la orden ha sido terminada.
		/// </summary>
		public override bool Ejecutar(float t, Armada armada)
		{
			bool retOrdenPasada = _actual.Ejecutar(t, armada);

			// Si ya llegó al origen, ya terminó toda la orden.
			if (armada.Posicion.Equals(Origen))
				return true;

			// Si llegó a dónde se encuentran los recursos
			if (retOrdenPasada)
			{
				// Recoger todo lo que se encuentra allá
				foreach (var s in armada.Unidades)
				{
					s.RecogerTodo();
				}
				_actual = new OrdenIr(Origen);
			}

			// Aún no acaba
			return false;
		}
	
	}
}