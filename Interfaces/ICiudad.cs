using System.Collections.Generic;

namespace Civ
{
	public interface ICiudad: IAlmacenante, ITickable, IPuntuado, IPosicionable
	{
		ICollection<Ciencia> Avances { get; }

		float AlimentoAlmacen { get; set; }

		string Nombre { get; set; }

		int NumEdificios(EdificioRAW edif);

		void IntentaConstruirAutoconstruibles();

		bool ExisteEdificio(EdificioRAW edif);

		ICollection<TrabajoRAW> ObtenerTrabajosAbiertos();

		Trabajo EncuentraInstanciaTrabajo(TrabajoRAW raw);

		List<Armada> ArmadasEnCiudad();

		Armada Defensa { get; }

		ICivilizacion CivDueño{ get; set; }

		InfoPoblacion GetPoblacionInfo{ get; }

		ulong UnidadesConstruibles(UnidadRAW unidad);

		ICollection<UnidadRAW> UnidadesConstruibles();

		Stack Reclutar(UnidadRAW uRAW, ulong cantidad = 1);

		ulong NumTrabajadores { get; }
	}


}

