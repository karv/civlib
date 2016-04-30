﻿using System;

namespace Civ
{
	[Serializable]
	/// <summary>
	/// Clase de diplomacia para civilizaciones nómadas
	/// </summary>
	public class DiplomaciaNómada : IDiplomacia
	{
		public bool PermiteAtacar (Armada arm)
		{
			return true;
		}

		public bool PermitePaso (Armada arm)
		{
			return false;
		}

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