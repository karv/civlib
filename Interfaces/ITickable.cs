using System;

namespace Civ
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
		void Tick (TimeSpan t);

		/// <summary>
		/// Ocurre antes del tick
		/// </summary>
		event Action<TimeSpan> AlTickAntes;

		/// <summary>
		/// Ocurre después del tick
		/// </summary>
		event Action<TimeSpan> AlTickDespués;
	}
}

