using System.Collections.Generic;
using Civ.Comandos;
using Civ.ObjetosEstado;

namespace Civ.RAW
{
	/// <summary>
	/// Una clase de unidad
	/// </summary>
	public interface IUnidadRAW : IPuntuado
	{
		/// <summary>
		/// Revisa si esta unidad tiene un flag
		/// </summary>
		/// <returns><c>true</c>, se existe flag, <c>false</c> otherwise.</returns>
		/// <param name="flag">Modificador.</param>
		bool TieneFlag (string flag);

		/// <summary>
		/// Velocidad de desplazamiento (unidades por hora)
		/// </summary>
		/// <value>The velocidad.</value>
		float Velocidad { get; }

		/// <summary>
		/// Devuelve el número máximo de estas unidades que puede crear una ciudad
		/// </summary>
		/// <returns>Máximo número de reclutas</returns>
		/// <param name="ciudad">Ciudad que recluta</param>
		ulong MaxReclutables (ICiudad ciudad);

		/// <summary>
		/// Recluta una cantidad de estas unidades en una ciudad.
		/// </summary>
		/// <param name="cantidad">Cantidad a reclutar</param>
		/// <param name="ciudad">Ciudad dónde reclutar</param>
		void Reclutar (ulong cantidad, ICiudad ciudad);

		/// <summary>
		/// Nombre de la unidad
		/// </summary>
		string Nombre { get; }

		/// <summary>
		/// Peso de cada unidad de este tipo
		/// </summary>
		float Peso { get; }

		/// <summary>
		/// Cantidad de peso que puede cargar
		/// </summary>
		float MaxCarga { get; }

		/// <summary>
		/// Devuelve la defensa de la unidad
		/// </summary>
		float Defensa { get; }

		/// <summary>
		/// Devuelve los comandos especiales de la unidad
		/// </summary>
		/// <value>The comandos.</value>
		IEnumerable<IComandoEspecial> Comandos { get; }

		/// <summary>
		/// Revisa si esta unidad está disponible para ser creada por una civilización
		/// </summary>
		/// <returns><c>true</c>, si está disponible; <c>false</c> otherwise.</returns>
		/// <param name="civil">Civilización</param>
		bool EstaDisponible (ICivilización civil);
	}
}