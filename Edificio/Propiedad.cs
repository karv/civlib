using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Civ
{
	/// <summary>
	/// Representa una propiedad innata de un edificio.
	/// </summary>
	[DataContract]
	public class Propiedad : IRequerimiento<Ciudad>, CivLibrary.Debug.IPlainSerializable
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
		bool IRequerimiento<Ciudad>.LoSatisface(Ciudad C)
		{
			return C.ExistePropiedad(this);
		}

		/// <summary>
		/// El tick de este edificio sobre una ciudad.
		/// </summary>
		/// <param name="C"><see cref="Civ.Ciudad"/> donde hará un tick esta propiedad.</param>
		public virtual void Tick(IAlmacenante C, float t = 1)
		{
			foreach (TasaProd x in _Salida)
			{
				if (C.Almacen[x.Rec] < x.Max)
				{
					C.Almacen[x.Rec] = Math.Min(C.Almacen[x.Rec] + x.Crec * t, x.Max);
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

		string CivLibrary.Debug.IPlainSerializable.PlainSerialize(int tabs)
		{
			string tab = "";
			string ret;
			for (int i = 0; i < tabs; i++)
			{
				tab += "\t";
			}
			ret = tab + "(Propiedad)" + Nombre + "\n";

			// Revisar ecosistemas

			foreach (var x in Global.g_.Data.Ecosistemas)
			{
				foreach (var item in tab)
				{
					
				}
				if (x.PropPropiedad.ContainsKey(this))
					ret += tab += x.Nombre;
			}

			return ret;
		}
	}
}