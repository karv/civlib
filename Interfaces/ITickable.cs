using System;

namespace Civ
{
	[Serializable]
	public class TimeEventArgs : EventArgs
	{
		public TimeSpan Time { get; }

		public TimeEventArgs (TimeSpan time)
		{
			Time = time;
		}
	}

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
		event EventHandler AlTickAntes;

		/// <summary>
		/// Ocurre después del tick
		/// </summary>
		event EventHandler AlTickDespués;
	}
}