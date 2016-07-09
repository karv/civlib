using System;

namespace Civ.Global
{
	/// <summary>
	/// Proporciona un método para hacer un tick temporal.
	/// </summary>
	public interface ITickable
	{
		/// <summary>
		/// Ejecuta un tick
		/// </summary>
		/// <param name="t">Lapso del tick</param>
		void Tick (TimeEventArgs t);

		/// <summary>
		/// Ocurre antes del tick
		/// </summary>
		event EventHandler AlTickAntes;

		/// <summary>
		/// Ocurre después del tick
		/// </summary>
		event EventHandler AlTickDespués;
	}
}