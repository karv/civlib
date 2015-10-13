using System.Collections.Generic;
using System.Runtime.Serialization;
using ListasExtra;
using Civ;
using System.Globalization;

namespace Civ.Data
{
	/// <summary>
	/// Representa una clase de unidad
	/// </summary>
	[DataContract (Name = "Unidad", IsReference = true)]
	public class UnidadRAWCombate : UnidadRAW, IPuntuado, IUnidadRAWCombate
	{
		public class Modificadores : ListaPeso<string>
		{
		}

		#region IPuntuado

		float IPuntuado.Puntuación
		{
			get
			{
				return Fuerza;
			}
		}

		#endregion

		/// <summary>
		/// Lista de modificadores de combate de la unidad.
		/// </summary>        
		[DataMember (Name = "Modificadores")]
		public Modificadores Mods { get; }

		float IUnidadRAWCombate.getModificador (string modificador)
		{
			return Mods [modificador];
		}

		IEnumerable<string> IUnidadRAWCombate.Modificadores{ get { return Mods.Keys; } }

		/// <summary>
		/// Fuerza de la unidad.
		/// </summary>
		[DataMember]
		public float Fuerza { get; set; }

		/// <summary>
		/// Flotante en [0, 1]
		/// Qué tanto se dispersa el daño entre el stack enemigo.
		/// </summary>
		[DataMember]
		public float Dispersión { get; set; }

		#region IImportable

		protected override void LeerLínea (string [] spl)
		{
			base.LeerLínea (spl);
			switch (spl [0])
			{
				case "dispersión":
					Dispersión = float.Parse (spl [1]);
					return;
				case "fuerza":
					Fuerza = float.Parse (spl [1]);
					return;
				case "modificador":
					Mods [spl [1]] = float.Parse (spl [2]);
					return;
			}
		}

		#endregion
	}
}