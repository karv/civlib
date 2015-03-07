using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Civ
{
	/// <summary>
	/// Representa una propiedad innata de un edificio.
	/// </summary>
	[DataContract]
	public class Propiedad : IRequerimiento
	{
		public Propiedad()
		{
		}

		/// <summary>
		/// Nombre de la propiedad.
		/// </summary>
		[DataMember]
		public string Nombre;
		[DataMember(Name = "Salida")]
		ListasExtra.ListaPeso<Recurso> _Salida = new ListasExtra.ListaPeso<Recurso>();

		/// <summary>
		/// Recursos que produce esta propiedad por turno.
		/// </summary>
		public ListasExtra.ListaPeso<Recurso> Salida
		{
			get { return _Salida; }
		}
		// IRequerimiento:
		bool IRequerimiento.LoSatisface(Ciudad C)
		{
			return C.ExistePropiedad(this);
		}

		/// <summary>
		/// El tick de este edificio sobre una ciudad.
		/// </summary>
		/// <param name="C"><see cref="Civ.Ciudad"/> donde hará un tick esta propiedad.</param>
		public virtual void Tick(Ciudad C, float t = 1)
		{
			foreach (Recurso x in _Salida.Keys)
			{
				C.Almacén[x] += _Salida[x] * t;
			}
		}

		public override string ToString()
		{
			return Nombre;
		}
	}
}