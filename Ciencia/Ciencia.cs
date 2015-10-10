using System;
using System.Runtime.Serialization;
using C5;

namespace Civ.Data
{
	/// <summary>
	/// Representa un adelanto científico.
	/// </summary>
	[DataContract (IsReference = true)]
	public class Ciencia : IRequerimiento<ICiudad>, Civ.Debug.IPlainSerializable
	{
		[DataContract (IsReference = false)]
		public class Requerimiento
		{
			[DataMember (Name = "Recurso")]
			readonly RequiereCiencia _Recursos = new RequiereCiencia ();

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
			[DataMember (Name = "Ciencias")]
			public ICollection< Ciencia> Ciencias = new HashSet<Ciencia> ();
			// Se debe convertir en GuardedCollection cuando se lea.
		}

		/// <summary>
		/// Nombre de la ciencia;
		/// </summary>
		[DataMember (Name = "Nombre")]
		public String Nombre { get; set; }

		public override string ToString ()
		{
			return Nombre;
		}

		// Sobre los requerimientos.
		/// <summary>
		/// Requerimientos para poder aprender este avance.
		/// </summary>
		[DataMember (Name = "Requiere")]
		public Requerimiento Reqs = new Requerimiento ();


		#region IRequerimiento

		bool IRequerimiento<ICiudad>.LoSatisface (ICiudad ciudad)
		{
			return ciudad.CivDueño.Avances.Contains (this);
		}

		#endregion

		#region PlainSerializable

		string Civ.Debug.IPlainSerializable.PlainSerialize (int tabs)
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
				Civ.Debug.IPlainSerializable Ser = x;
				ret += Ser.PlainSerialize (tabs + 1);
			}

			foreach (var x in Reqs.Recursos.Keys)
			{
				Civ.Debug.IPlainSerializable Ser = x;
				ret += Ser.PlainSerialize (tabs + 1);
			}
			return ret;
		}

		#endregion
	}
}
