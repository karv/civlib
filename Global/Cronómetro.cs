using System;

namespace Civ.Global
{
	/// <summary>
	/// Un cronómetro para invocar eventos cada cierto tiempo
	/// (indep. CoefVelocidad)
	/// </summary>
	public class Cronómetro : ITickable
	{
		/// <summary>
		/// Devuelve la fecha en la que ocurrió el último tick
		/// </summary>
		public DateTime ÚltimoTick { get; protected set; }

		/// <summary>
		/// Devuelve o establece la frecuencia de invocación
		/// </summary>
		/// <value>The intervalo.</value>
		public TimeSpan Intervalo { get; set; }

		bool _habilitado;

		/// <summary>
		/// Devuelve o establece si este control está habilitado
		/// </summary>
		public bool Habilitado
		{
			get
			{
				return _habilitado;
			}
			set
			{
				ÚltimoTick = DateTime.Now;
				_habilitado = value;
			}
		}

		/// <summary>
		/// Devuelve o establece si el control reiniciará al ejecutar el evento.
		/// </summary>
		public bool Recurrente { get; set; }

		public Cronómetro ()
		{
			Reestablecer ();
		}

		/// <param name="intervalo">Intervalo.</param>
		public Cronómetro (TimeSpan intervalo)
		{
			Reestablecer ();
			Intervalo = intervalo;
		}

		public void Reestablecer ()
		{
			ÚltimoTick = DateTime.Now;
		}

		/// <summary>
		/// Ejecuta un tick
		/// </summary>
		/// <param name="t">Lapso del tick</param>
		public void Tick (TimeSpan t)
		{
			AlTickAntes?.Invoke ();
			if (_habilitado && ÚltimoTick + Intervalo >= DateTime.Now)
			{
				AlLlamar?.Invoke ();
				if (Recurrente)
					Reestablecer ();
				else
					Habilitado = false;
			}
			AlTickDespués?.Invoke ();
		}

		/// <summary>
		/// Ocurre cuando transcurre el tiempo
		/// </summary>
		public event Action AlLlamar;

		/// <summary>
		/// Ocurre antes del tick
		/// </summary>
		public event Action<TimeSpan> AlTickAntes;

		/// <summary>
		/// Ocurre después del tick
		/// </summary>
		public event Action<TimeSpan> AlTickDespués;

	}
}