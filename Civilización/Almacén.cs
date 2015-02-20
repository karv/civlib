using System;
using ListasExtra;

namespace Civ
{
	public partial class Civilizacion
	{
		public readonly AlmacenCiv Almacen;
	}

	/// <summary>
	/// Almacena recursos globales.
	/// </summary>
	public class AlmacenCiv:ListaPeso<Recurso>
	{
		public readonly Civilizacion Civil;

		public AlmacenCiv(Civilizacion C)
		{
			Civil = C;
		}

		/// <summary>
		/// Devuelve la cantidad de recursos presentes.
		/// Si R es global devuelve su valor "as is".
		/// Si R no es globa, suma los almacenes de cada ciudad.
		/// 
		/// O establece los recursos globales del almacén global.
		/// </summary>
		/// <param name="R">Recurso</param>
		new public float this [Recurso R]
		{
			get
			{
				if (R.EsGlobal)
				{
					return base[R];
				}
				else
				{
					float ret = 0;
					foreach (var x in Civil.getCiudades)
					{
						ret += x.Almacén[R];
					}
					return ret;
				}
			}
			set
			{
				if (R.EsGlobal)
					base[R] = value;
				else
				{
					throw new Exception(string.Format("Sólo se pueden almacenar recursos globales en AlmacenCiv.\n{0} no es global.", R));
				}
			}
		}
	}
}