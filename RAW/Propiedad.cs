using System.Collections.Generic;
using System;
using Civ.ObjetosEstado;
using Civ.Global;

namespace Civ.RAW
{
	/// <summary>
	/// Representa una propiedad innata de un edificio.
	/// </summary>
	[Serializable]
	public class Propiedad : IRequerimiento<Ciudad>
	{
		/// <summary>
		/// Nombre de la propiedad.
		/// </summary>
		public string Nombre;

		public Propiedad ()
		{
			Salida = new HashSet<TasaProd> ();
		}

		/// <summary>
		/// Recursos que produce esta propiedad por turno.
		/// </summary>
		public ICollection<TasaProd> Salida { get; private set; }
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
		public virtual void Tick (IAlmacén almacén, TimeSpan t)
		{
			foreach (TasaProd x in Salida)
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
	}
}