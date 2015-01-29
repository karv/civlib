using System;
using System.Collections.Generic;

namespace Civ
{
	public partial class Ciudad
	{

		/// <summary>
		/// Revisa si esta ciudad satisface un Irequerimiento.
		/// </summary>
		/// <param name="Req">Un requerimiento</param>
		/// <returns>Devuelve <c>true</c> si esta ciudad satisface un Irequerimiento. <c>false</c> en caso contrario.</returns>
		public bool SatisfaceReq(IRequerimiento Req)
		{
			return Req.LoSatisface(this);
		}

		/// <summary>
		/// Revisa si esta ciudad satisface una lista de requerimientos.
		/// </summary>
		/// <param name="Req"></param>
		/// <returns>Devuelve <c>true</c> si esta ciudad satisface todos los Irequerimiento. <c>false</c> en caso contrario.</returns>
		public bool SatisfaceReq(List<IRequerimiento> Req)
		{
			return Req.TrueForAll(x => x.LoSatisface(this));
		}

	}
}

