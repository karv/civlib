using System;

namespace Civ.Global
{
	/// <summary>
	/// Argumentos del evento Tick
	/// </summary>
	[Serializable]
	public class TiempoEventArgs : EventArgs
	{
		/// <summary>
		/// Duración del tick a tiempo usuario
		/// </summary>
		/// <value>The user time.</value>
		public TimeSpan UserTime { get; }

		/// <summary>
		/// Duración del tick a tiempo juego
		/// </summary>
		/// <value>The game time.</value>
		public TimeSpan GameTime { get; }

		/// <param name="userTime">User time.</param>
		/// <param name="gameTime">Game time.</param>
		public TiempoEventArgs (TimeSpan userTime, TimeSpan gameTime)
		{
			UserTime = userTime;
			GameTime = gameTime;
		}

		/// <param name="hoursRealTime">Hours real time.</param>
		/// <param name="accel">Accel.</param>
		public TiempoEventArgs (double hoursRealTime, double accel)
		{
			UserTime = TimeSpan.FromHours (hoursRealTime);
			GameTime = TimeSpan.FromHours (hoursRealTime * accel);
		}
	}
}