using System;
using ListasExtra;
using System.Linq;

namespace Civ
{
	public partial class Civilizacion
	{
		public readonly AlmacenCiv Almacen;
	}

	/// <summary>
	/// Almacena recursos globales.
	/// </summary>
	public class AlmacenCiv:ListasExtra.Lock.ListaPesoBloqueable<Recurso>, IAlmacén
	{
		public readonly Civilizacion Civil;

		public AlmacenCiv(Civilizacion C)
		{
			Civil = C;
		}

		/// <summary>
		/// Elimina los recursos con la flaf "Desaparece"
		/// </summary>
		public void RemoverRecursosDesaparece()
		{
			foreach (var x in Entradas)
			{
				if (x.Desaparece)
					this[x] = 0;
			}
		}

		/// <summary>
		/// Devuelve una copia de la lista de entradas.
		/// </summary>
		public Recurso[] Entradas
		{
			get
			{
				return Keys.ToArray<Recurso>();
			}
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
						ret += x.Almacen[R];
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

		#region IAlmacénRead implementation

		float IAlmacénRead.recurso(Recurso R)
		{
			return this[R];
		}

		System.Collections.Generic.IEnumerable<Recurso> IAlmacénRead.recursos
		{
			get
			{
				return Keys;
			}
		}

		#endregion

		#region IAlmacén implementation

		public void changeRecurso(Recurso rec, float delta)
		{
			this.Add(rec, delta);
		}

		[Obsolete]
		void IAlmacén.setRecurso(Recurso rec, float val)
		{
			this[rec] = val;
		}

		#endregion
	}
}