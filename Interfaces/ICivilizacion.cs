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

		int CuentaEdificios(EdificioRAW claseEdif);

		IList<ICiudad> Ciudades { get; }

		ICollection<Armada> Armadas { get; }

		IDiplomacia Diplomacia{ get; }

		ICollection<Ciencia> Avances { get; }

		AlmacénCiv Almacen { get; }

		void AgregaMensaje(IU.Mensaje mensaje);
	}

	public static class CivExt
	{
		/// <summary>
		/// Quita una ciudad de la civilización, haciendo que quede sin <c>CivDueño</c>.
		/// </summary>
		/// <param name="ciudad">Ciudad a quitar.</param>
		/// <param name="civilización"></param>
		public static void RemoveCiudad(this ICivilizacion civilización, Ciudad ciudad)
		{
			if (ciudad.CivDueno == civilización)
				ciudad.CivDueno = null;
		}

	}
}

