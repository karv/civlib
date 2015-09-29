using System;
using System.Collections.Generic;

namespace Civ
{
	/// <summary>
	/// Promete habilidades para leer recursos de un almacén
	/// </summary>
	public interface IAlmacénRead
	{
		/// <summary>
		/// Devuelve la lista de recursos implicados
		/// </summary>
		/// <value>The recursos.</value>
		IEnumerable<Recurso> recursos { get; }

		/// <summary>
		/// Devuelve la cantidad existente de un recurso dado.
		/// </summary>
		/// <param name="recurso">Recurso.</param>
		float recurso(Recurso recurso);
	}

	public interface IAlmacén : IAlmacénRead
	{
		/// <summary>
		/// Establece la cantidad de recursos
		/// </summary>
		/// <param name="rec">Rec.</param>
		/// <param name="val">Value.</param>
		[ObsoleteAttribute]
		void SetRecurso(Recurso rec, float val);

		/// <summary>
		/// Cambia la cantidad de recursos por una cantidad dada.
		/// </summary>
		/// <param name="rec">Rec.</param>
		/// <param name="delta">Delta.</param>
		void ChangeRecurso(Recurso rec, float delta);
	}

	public static class ExtIAlmacén
	{
		/// <summary>
		/// Crea una copia de el almacén a un diccionario.
		/// </summary>
		/// <returns>Un diccionario con las mismas entradas que este IAlmacén.</returns>
		public static Dictionary<Recurso, float> ToDictionary(this IAlmacénRead alm)
		{
			var ret = new Dictionary<Recurso, float>();
			foreach (var x in alm.recursos)
			{
				ret.Add(x, alm.recurso(x));
			}
			return ret;
		}
	}

	public class AlmacénMáximo: AlmacenCiudad
	{
		public float Max;

		#region ctor

		public AlmacénMáximo(Ciudad ciudad) : base(ciudad)
		{
		}

		#endregion

		/// <summary>
		/// Devuelve el espacio libre.
		/// </summary>
		public float Libre()
		{
			return Max - SumaTotal();
		}

		public new float this [Recurso recurso]
		{
			get
			{
				return base[recurso];
			}
			set
			{
				base[recurso] = Math.Min(value, base[recurso] + Libre());
			}
		}
	}
}