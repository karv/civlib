using System;
using System.Linq;
using Civ.RAW;
using Civ.ObjetosEstado;

namespace Civ.Almacén
{
	/// <summary>
	/// Almacena recursos globales.
	/// </summary>
	[Serializable]
	public class AlmacénCiv : AlmacénGenérico
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
			foreach (var x in Entradas.Where (x => x.Desaparece && this[x] > 0))
				this [x] = 0;
		}

		/// <summary>
		/// Devuelve una copia de la lista de entradas.
		/// </summary>
		public Recurso[] Entradas
		{
			get
			{
				return Keys.ToArray<Recurso> ();
			}
		}

		/// <summary>
		/// Devuelve la cantidad de recursos presentes.
		/// Si R es global devuelve su valor "as is".
		/// Si R no es globa, suma los almacenes de cada ciudad.
		/// 
		/// O establece los recursos globales del almacén global.
		/// </summary>
		/// <param name="rec">Recurso</param>
		public override float this [Recurso rec]
		{
			get
			{
				if (rec.EsGlobal)
				{
					return base [rec];
				}
				else
				{
					float ret = 0;
					foreach (var x in Civil.Ciudades)
					{
						ret += x.Almacén [rec];
					}
					return ret;
				}
			}
			set
			{
				if (rec.EsGlobal)
					base [rec] = value;
				else
				{
					throw new Exception (string.Format (
						"Sólo se pueden almacenar recursos globales en AlmacenCiv.\n{0} no es global.",
						rec));
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