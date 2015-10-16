using System;
using Civ.Data;

namespace Civ.Combate
{
	/// <summary>
	/// Permite atacar a una armada
	/// </summary>
	public interface IAtacante
	{
		/// <summary>
		/// Devuelve el daño (indep tiempo) a una unidadRAW
		/// </summary>
		/// <param name="unidad">Unidad.</param>
		float ProponerDaño (IUnidadRAW unidad);

		/// <summary>
		/// Causa daño propuesto a un stack
		/// </summary>
		/// <param name="Def">Defensor</param>
		IAnálisisCombate CausarDaño (IDefensor Def, TimeSpan t);

		float Dispersión { get; }

		event Action AlSerAtacado;

	}

	/// <summary>
	/// Una armada defendiendo
	/// </summary>
	public interface IDefensor
	{
		/// <summary>
		/// Elige un stack que defienda contra un atacante
		/// </summary>
		/// <param name="atacante">Atacante.</param>
		Stack Defensa (IAtacante atacante);
	}
}