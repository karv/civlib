using System;
using ListasExtra;
using System.Collections.Generic;

namespace Civ
{
	public partial class Ciudad
	{
		//Almacén
		//TODO Hacer una clase que controle bien esto; luego veo.
		/// <summary>
		/// Almacén de recursos.
		/// </summary>
		public ListaPeso<Recurso> Almacén = new ListaPeso<Recurso>();

		/// <summary>
		/// Devuelve el alimento existente en la ciudad.
		/// </summary>
		/// <value>The alimento almacén.</value>
		public float AlimentoAlmacén
		{
			get
			{
				return Almacén[RecursoAlimento];
			}
			set
			{
				Almacén[RecursoAlimento] = value;
			}

		}
	}
}

