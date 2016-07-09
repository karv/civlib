using System.Linq;
using System.Collections.Generic;
using System;
using ListasExtra;

namespace Civ.IU
{
	/// <summary>
	/// Manejador de mensajes de una civilización
	/// </summary>
	public class ManejadorMensajes : ListaCíclica<IMensaje>
	{
		#region Interno

		/// <summary>
		/// Determina si dos mensajes son iguales
		/// </summary>
		public readonly IEqualityComparer<IMensaje> IgualadorRepetición;

		#endregion

		#region Control

		/// <Docs>The item to add to the current collection.</Docs>
		/// <para>Adds an item to the current collection.</para>
		/// <remarks>To be added.</remarks>
		/// <exception cref="System.NotSupportedException">The current collection is read-only.</exception>
		/// <summary>
		/// Agrega un mensaje al usuario
		/// </summary>
		/// <param name="m">Mensaje</param>
		public new void Add (IMensaje m)
		{
			if (!this.Any 
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
		public bool Remove (TipoRepetición repetidor)
		{
			var removing = this.Where (x => x.Tipo == repetidor);
			var ret = false;
			foreach (var x in removing)
			{	
				AlEliminar?.Invoke (this, new MensajeEventArgs (x, this));
				ret |= Remove (x);
			}
			return ret;
		}

		#endregion

		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.IU.ManejadorMensajes"/> class.*/
		/// </summary>
		public ManejadorMensajes ()
		{
			IgualadorRepetición = new Mensaje.Igualador ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.IU.ManejadorMensajes"/> class.
		/// </summary>
		/// <param name="comparador">Comparador.</param>
		public ManejadorMensajes (IEqualityComparer<IMensaje> comparador)
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