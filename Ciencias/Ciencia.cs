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
		/// <summary>
		/// Requerimientos para esta ciencia
		/// </summary>
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

		#region General

		/// <summary>
		/// Nombre de la ciencia;
		/// </summary>
		public string Nombre { get; set; }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Civ.Ciencias.Ciencia"/>.
		/// </summary>
		/// <returns>El nombre de la ciencia</returns>
		public override string ToString ()
		{
			return Nombre;
		}

		#endregion

		#region Requerimiento

		/// <summary>
		/// Requerimientos para poder aprender este avance.
		/// </summary>
		public Requerimiento Reqs = new Requerimiento ();

		bool IRequerimiento<ICiudad>.LoSatisface (ICiudad ciudad)
		{
			return ciudad.CivDueño.Avances.Contains (this);
		}

		#endregion
	}

	[Serializable]
	public sealed class AvanceEventArgs : EventArgs
	{
		public readonly Ciencia Ciencia;
		public readonly ICivilización Civil;

		public AvanceEventArgs (Ciencia ciencia, ICivilización civil)
		{
			Ciencia = ciencia;
			Civil = civil;
		}
	}
}