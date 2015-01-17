using System;

namespace Civ
{
	public partial class Trabajo
	{
		ulong _Trabajadores;

		public ulong Trabajadores {
			get {
				return _Trabajadores;
			}
			set {
				ulong realValue;

				_Trabajadores = 0;
				realValue = (ulong)Math.Min (value, EdificioBase.getEspaciosTrabajadoresCiudad);
				_Trabajadores = realValue;
			}
		}

		public ulong MaxTrabajadores
		{
			get
			{
				return EdificioBase.getEspaciosTrabajadoresCiudad + Trabajadores;
			}
		}
	}
}

