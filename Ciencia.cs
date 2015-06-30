using System;
using System.Runtime.Serialization;

namespace Civ
{
	public class RequiereCiencia: ListasExtra.ListaPeso<Recurso>
	{
		public RequiereCiencia() : base(new System.Collections.Generic.Dictionary<Recurso, float>())
		{
		}
	}

	/// <summary>
	/// Representa un adelanto científico.
	/// </summary>
	[DataContract(IsReference = true)]
	public class Ciencia : IRequerimiento<Ciudad>, CivLibrary.Debug.IPlainSerializable
	{
		[DataContract(IsReference = false)]
		public class Requerimiento
		{
			[DataMember(Name = "Recurso")]
			RequiereCiencia _Recursos = new RequiereCiencia();

			/// <summary>
			/// Devuelve la lista de recursos que se necesita para investigar
			/// </summary>
			/// <value>The recursos.</value>
			public RequiereCiencia Recursos
			{
				get
				{
					return _Recursos;
				}
			}

			/// <summary>
			/// Lista de requisitos científicos.
			/// </summary>
			[DataMember(Name = "Ciencias")]
			public System.Collections.Generic.List<Ciencia> Ciencias = new System.Collections.Generic.List<Ciencia>();
		}

		/// <summary>
		/// Nombre de la ciencia;
		/// </summary>
		[DataMember(Name = "Nombre")]
		public String Nombre;

		public override string ToString()
		{
			return Nombre;
		}
		// Sobre los requerimientos.
		/// <summary>
		/// Requerimientos para poder aprender este avance.
		/// </summary>
		[DataMember(Name = "Requiere")]
		public Requerimiento Reqs = new Requerimiento();
		// IRequerimiento
		bool Civ.IRequerimiento<Ciudad>.LoSatisface(Ciudad C)
		{
			return C.CivDueno.Avances.Contains(this);
		}

		string CivLibrary.Debug.IPlainSerializable.PlainSerialize(int tabs)
		{
			string tab = "";
			string ret;
			for (int i = 0; i < tabs; i++)
			{
				tab += "\t";
			}
			ret = tab + "(Ciencia)" + Nombre + "\n";

			foreach (Ciencia x in Reqs.Ciencias)
			{
				CivLibrary.Debug.IPlainSerializable Ser = (CivLibrary.Debug.IPlainSerializable)x;
				ret += Ser.PlainSerialize(tabs + 1);
				//ret += (CivLibrary.Debug.IPlainSerializable)(x) .PlainSerialize(tabs + 1);
			}

			foreach (var x in Reqs.Recursos.Keys)
			{
				CivLibrary.Debug.IPlainSerializable Ser = (CivLibrary.Debug.IPlainSerializable)x;
				ret += Ser.PlainSerialize(tabs + 1);// + "(" + Reqs.Recursos[x] + ")";
			}
			return ret;
		}
	}
}
