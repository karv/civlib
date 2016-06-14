using System;
using ListasExtra;
using System.Linq;
using Civ.RAW;
using Civ.ObjetosEstado;

namespace Civ.Almacén
{
	[Serializable]
	/// <summary>
	/// Almacena recursos globales.
	/// </summary>
	public class AlmacénCiv : ListaPeso<Recurso>, IAlmacén
	{
		#region General

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
		/// <param name="recurso">Recurso</param>
		new public float this [Recurso recurso]
		{
			get
			{
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

		#region Eventos

		/// <summary>
		/// Ocurre cuando cambia el almacén de un recurso
		/// Recurso, valor viejo, valor nuevo
		/// </summary>
		event EventHandler<CambioElementoEventArgs<Recurso, float>> IAlmacénRead.AlCambiar
		{
			add
			{
				AlCambiarValor += value;
			}
			remove
			{
				AlCambiarValor -= value;
			}
		}

		#endregion

		#region IAlmacénRead implementation

		System.Collections.Generic.IEnumerable<Recurso> IAlmacénRead.Recursos
		{
			get
			{
				return Keys;
			}
		}

		#endregion
	}
}