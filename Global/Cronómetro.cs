using System;

namespace Civ.Global
{
	/// <summary>
	/// Un cronómetro para invocar eventos cada cierto tiempo
	/// (indep. CoefVelocidad)
	/// </summary>
	public class Cronómetro
	{
		#region Propiedades y campos

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

		/// <summary>
		/// Si el cronómetro mide el tiempo de juego o tiempo real
		/// </summary>
		/// <value><c>true</c> si se mide el tiempo de juego; <c>false</c> si se mide el tiempo real.</value>
		public bool TiempoJuego { get; set; }

		/// <summary>
		/// Si el cronómetro se pausa junto con la pausa del juego
		/// </summary>
		/// <value><c>true</c> si se pausa; otherwise, <c>false</c>.</value>
		public bool SePausa { get; set; }

		/// <summary>
		/// Devuelve la hora y fecha de la próxima vez que se dispare este evento
		/// </summary>
		public DateTime PróximoTick
		{
			get
			{
				return ÚltimoTick + Intervalo;
			}
		}

		#endregion

		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Global.Cronómetro"/> class.
		/// </summary>
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

		#endregion

		#region Métodos

		/// <summary>
		/// Reinicia el contador.
		/// </summary>
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
			AlTickAntes?.Invoke (this, EventArgs.Empty);
			if (_habilitado && PróximoTick < DateTime.Now)
			{
				AlLlamar?.Invoke (this, EventArgs.Empty);
				if (Recurrente)
					Reestablecer ();
				else
					Habilitado = false;
			}
			AlTickDespués?.Invoke (this, EventArgs.Empty);
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre cuando transcurre el tiempo
		/// </summary>
		public event EventHandler AlLlamar;

		/// <summary>
		/// Ocurre antes del tick
		/// </summary>
		public event EventHandler AlTickAntes;

		/// <summary>
		/// Ocurre después del tick
		/// </summary>
		public event EventHandler AlTickDespués;

		#endregion
	}
}