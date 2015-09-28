using System.Runtime.Serialization;
using ListasExtra;
using System.Collections.Generic;

namespace Civ
{
	/// <summary>
	/// Es la forma RAW de Terreno.
	/// Es, en forma menos técnica, una clase de ecosistema ecológico.
	/// Ej. Selva, desierto, etc.
	/// </summary>
	[DataContract]
	public class Ecosistema
	{
		/// <summary>
		/// Representa las propiedades que puede adquirir un ecosistema.
		/// </summary>
		//[DataContract (Name= "Propiedad")]
		public class EcosistemaPropiedades : ListaPeso<Propiedad>
		{
		}

		/// <summary>
		/// El nombre del terreno
		/// </summary>
		[DataMember]
		public string Nombre;
		[DataMember(Name = "Propiedades")]
		EcosistemaPropiedades _PropPropiedad = new EcosistemaPropiedades();

		/// <summary>
		/// Es la lista de probabilidades de que una <c>Propiedad</c> <c>Innata</c> aparezca en un terreno con esta ecología.
		/// </summary>
		/// <value><c>ListaPeso</c> de asignación de <c>Propiedades</c> con sus probabilidades.</value>
		public EcosistemaPropiedades PropPropiedad
		{
			get
			{
				return _PropPropiedad;
			}
		}

		[DataMember(Name = "Nombres")]
		public List<string> Nombres;

		public Terreno CrearTerreno()
		{
			return new Terreno(this);
		}
	}
}

