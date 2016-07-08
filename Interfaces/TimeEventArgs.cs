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
	
}