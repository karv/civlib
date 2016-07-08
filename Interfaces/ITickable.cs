using System;

namespace Civ
{
	[Serializable]
	public class TimeEventArgs : EventArgs
	{
		public TimeSpan UserTime { get; }

		public TimeSpan GameTime { get; }

		public TimeEventArgs (TimeSpan userTime, TimeSpan gameTime)
		{
			UserTime = userTime;
			GameTime = gameTime;
		}

		public TimeEventArgs (double hoursRealTime, double accel)
		{
			UserTime = TimeSpan.FromHours (hoursRealTime);
			GameTime = TimeSpan.FromHours (hoursRealTime * accel);
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