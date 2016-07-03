using System;
using ListasExtra;
using Civ.RAW;
using System.Collections.Generic;
using Civ.Global;

namespace Civ.Almacén
{
	/// <summary>
	/// Un almacén genérico y común
	/// </summary>
	[Serializable]
	public class AlmacénGenérico : IAlmacén
	{
		#region General

		readonly float [] _recs = new float[Juego.Data.Recursos.Count];

		/// <summary>
		/// Devuelve la cantidad de diferentes recursos.
		/// </summary>
		/// <value>The count.</value>
		public int Count
		{
			get
			{
				return _recs.Length;
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
		public float[] AsArray ()
		{
			return _recs;
		}

		public float ContieneRecursos (float [] otrosReqs)
		{
			var ret = float.PositiveInfinity;
			for (int i = 0; i < Count; i++)
			{
				if (otrosReqs [i] > 0)
					ret = Math.Min (ret, _recs [i] / otrosReqs [i]);
			}
			return ret;
		}

		public float ContieneRecursos (IAlmacénRead otrosReqs)
		{
			return ContieneRecursos (otrosReqs.AsArray ());
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