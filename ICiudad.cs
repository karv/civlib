using System;
using System.Collections.Generic;

namespace Civ
{
	public interface ICiudad: IAlmacenante, ITickable, IPuntuado, IPosicionable
	{
		ICollection<Ciencia> Avances { get; }

		float AlimentoAlmacen { get; set; }

		string Nombre { get; set; }

		int NumEdificios(EdificioRAW Edif);

		void IntentaConstruirAutoconstruibles();

		bool ExisteEdificio(EdificioRAW Edif);
	
	}


}

