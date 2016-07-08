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
	/// <summary>
	/// Manejador de mensajes de una civilización
	/// </summary>
	public class ManejadorMensajes : ListaCíclica<Mensaje>
	{
		#region Interno

		/// <summary>
		/// Determina si dos mensajes son iguales
		/// </summary>
		public readonly IEqualityComparer<Mensaje> IgualadorRepetición;

		#endregion

		#region Control

		/// <Docs>The item to add to the current collection.</Docs>
		/// <para>Adds an item to the current collection.</para>
		/// <remarks>To be added.</remarks>
		/// <exception cref="System.NotSupportedException">The current collection is read-only.</exception>
		/// <summary>
		/// Agrega un mensaje al usuario
		/// </summary>
		public new void Add (Mensaje m)
		{
			if (m.VerificadorRepetición == null || !this.Any 
				(z => IgualadorRepetición.Equals (z, m)))
			{
				base.Add (m);
				AlAgregar?.Invoke (this, new MensajeEventArgs (m, this));
			}
		}

		/// <Docs>The item to remove from the current collection.</Docs>
		/// <para>Removes the first occurrence of an item from the current collection.</para>
		/// <summary>
		/// Elimina los mensajes con un repetidor dado.
		/// </summary>
		/// <param name="repetidor">Repetidor.</param>
		public bool Remove (IRepetidor repetidor)
		{
			var removing = this.Where (x => x.VerificadorRepetición.Coincide (repetidor));
			var ret = RemoveAll (x => x.VerificadorRepetición.Coincide (repetidor)) > 0;
			foreach (var x in removing)
				AlEliminar?.Invoke (this, new MensajeEventArgs (x, this));
			return ret;
		}

		#endregion

		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.IU.ManejadorMensajes"/> class.*/
		/// </summary>
		public ManejadorMensajes ()
		{
			IgualadorRepetición = new IgualadorRepetidorMensaje ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.IU.ManejadorMensajes"/> class.
		/// </summary>
		/// <param name="comparador">Comparador.</param>
		public ManejadorMensajes (IEqualityComparer<Mensaje> comparador)
		{
			IgualadorRepetición = comparador;
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre después de agregar un mensaje.
		/// </summary>
		public event EventHandler AlAgregar;

		/// <summary>
		/// Ocurre después de eliminar un mensaje.
		/// </summary>
		public event EventHandler AlEliminar;

		#endregion
	}
}