using System.Linq;
using System.Collections.Generic;
using System;
using ListasExtra;

namespace Civ.IU
{
	// TODO: Hacer un EqualityComparer para manejar repeticiones.
	/// <summary>
	/// Manejador de mensajes de una civilización
	/// </summary>
	public class ManejadorMensajes : ListaCíclica<Mensaje>
	{
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
			if (m.VerificadorRepetición == null || !this.Any (z => m.VerificadorRepetición.Coincide (z.VerificadorRepetición)))
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