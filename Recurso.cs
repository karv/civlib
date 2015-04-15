using System;
using System.Runtime.Serialization;

namespace Civ
{
	[DataContract(IsReference = true)]
	public class Recurso
	{
		public override string ToString()
		{
			return Nombre;
		}

		/// <summary>
		/// Desaparece al final del turno.
		/// </summary>
		[DataMember(Name = "Desaparece")]
		public bool Desaparece;
		/// <summary>
		/// ¿El recurso es científico?
		/// </summary>
		[DataMember(Name = "Científico")]
		public bool EsCientifico;
		/// <summary>
		/// Nombre del recurso.
		/// </summary>
		[DataMember(Name = "Nombre")]
		public string Nombre;
		/// <summary>
		/// Devuelve o establece si el recurso es global. De ser false, se almacena en cada ciudad por separado.
		/// De ser true, cada ciudad puede tomar de un almacén global.
		/// </summary>
		[DataMember(Name = "Global")]
		public bool EsGlobal = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Recurso"/> class.
		/// </summary>
		/// <param name="nom">Nombre del recurso.</param>
		public Recurso(string nom)
		{
			Nombre = nom;
		}

		public Recurso()
		{
		}

		[DataMember(Name = "Imagen")]
		public string Img = null;
	}
}