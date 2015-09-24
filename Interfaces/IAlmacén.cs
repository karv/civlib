using System;
using System.Collections.Generic;

namespace Civ
{
	/// <summary>
	/// Promete habilidades para leer recursos de un almacén
	/// </summary>
	public interface IAlmacénRead
	{
		IEnumerable<Recurso> recursos { get; }

		float recurso(Recurso recurso);
	}

	public interface IAlmacén : IAlmacénRead
	{
		[ObsoleteAttribute]
		void SetRecurso(Recurso rec, float val);

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