using System;

namespace Civ
{
	/// <summary>
	/// Proporciona un método para hacer un tick temporal.
	/// </summary>
	public interface ITickable
	{
		void Tick(float t);
	}

	public static class ExtITickable
	{
		public static void Tick(this ITickable ticker)
		{
			ticker.Tick(1);
		}
	}
}

