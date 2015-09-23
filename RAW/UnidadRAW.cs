using System.Collections.Generic;
using System.Runtime.Serialization;
using ListasExtra;
using Civ;

namespace Civ.Data
{
	/// <summary>
	/// Representa una clase de unidad
	/// </summary>
	[DataContract (Name = "Unidad", IsReference = true)]
	public class UnidadRAW : UnidadRAWBase, Civ.Debug.IPlainSerializable, IPuntuado
	{
		public class Modificadores : ListaPeso<string>
		{
		}

		public class Requerimientos : ListaPeso<Recurso>
		{
		}


		#region Settler

		public struct ColonizarOpciones
		{
			/// <summary>
			/// Población con la que cada unidad se convierte en población productiva en la nueva ciudad.
			/// </summary>
			[DataMember (Name = "Población")]
			public float PoblacionACiudad;

			[DataMember (Name = "Edificios")]
			EdificioRAW [] _edificiosIniciales;

			/// <summary>
			/// Edificios con los que inicia la nueva ciudad.
			/// </summary>
			public EdificioRAW[] EdificiosIniciales
			{
				get
				{
					return _edificiosIniciales ?? new EdificioRAW[0];
				}
				set
				{
					_edificiosIniciales = value;
				}
			}
		}

		[DataMember (Name = "Colonizar")]
		public ColonizarOpciones? Colonización;

		public bool PuedeColonizar
		{
			get
			{
				return Colonización != null;
			}
		}

		#endregion

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

		/// <summary>
		/// Fuerza de la unidad.
		/// </summary>
		[DataMember]
		public float Fuerza;

		/// <summary>
		/// Flotante en [0, 1]
		/// Qué tanto se dispersa el daño entre el stack enemigo.
		/// </summary>
		[DataMember]
		public float Dispersion;

		/// <summary>
		/// Flags.
		/// </summary>
		[DataMember (Name = "Flags")]
		public ICollection<string> Flags { get; }

		// Reqs
		/// <summary>
		/// Requerimientos para crearse.
		/// </summary>
		[DataMember (Name = "Requerimientos")]
		public Requerimientos Reqs { get; }

		/// <summary>
		/// Devuelve la ciencia requerida para entrenar a la unidad.
		/// </summary>
		[DataMember (Name = "Ciencia")]
		public Ciencia ReqCiencia { get; set; }

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
			ret = tab + "(Unidad)" + Nombre + "\n";

			foreach (var x in Reqs.Keys)
			{
				ret += ((Civ.Debug.IPlainSerializable)x).PlainSerialize (tabs + 1);
			}
			return ret;
		}
	}
}