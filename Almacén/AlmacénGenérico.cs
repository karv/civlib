using System;
using ListasExtra;
using Civ.RAW;

namespace Civ.Almacén
{
	/// <summary>
	/// Un almacén genérico
	/// </summary>
	[Serializable]
	public class AlmacénGenérico : ListaPeso<Recurso>, IAlmacén
	{
		#region General

		/// <summary>
		/// Devuelve la lista de recursos implicados
		/// </summary>
		/// <value>The recursos.</value>
		public System.Collections.Generic.IEnumerable<Recurso> Recursos
		{
			get
			{
				return Keys;
			}
		}

		#endregion

		#region Eventos

		event EventHandler<CambioElementoEventArgs<Recurso, float>> IAlmacénRead.AlCambiar
		{
			add
			{
				AlCambiarValor += value;
			}
			remove
			{
				AlCambiarValor -= value;
			}
		}

		#endregion
	}
}