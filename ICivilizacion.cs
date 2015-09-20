using System;
using System.Collections.Generic;
using Civ;

namespace Civ
{
	/// <summary>
	/// Una interface de civilización
	/// </summary>
	public interface ICivilizacion : ITickable, IPuntuado
	{
		string Nombre { get; }

		int CuentaEdificios(EdificioRAW ClaseEdif);

		ICollection<ICiudad> Ciudades { get; }

		ICollection<Armada> Armadas { get; }

		IDiplomacia Diplomacia{ get; }

		ICollection<Ciencia> Avances { get; }

		AlmacénCiv Almacen { get; }

		void AgregaMensaje(IU.Mensaje Mens);
	}

	public static class CivExt
	{
		/// <summary>
		/// Quita una ciudad de la civilización, haciendo que quede sin <c>CivDueño</c>.
		/// </summary>
		/// <param name="C">Ciudad a quitar.</param>
		public static void removeCiudad(this ICivilizacion civ, Ciudad C)
		{
			if (C.CivDueno == civ)
				C.CivDueno = null;
		}

	}
}

