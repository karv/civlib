using System.Collections.Generic;
using System.Runtime.Serialization;
using System;

namespace Civ.Data
{
	/// <summary>
	/// Representa una propiedad innata de un edificio.
	/// </summary>
	[DataContract]
	public class Propiedad : IRequerimiento<Ciudad>, Civ.Debug.IPlainSerializable
	{
		/// <summary>
		/// Nombre de la propiedad.
		/// </summary>
		[DataMember]
		public string Nombre;

		/// <summary>
		/// Recursos que produce esta propiedad por turno.
		/// </summary>
		[DataMember (Name = "Salida")]
		public ICollection<TasaProd.TasaProd> Salida { get; }
		// IRequerimiento:
		bool IRequerimiento<Ciudad>.LoSatisface (Ciudad ciudad)
		{
			return ciudad.ExistePropiedad (this);
		}

		/// <summary>
		/// El tick de este edificio sobre una ciudad.
		/// </summary>
		/// <param name="almacén"><see cref="Civ.ICiudad"/> donde hará un tick esta propiedad.</param>
		/// <param name="t">longitud del tick</param>
		public virtual void Tick (IAlmacenante almacén, TimeSpan t)
		{
			foreach (Civ.Data.TasaProd.TasaProd x in Salida)
			{
				x.Tick (almacén, t);
			}
		}

		public override string ToString ()
		{
			return Nombre;
		}

		string Civ.Debug.IPlainSerializable.PlainSerialize (int tabs)
		{
			string tab = "";
			string ret;
			for (int i = 0; i < tabs; i++)
			{
				tab += "\t";
			}
			ret = tab + "(Propiedad)" + Nombre + "\n";

			// Revisar ecosistemas

			foreach (var x in Global.Juego.Data.Ecosistemas)
			{
				if (x.PropPropiedad.ContainsKey (this))
					ret += tab += x.Nombre;
			}

			return ret;
		}
	}
}