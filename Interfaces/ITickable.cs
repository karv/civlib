using System;

namespace Civ
{
	/// <summary>
	/// Proporciona un método para hacer un tick temporal.
	/// </summary>
	public interface ITickable
	{
		void Tick(TimeSpan t);
	}
}

