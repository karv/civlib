using System;
using ListasExtra;
using Civ.Data;

namespace Civ
{
	[Serializable]
	/// <summary>
	/// Un almacén genérico
	/// </summary>
	public class AlmacénGenérico : ListaPeso<Recurso>, IAlmacén
	{
		public System.Collections.Generic.IEnumerable<Recurso> Recursos
		{
			get
			{
				return Keys;
			}
		}

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
	}
}

