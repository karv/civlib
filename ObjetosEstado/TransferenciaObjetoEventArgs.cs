using System.Collections.Generic;
using Civ.RAW;
using System;
using Civ.Topología;
using Civ.Almacén;

namespace Civ.ObjetosEstado
{
	[Serializable]
	public sealed class TransferenciaObjetoEventArgs : EventArgs
	{
		public ICivilización Anterior { get; }

		public ICivilización Actual { get; }

		public readonly object Objeto;

		public TransferenciaObjetoEventArgs (ICivilización anterior,
		                                     ICivilización actual,
		                                     object objeto)
		{
			Anterior = anterior;
			Actual = actual;
			Objeto = objeto;
		}
	}
	
}