using System.Linq;
using System.Collections.Generic;
using System;

namespace IU
{
	[Serializable]
	/// <summary>
	/// Manejador de mensajes de una civilización
	/// </summary>
	public class ManejadorMensajes : Queue<Mensaje>
	{
		public new void Enqueue (Mensaje m)
		{
			if (m.VerificadorRepetición == null || !this.Any (m.VerificadorRepetición.Equals))
			{
				base.Enqueue (m);
			}
		}
	}
}

