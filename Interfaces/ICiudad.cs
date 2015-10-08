using System.Collections.Generic;
using Civ.Data;
using System;

namespace Civ
{
	/// <summary>
	/// Una ciudad
	/// </summary>
	public interface ICiudad: IAlmacenante, ITickable, IPuntuado, IPosicionable
	{
		/// <summary>
		/// Nombre de la ciudad
		/// </summary>
		string Nombre { get; set; }

		/// <summary>
		/// Número de edificios que existen de cierta clase.
		/// </summary>
		int NumEdificios (EdificioRAW edif);

		/// <summary>
		/// Contruye las propiedades autocontruíbles
		/// </summary>
		void IntentaConstruirAutoconstruibles ();

		/// <summary>
		/// Revisa si existe un edificio de cierta clase
		/// </summary>
		bool ExisteEdificio (EdificioRAW edif);

		/// <summary>
		/// Devuelve una colección de los trabajos que puede realizar esta ciudad
		/// </summary>
		ICollection<TrabajoRAW> ObtenerTrabajosAbiertos ();

		/// <summary>
		/// Devuelve el trabajo correspondiente a un RAW en la ciudad. 
		/// Si no existe debe crearlo. 
		/// Si no puede crearlo debe tirar error.
		/// </summary>
		Trabajo EncuentraInstanciaTrabajo (TrabajoRAW raw);

		/// <summary>
		/// Devuelve una colección con las armadas estacionadas en la ciudad.
		/// </summary>
		/// <returns>The en ciudad.</returns>
		ICollection<Armada> ArmadasEnCiudad ();

		/// <summary>
		/// Devuelve la armada inmovil que representa la defensa de la ciudad.
		/// </summary>
		Armada Defensa { get; }

		/// <summary>
		/// Devuelve o establece la civilización que posee a esta ciudad.
		/// </summary>
		ICivilización CivDueño { get; set; }

		/// <summary>
		/// Devuelve la información de distribución de poblaciones
		/// </summary>
		InfoPoblacion GetPoblacionInfo { get; }

		/// <summary>
		/// Devuelve la cantidad máxima de unidades contruíbles de cierto tipo.
		/// </summary>
		ulong UnidadesConstruibles (UnidadRAW unidad);

		/// <summary>
		/// Devuelve una colección de las unidades que podrían ser contruidas en esta ciudad
		/// </summary>
		ICollection<UnidadRAW> UnidadesConstruibles ();

		/// <summary>
		/// Recluta unidades directamente a la armada Defensa
		/// </summary>
		/// <param name="uRAW">Tipo de unidad</param>
		/// <param name="cantidad">Cantidad.</param>
		Stack Reclutar (UnidadRAW uRAW, ulong cantidad = 1);

		/// <summary>
		/// Devuelve el número de trabajadores.
		/// </summary>
		ulong NumTrabajadores { get; }

		/// <summary>
		/// Ocurre cuando el nombre de la ciudad es cambiado
		/// </summary>
		event Action AlCambiarNombre;

		/// <summary>
		/// Ocurre cuando esta ciudad cambia de dueño
		/// </summary>
		event Action AlCambiarDueño;

		/// <summary>
		/// Ocurre cuando se recluta unidades en esta ciudad
		/// </summary>
		event Action<UnidadRAW, ulong> AlReclutar;
	}


}

