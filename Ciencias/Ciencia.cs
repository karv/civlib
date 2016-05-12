using System;
using System.Collections.Generic;
using Civ.ObjetosEstado;
using Civ.Global;

namespace Civ.Ciencias
{
	/// <summary>
	/// Representa un adelanto científico.
	/// </summary>
	[Serializable]
	public class Ciencia : IRequerimiento<ICiudad>, IPuntuado
	{
		[Serializable]
		public class Requerimiento
		{
			readonly RequiereCiencia _Recursos = new RequiereCiencia ();

			/// <summary>
			/// Devuelve la lista de recursos que se necesita para investigar
			/// </summary>
			/// <value>The recursos.</value>
			public RequiereCiencia Recursos
			{
				get
				{
					return _Recursos;
				}
			}

			/// <summary>
			/// Lista de requisitos científicos.
			/// </summary>
			public ICollection< Ciencia> Ciencias = new HashSet<Ciencia> ();
			// Se debe convertir en GuardedCollection cuando se lea.
		}

		#region Puntuación

		float CoefPunc = 1.2f;

		float IPuntuado.Puntuación
		{
			get
			{
				var ret = 0f;
				foreach (var x in this.Reqs.Recursos)
					ret += x.Value * x.Key.Valor;
				return ret * CoefPunc;
			}
		}

		#endregion

		/// <summary>
		/// Nombre de la ciencia;
		/// </summary>
		public String Nombre { get; set; }

		public override string ToString ()
		{
			return Nombre;
		}

		// Sobre los requerimientos.
		/// <summary>
		/// Requerimientos para poder aprender este avance.
		/// </summary>
		public Requerimiento Reqs = new Requerimiento ();

		#region IRequerimiento

		bool IRequerimiento<ICiudad>.LoSatisface (ICiudad ciudad)
		{
			return ciudad.CivDueño.Avances.Contains (this);
		}

		#endregion
	}
}