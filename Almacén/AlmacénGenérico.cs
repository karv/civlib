using System;
using ListasExtra;
using Civ.RAW;

namespace Civ.Almacén
{
	[Serializable]
	/// <summary>
	/// Un almacén genérico
	/// </summary>
	public class AlmacénGenérico : ListaPeso<Recurso>, IAlmacén
	{
		#region General

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