using System.Collections.Generic;
using System.Runtime.Serialization;
using System;
using Civ.Data.Import;
using Civ.Data.TasaProd;

namespace Civ.Data
{
	/// <summary>
	/// Representa una propiedad innata de un edificio.
	/// </summary>
	[DataContract]
	public class Propiedad : IRequerimiento<Ciudad>, IImportable
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
		public ICollection<TasaProd.TasaProd> Salida { get; private set; }
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

		#region apuntadores

		List<string> ref_Salida = new List<string> ();

		#endregion

		public void Importar (System.IO.StreamReader reader)
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
					case "salida":
						ref_Salida.Add (spl [1]);
						break;
				}
			}
		}

		void IImportable.Vincular ()
		{
			Salida = new C5.HashSet<TasaProd.TasaProd> ();
			foreach (var x in ref_Salida)
			{
				var a = ImportMachine.Valor (x) as TasaProd.TasaProd;
				Salida.Add (a);
			}
			ref_Salida = null;
		}
	}
}