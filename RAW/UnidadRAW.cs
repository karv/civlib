using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ListasExtra;

namespace Civ
{
	/// <summary>
	/// Representa una clase de unidad
	/// </summary>
	[DataContract(Name = "Unidad", IsReference = true)]
	public class UnidadRAW : CivLibrary.Debug.IPlainSerializable
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
			[DataMember(Name = "Población")]
			public float poblacionACiudad;

			[DataMember(Name = "Edificios")]
			EdificioRAW[] _edificiosIniciales;

			/// <summary>
			/// Edificios con los que inicia la nueva ciudad.
			/// </summary>
			public EdificioRAW[] edificiosIniciales
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

		[DataMember(Name = "Colonizar")]
		public ColonizarOpciones? colonizacion;

		public bool PuedeColonizar
		{
			get
			{
				return colonizacion != null;
			}
		}

		#endregion


		/// <summary>
		/// El nombre de la clase de unidad.
		/// </summary>
		[DataMember]
		public string Nombre;
		[DataMember(Name = "Modificadores")]
		Modificadores _Mods = new Modificadores();

		/// <summary>
		/// Lista de modificadores de combate de la unidad.
		/// </summary>        
		public Modificadores Mods
		{
			get { return _Mods; }
		}

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
		/// Cómo se mueve por el mapa
		/// </summary>
		[DataMember]
		public float Velocidad;

		[DataMember(Name = "Flags")]
		private List<string> _Flags = new List<string>();

		/// <summary>
		/// Flags.
		/// </summary>
		public List<string> Flags
		{
			get { return _Flags; }
		}
		// Reqs
		[DataMember(Name = "Requerimientos")]
		private Requerimientos _Reqs = new Requerimientos();

		/// <summary>
		/// Requerimientos para crearse.
		/// </summary>
		public Requerimientos Reqs
		{
			get { return _Reqs; }
		}

		[DataMember(Name = "Ciencia")]
		Ciencia _ReqCiencia;

		/// <summary>
		/// Devuelve la ciencia requerida para entrenar a la unidad.
		/// </summary>
		public Ciencia ReqCiencia
		{
			get { return _ReqCiencia; }
		}

		/// <summary>
		/// Población productiva que requiere para entrenar.
		/// </summary>
		[DataMember(Name = "CostePoblación")]
		public ulong CostePoblacion;
		/// <summary>
		/// Representa el coste de espacio de esta unidad en una armada.
		/// </summary>
		[DataMember]
		public float Peso;

		public override string ToString()
		{
			return Nombre;
		}

		string CivLibrary.Debug.IPlainSerializable.PlainSerialize(int tabs)
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
				ret += ((CivLibrary.Debug.IPlainSerializable)x).PlainSerialize(tabs + 1);
			}
			return ret;
		}
	}
}