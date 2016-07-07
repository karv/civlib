using System;
using System.Linq;
using Civ.RAW;
using Civ.ObjetosEstado;
using Civ.Global;

namespace Civ.Almacén
{
	/// <summary>
	/// Almacena recursos globales.
	/// </summary>
	[Serializable]
	public class AlmacénCiv : AlmacénGenérico, IAlmacén
	{
		#region General

		/// <summary>
		/// Civilización que posee este almacén
		/// </summary>
		public readonly ICivilización Civil;

		/// <summary>
		/// Elimina los recursos con la flag "Desaparece"
		/// </summary>
		public void RemoverRecursosDesaparece ()
		{
			foreach (var x in Recursos.Where (x => x.Desaparece))
				this [x] = 0;
		}

		/// <summary>
		/// Devuelve la cantidad de recursos presentes.
		/// Si R es global devuelve su valor "as is".
		/// Si R no es globa, suma los almacenes de cada ciudad.
		/// 
		/// O establece los recursos globales del almacén global.
		/// </summary>
		/// <param name="id">Id del recurso</param>
		new public float this [int id]
		{
			get
			{
				var recurso = Juego.Data.Recursos [id];
				if (recurso.EsGlobal)
				{
					return base [recurso];
				}
				else
				{
					float ret = 0;
					foreach (var x in Civil.Ciudades)
					{
						ret += x.Almacén [recurso];
					}
					return ret;
				}
			}
			set
			{
				var recurso = Juego.Data.Recursos [id];
				if (recurso.EsGlobal)
					base [recurso] = value;
				else
				{
					throw new Exception (string.Format (
						"Sólo se pueden almacenar recursos globales en AlmacenCiv.\n{0} no es global.",
						recurso));
				}
			}
		}

		#endregion

		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Almacén.AlmacénCiv"/> class.
		/// </summary>
		/// <param name="civilizacion">civilizacion vinculada a este almacén</param>
		public AlmacénCiv (ICivilización civilizacion)
		{
			Civil = civilizacion;
		}

		#endregion
	}
}