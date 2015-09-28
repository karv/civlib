using System.Collections.Generic;
using Civ;
using System;

namespace Civ
{
	public class CivilizacionBarbara: ICivilizacion
	{
		public IDiplomacia Diplomacia;

		public string Nombre
		{
			get
			{
				return "Bárbaros";
			}
		}

		public int CuentaEdificios(EdificioRAW edif)
		{
			return 0;
		}

		public IList<ICiudad> Ciudades
		{
			get
			{
				return new Ciudad[0];
			}
		}

		List<Armada> _armadas = new List<Armada>();

		public ICollection<Armada> Armadas
		{
			get
			{
				return _armadas;
			}
		}

		IDiplomacia ICivilizacion.Diplomacia
		{
			get
			{
				return Diplomacia;
			}
		}

		public ICollection<Ciencia> Avances
		{
			get
			{
				return new Ciencia[0];
			}
		}

		void ICivilizacion.AgregaMensaje(IU.Mensaje mensaje)
		{
		}

		void ITickable.Tick(TimeSpan t)
		{
		}

		float IPuntuado.Puntuacion
		{
			get
			{
				return 0;
			}
		}

		public AlmacénCiv Almacen
		{
			get
			{
				return null;
			}
		}
	}
}

