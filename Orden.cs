namespace Civ.Orden
{
	/// <summary>
	/// Representa una orden de una armada
	/// </summary>
	public abstract class Orden
	{
		/// <summary>
		/// Ejecuta la orden
		/// Devuelve true si la orden ha sido terminada.
		/// </summary>
		/// <param name="t">T.</param>
		/// <param name="armada">Armada.</param>
		public abstract bool Ejecutar(float t, Armada armada);
	}
}