using System.Collections.Generic;

namespace Civ.IU
{
	/// <summary>
	/// El tipo de repetición
	/// </summary>
	public enum TipoRepetición
	{
		/// <summary>
		/// No hay tipo, 
		/// </summary>
		NoTipo = 0,
		/// <summary>
		/// Repetición de cuando una armada termina una orden.
		/// </summary>
		ArmadaTerminaOrden,
		/// <summary>
		/// Repetición de cuando la población pierde población
		/// </summary>
		PerderPoblaciónOcupada,
		/// <summary>
		/// Repetición de cuando se está desperdiciando recursos.
		/// </summary>
		DesperdiciandoRecurso,
		/// <summary>
		/// Se recibe un análisis de combate completo
		/// </summary>
		AnálisisCombateCompleto,
		/// <summary>
		/// Cuando se termina una investigación.
		/// </summary>
		InvestigaciónTerminada
	}
	
}