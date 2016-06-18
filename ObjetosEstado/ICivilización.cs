using System.Collections.Generic;
using Civ.Almacén;
using Civ.Ciencias;
using Civ.Global;
using Civ.ObjetosEstado;
using Civ.RAW;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Una interface de civilización
	/// </summary>
	public interface ICivilización : ITickable, IPuntuado
	{
		/// <summary>
		/// Nombre de la civilización
		/// </summary>
		/// <value>The nombre.</value>
		string Nombre { get; }

		/// <summary>
		/// Cuenta el número de edificios existentes en alguna ciudad de esta civilización
		/// </summary>
		int CuentaEdificios (EdificioRAW claseEdif);

		/// <summary>
		/// Devuelve una lista con las ciudades de la civilización
		/// </summary>
		IList<ICiudad> Ciudades { get; }

		/// <summary>
		/// Devuelve una colección con las armadas
		/// </summary>
		IList<Armada> Armadas { get; }

		/// <summary>
		/// Devuelve el modelo diplomático.
		/// </summary>
		IDiplomacia Diplomacia { get; }

		/// <summary>
		/// Devuelve los avances científicos/culturales que posee la civilización
		/// </summary>
		ICollection<Ciencia> Avances { get; }

		/// <summary>
		/// El almacén de recursos globales.
		/// </summary>
		AlmacénCiv Almacén { get; }

		/// <summary>
		/// Agrega un mensaje al usuario de esta civilzación
		/// </summary>
		/// <param name="mensaje">Mensaje.</param>
		void AgregaMensaje (IU.IMensaje mensaje);

		/// <summary>
		/// Devuelve si esta civilización está marcada como bárbara
		/// </summary>
		bool EsBárbaro { get; }

		#region Armadas

		/// <summary>
		/// Devuelve el peso máximo que puede tener cualquier armada libre (no defensa)
		/// de esta civilización.
		/// </summary>
		float MaxPeso { get; }

		#endregion

		/// <summary>
		/// Elimina esta civilización del juego
		/// </summary>
		void Destruirse ();

		/// <summary>
		/// Revisa si esta civilización está en realidad muerta
		/// </summary>
		bool DeboDestruirme ();
	}

	/// <summary>
	/// Extensiones de ICivilización
	/// </summary>
	public static class CivExt
	{
		/// <summary>
		/// Quita una ciudad de la civilización, haciendo que quede sin <c>CivDueño</c>.
		/// </summary>
		/// <param name="ciudad">Ciudad a quitar.</param>
		/// <param name="civilización"></param>
		public static void RemoveCiudad (this ICivilización civilización,
		                                 Ciudad ciudad)
		{
			if (ciudad.CivDueño == civilización)
				ciudad.CivDueño = null;
		}
	}
}