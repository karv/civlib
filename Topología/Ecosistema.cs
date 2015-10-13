using System.Runtime.Serialization;
using ListasExtra;
using System.Collections.Generic;
using Civ.Data;
using Civ.Data.Import;

namespace Civ
{
	/// <summary>
	/// Es la forma RAW de Terreno.
	/// Es, en forma menos técnica, una clase de ecosistema ecológico.
	/// Ej. Selva, desierto, etc.
	/// </summary>
	[DataContract]
	public class Ecosistema : IImportable
	{
		/// <summary>
		/// Representa las propiedades que puede adquirir un ecosistema.
		/// </summary>
		//[DataContract (Name= "Propiedad")]
		public class EcosistemaPropiedades : ListaPeso<Propiedad>
		{
		}

		public Ecosistema ()
		{
			Nombres = new C5.ArrayList<string> ();
			PropPropiedad = new EcosistemaPropiedades ();
		}

		/// <summary>
		/// El nombre del terreno
		/// </summary>
		[DataMember]
		public string Nombre;

		/// <summary>
		/// Es la lista de probabilidades de que una <c>Propiedad</c> <c>Innata</c> aparezca en un terreno con esta ecología.
		/// </summary>
		/// <value><c>ListaPeso</c> de asignación de <c>Propiedades</c> con sus probabilidades.</value>
		[DataMember (Name = "Propiedades")]
		public EcosistemaPropiedades PropPropiedad { get; }

		/// <summary>
		/// Lista de nombres para terrenos
		/// </summary>
		[DataMember (Name = "Nombres")]
		public ICollection<string> Nombres { get; }

		/// <summary>
		/// Crea un terreno aleatoriamente a partir de este ecosistema
		/// </summary>
		public Terreno CrearTerreno ()
		{
			return new Terreno (this);
		}

		#region IImportable

		C5.ArrayList<string []> _prop_ids = new C5.ArrayList<string []> ();

		void IImportable.Importar (System.IO.StreamReader reader)
		{
			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine ();
				line.ToLower ();
				var spl = line.Split (':');
				for (int i = 0; i < spl.Length; i++)
				{
					spl [i] = spl [i].Trim ();
				}

				switch (spl [0])
				{
					case "nombre":
						Nombre = spl [1];
						break;
					case "nombres":
						Nombres.Add (spl [1]);
						break;
					case "propiedad":
						var a = new string[2];
						a [0] = spl [1];
						a [1] = spl [2];
						_prop_ids.Add (a);
						break;
				}
			}
		}

		void IImportable.Vincular ()
		{
			foreach (var x in _prop_ids)
			{
				var a = ImportMachine.Valor (x [0]) as Propiedad;
				PropPropiedad.Add (a, float.Parse (x [1]));
			}
			_prop_ids = null;
		}

		#endregion
	}
}