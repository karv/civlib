﻿using System;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Clase de diplomacia para civilizaciones nómadas
	/// </summary>
	[Serializable]
	public class DiplomaciaNómada : IDiplomacia
	{
		/// <summary>
		/// Si se le permite atacar a cierta armada.
		/// </summary>
		/// <returns><c>true</c>, if atacar was permited, <c>false</c> otherwise.</returns>
		/// <param name="arm">Arm.</param>
		public bool PermiteAtacar (Armada arm)
		{
			return true;
		}

		/// <summary>
		/// Permites the paso.
		/// </summary>
		/// <returns><c>true</c>, if paso was permited, <c>false</c> otherwise.</returns>
		/// <param name="arm">Arm.</param>
		public bool PermitePaso (Armada arm)
		{
			return false;
		}

		/// <summary>
		/// Occurs when al cambiar diplomacia.
		/// <remarks>No hace nada, es ólo para llenar IDiplomacia.</remarks>
		/// </summary>
		event Action IDiplomacia.AlCambiarDiplomacia
		{
			add
			{
			}
			remove
			{
			}
		}

	}
}