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
			Nombres = new List<string> ();
			PropPropiedad = new EcosistemaPropiedades ();
		}

		/// <summary>
		/// El nombre del terreno
		/// </summary>
		public string Nombre;

		/// <summary>
		/// Es la lista de probabilidades de que una <c>Propiedad</c> <c>Innata</c> aparezca en un terreno con esta ecología.
		/// </summary>
		/// <value><c>ListaPeso</c> de asignación de <c>Propiedades</c> con sus probabilidades.</value>
		public EcosistemaPropiedades PropPropiedad { get; }

		/// <summary>
		/// Lista de nombres para terrenos
		/// </summary>
		public ICollection<string> Nombres { get; }

		/// <summary>
		/// Crea un terreno aleatoriamente a partir de este ecosistema
		/// </summary>
		public Terreno CrearTerreno ()
		{
			return new Terreno (this);
		}

		#region IImportable

		List<string []> _prop_ids = new List<string []> ();

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
				System.Console.WriteLine (string.Format (
					"Intentando pedir variable temporal {0} para el ecosistema {1}",
					x [0], Nombre));
				var a = ImportMachine.Valor (x [0]) as Propiedad;
				PropPropiedad.Add (a, float.Parse (x [1]));
			}
			_prop_ids = null;
		}

		#endregion
	}
}