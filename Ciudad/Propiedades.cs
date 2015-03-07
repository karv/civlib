using System;

namespace Civ
{
	public partial class Ciudad
	{
		/// <summary>
		/// Agrega una instancia de <c>Propiedad</c> a la ciudad.
		/// </summary>
		/// <param name="Prop">Propiedad a agregar.</param>
		public void AgregaPropiedad(Propiedad Prop)
		{
			if (!Propiedades.Contains(Prop))
			{
				Propiedades.Add(Prop);
			}
		}
	}
}