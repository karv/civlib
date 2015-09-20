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

		ICollection<TrabajoRAW> ObtenerTrabajosAbiertos();

		Trabajo EncuentraInstanciaTrabajo(TrabajoRAW TRAW);

		List<Armada> ArmadasEnCiudad();

		Armada Defensa { get; }

		ICivilizacion CivDueño{ get; }

		InfoPoblacion GetPoblacionInfo{ get; }

		ulong UnidadesConstruibles(UnidadRAW unidad);

		ICollection<UnidadRAW> UnidadesConstruibles();

		Stack EntrenarUnidades(UnidadRAW uRAW, ulong cantidad = 1);

		ulong getNumTrabajadores { get; }
	}


}

