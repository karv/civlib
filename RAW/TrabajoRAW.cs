using System.Collections.Generic;
using ListasExtra;
using System.Runtime.Serialization;

namespace Civ.Data
{
	/// <summary>
	/// Representa un trabajo en un edificioRAW
	/// </summary>	
	[DataContract (IsReference = true, Name = "Trabajo")]
	public class TrabajoRAW: Civ.Debug.IPlainSerializable
	{
		/// <summary>
		/// Nombre
		/// </summary>
		[DataMember]
		public string Nombre;
		/// <summary>
		/// EdificioRAW vinculado a este trabajo.
		/// </summary>
		[DataMember]
		public EdificioRAW Edificio;

		/// <summary>
		/// Recursos producidos por trabajador*turno (Base)
		/// </summary>
		[DataMember (Name = "Salida")]
		public ListaPeso<Recurso> SalidaBase { get; }

		/// <summary>
		/// Recursos consumidos por trabajador*turno (Base)
		/// </summary>
		[DataMember (Name = "Entrada")]
		public ListaPeso<Recurso> EntradaBase { get; }

		public override string ToString ()
		{
			return string.Format ("{0} @ {1}", Nombre, Edificio);
		}
		// Requiere
		/// <summary>
		/// Lista de requerimientos.
		/// </summary>
		[DataMember]
		public Requerimiento Requiere = new Requerimiento ();

		/// <summary>
		/// Devuelve la lista de requerimientos.
		/// </summary>
		/// <value>El IRequerimiento</value> 
		public ICollection<IRequerimiento<ICiudad>> Reqs ()
		{
			return Requiere.Requiere ();
		}

		string Civ.Debug.IPlainSerializable.PlainSerialize (int tabs)
		{
			string tab = "";
			string ret;
			Civ.Debug.IPlainSerializable Ser;
			for (int i = 0; i < tabs; i++)
			{
				tab += "\t";
			}

			ret = tab + "(Trabajo)" + Nombre + "\n";

			Ser = Edificio;
			ret += Ser.PlainSerialize (tabs + 1);

			Ser = Requiere;
			ret += Ser.PlainSerialize (tabs + 1);

			return ret;

		}
	}
}