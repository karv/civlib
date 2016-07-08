using System.Linq;
using System.Collections.Generic;
using System;
using ListasExtra;

namespace Civ.IU
{
	/// <summary>
	/// Comparador default de mensajes
	/// </summary>
	public class IgualadorRepetidorMensaje : IEqualityComparer<Mensaje>
	{
		/// <summary>
		/// Determina si dos mensajes son esencialemnte el mismo
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public bool Equals (Mensaje x, Mensaje y)
		{
			return x.VerificadorRepetición.Coincide (y.VerificadorRepetición);
		}

		/// <Docs>The object for which the hash code is to be returned.</Docs>
		/// <para>Returns a hash code for the specified object.</para>
		/// <returns>A hash code for the specified object.</returns>
		/// <summary>
		/// Gets the hash code.
		/// </summary>
		/// <param name="obj">Object.</param>
		public int GetHashCode (Mensaje obj)
		{
			throw new NotImplementedException ();
		}
	}
	// TODO: Hacer un EqualityComparer para manejar repeticiones.
	
}