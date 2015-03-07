using System;

namespace Civ
{
	public partial class Trabajo
	{
		ulong _Trabajadores;

		/// <summary>
		/// Devuelve o establece el número de trabajadores ocupados en este trabajo.
		/// </summary>
		/// <value>The trabajadores.</value>
		public ulong Trabajadores
		{
			get
			{
				return _Trabajadores;
			}
			set
			{
				ulong realValue;

				_Trabajadores = 0;
				realValue = (ulong)Math.Min(value, EdificioBase.getEspaciosTrabajadoresCiudad);
				_Trabajadores = realValue;
			}
		}

		/// <summary>
		/// Devuelve el máximo número de trabajadores que tienen espacio en este trabajo actualmente.
		/// </summary>
		/// <value>The max trabajadores.</value>
		public ulong MaxTrabajadores
		{
			get
			{
				return EdificioBase.getEspaciosTrabajadoresCiudad + Trabajadores;
			}
		}
	}
}