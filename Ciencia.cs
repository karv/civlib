using System;

namespace Civ
{
	[Serializable()]
	/// <summary>
	/// Representa un adelanto científico.
	/// </summary>
	public class Ciencia : IRequerimiento
	{
		/// <summary>
		/// Nombre de la ciencia;
		/// </summary>
		public String Nombre;
        string IRequerimiento.ObtenerId()
        {
            return Nombre;
        }

		public override string ToString ()
		{
			return Nombre;
		}

			// Sobre los requerimientos.
		/// <summary>
		/// Recurso que se necesita para investigar.
		/// </summary>
		public String RecursoReq;
		/// <summary>
		/// Cantidad de <see cref="RecursoReq"/> que se necesita para investigar.
		/// </summary>
		public float CantidadReq;

		/// <summary>
		/// Lista de requerimientos científicos.
		/// </summary>
		public System.Collections.Generic.List<String> ReqCiencia = new System.Collections.Generic.List<String>();


			// IRequerimiento
		bool Civ.IRequerimiento.LoSatisface (Ciudad C){
			return C.CivDueño.Avances.Contains(this);
		}
	}
}
