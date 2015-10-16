﻿using System;

namespace Civ.Combate
{
	public interface IAnálisisCombate
	{
		IAtacante Atacante { get; }

		Stack Defensor { get; }

		string Análisis ();
	}
}

