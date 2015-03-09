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
		List<TasaProd> _Salida = new List<TasaProd>();

		/// <summary>
		/// Recursos que produce esta propiedad por turno.
		/// </summary>
		public List<TasaProd> Salida
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
			foreach (TasaProd x in _Salida)
			{
				if (C.Almacén[x.Rec] < x.Max)
				{
					C.Almacén[x.Rec] = Math.Min(C.Almacén[x.Rec] + x.Crec * t, x.Max);
				}
			}
		}

		public override string ToString()
		{
			return Nombre;
		}

		/// <summary>
		/// Es la tasa de producción para cada recurso.
		/// La forma en que se comporta
		/// </summary>
		public struct TasaProd
		{
			/// <summary>
			/// Recurso
			/// </summary>
			public Recurso Rec;
			/// <summary>
			/// Valor máximo
			/// </summary>
			public float Max;
			/// <summary>
			/// Crecimiento por chronon
			/// </summary>
			public float Crec;

			public TasaProd(Recurso nRec, float nMax, float nCrec)
			{
				Rec = nRec;
				Max = nMax;
				Crec = nCrec;
			}
		}
	}
}