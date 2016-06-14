using System.Linq;
using System.Collections.Generic;
using System;
using ListasExtra;

namespace Civ.IU
{
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
				AlAgregar?.Invoke (m);
			}
		}

		public bool Remove (IRepetidor repetidor)
		{
			var removing = this.Where (x => x.VerificadorRepetición.Coincide (repetidor));
			var ret = RemoveAll (x => x.VerificadorRepetición.Coincide (repetidor)) > 0;
			foreach (var x in removing)
				AlEliminar?.Invoke (x);
			return ret;
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre después de agregar un mensaje.
		/// </summary>
		public event Action<Mensaje> AlAgregar;

		/// <summary>
		/// Ocurre después de eliminar un mensaje.
		/// </summary>
		public event Action<Mensaje> AlEliminar;

		#endregion
	}
}