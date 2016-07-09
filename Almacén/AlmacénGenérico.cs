using System;
using ListasExtra;
using Civ.RAW;
using System.Collections.Generic;
using Civ.Global;
using System.Diagnostics;
using System.CodeDom;

namespace Civ.Almacén
{
	/// <summary>
	/// Un almacén genérico y común
	/// </summary>
	[Serializable]
	public class AlmacénGenérico : IAlmacén
	{
		#region ctor

		static int i;

		static AlmacénGenérico ()
		{
			Debug.WriteLine (sizeof (float) * 30);
		}

		/// <summary>
		/// </summary>
		public AlmacénGenérico ()
		{
			Debug.WriteLine (++i);
			//_recs = new float[Juego.Data.Recursos.Count];
			_recs = new float[1];
		}

		#endregion

		#region General

		readonly IList<float> _recs;

		/// <summary>
		/// Devuelve la cantidad de diferentes recursos.
		/// </summary>
		/// <value>The count.</value>
		public int Count
		{
			get
			{
				return _recs.Count;
			}
		}

		/// <summary>
		/// Devuelve la lista de recursos implicados
		/// </summary>
		/// <value>The recursos.</value>
		public IEnumerable<Recurso> Recursos
		{
			get
			{
				for (int i = 0; i < Count; i++)
				{
					if (_recs [i] != 0)
						yield return Juego.Data.Recursos [i];
				}
			}
		}

		/// <summary>
		/// Devuelve la cantidad de recursos existentes en un almacén
		/// </summary>
		/// <param name="rec">Recurso.</param>
		public float this [Recurso rec]
		{
			get
			{
				return this [rec.Id];
			}
			set
			{
				this [rec.Id] = value;
			}
		}

		/// <summary>
		/// Devuelve la cantidad de recursos existentes en un almacén
		/// </summary>
		/// <param name="id">Id del recurso</param>
		public float this [int id]
		{
			get
			{
				return _recs [id];
			}
			set
			{
				AlCambiar?.Invoke (
					this,
					new CambioElementoEventArgs<Recurso, float> (
						Juego.Data.Recursos [id],
						this [id],
						value));
				_recs [id] = value;
			}
		}

		#endregion

		#region Almacén

		/// <summary>
		/// Devuelve un array de float que representa las entradas de recursos.
		/// </summary>
		public IList<float> AsArray ()
		{
			return _recs;
		}

		/// <summary>
		/// Revisa si contiene (y cuántas veces) los recursos codificados en un arreglo de float.
		/// </summary>
		/// <param name="otrosReqs">Otros recursos</param>
		/// <returns>The recursos.</returns>
		public float ContieneRecursos (IList<float> otrosReqs)
		{
			var ret = float.PositiveInfinity;
			for (int i = 0; i < Count; i++)
			{
				if (otrosReqs [i] > 0)
					ret = Math.Min (ret, _recs [i] / otrosReqs [i]);
			}
			return ret;
		}

		/// <summary>
		/// Revisa si contiene (y cuántas veces) los recursos codificados en un arreglo de float.
		/// </summary>
		/// <param name="otrosReqs">Otros recursos</param>
		/// <returns>The recursos.</returns>
		public float ContieneRecursos (IAlmacénRead otrosReqs)
		{
			return ContieneRecursos (otrosReqs.AsArray ());
		}

		/// <summary>
		/// Devuelve un conjunto que hace referencia a los recursos positivos.
		/// </summary>
		/// <returns>The positivos.</returns>
		public HashSet<Recurso> RecursosPositivos ()
		{
			var ret = new HashSet<Recurso> ();
			for (int i = 0; i < Count; i++)
				if (this [i] > 0)
					ret.Add (Juego.Data.Recursos [i]);
			return ret;
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre cuando cambia el almacén de un recurso
		/// </summary>
		public event EventHandler<CambioElementoEventArgs<Recurso, float>> AlCambiar;


		#endregion
	}
}