using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Basic
{
	[DataContract(IsReference = true)]
	[Obsolete]
	public struct Par<T1, T2>
	{
		public T1 X;
		public T2 Y;

		public Par(T1 x, T2 y)
		{
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			return string.Format("({0}, {1})", X, Y);
		}
	}

	public static class Covertidor<T1, T2>
	{
		/// <summary>
		/// Convierte una lista de objetos S en la equivalente lista de objetos T, mediante un Convertidos
		/// </summary>
		/// <param name="entrada"></param>
		/// <param name="convertidor"></param>
		/// <returns></returns>
		public static List<T2> ConvertirLista(List<T1> entrada, Func<T1, T2> convertidor)
		{
			var ret = new List<T2>();

			foreach (T1 x in entrada)
			{
				ret.Add(convertidor(x));
			}
			return ret;
		}
	}

	public static class ExtRandom
	{
		/// <summary>
		/// Elije aleatoriamente $a_0, \dots, a_{Partes - 1}$ de tal forma que su suma sea Suma.
		/// </summary>
		/// <param name="r">Clase random que se usará</param>
		/// <param name="suma">La suma que debe de tener el conjunto.</param>
		/// <param name="partes">Valor máximo </param>
		/// <returns>Devuelve un arreglo pseudoaleatoriamente generado de flotantes cuya suma es 1.</returns>
		public static float[] Separadores(this Random r, int partes, float suma = 1)
		{
			//List<float> ret = new List<float>();
			var ret = new float[partes];
			float S = 0;
			for (int i = 0; i < partes; i++)
			{
				ret[i] = (float)r.NextDouble();
				S += ret[i];
			}

			for (int i = 0; i < partes; i++)
			{
				ret[i] = ret[i] * suma / S;
			}

			return ret;
		}

		/// <summary>
		/// Selecciona pseudoaleatoriamente una sublista de tamaño fijo de una lista dada.
		/// </summary>
		/// <param name="r"></param>
		/// <param name="n">Número de elementos a seleccionar.</param>
		/// <param name="lista">Lista de dónde seleccionar la sublista.</param>
		/// <returns>Devuelve una lista con los elementos seleccionados.</returns>
		public static List<object> SeleccionaPeso(this Random r, int n, ListasExtra.ListaPeso<object> lista)
		{
			List<object> ret;
			float Suma = 0;
			float rn;
			if (n == 0)
				return new List<object>();
			else
			{
				ret = r.SeleccionaPeso(n - 1, lista);

				foreach (var x in ret)
				{
					lista[x] = 0;
				}

				// Ahora seleecionar uno.
				Suma = 0;
				rn = (float)r.NextDouble() * lista.SumaTotal();

				foreach (var x in lista.Keys)
				{
					Suma += lista[x];
					if (Suma >= rn)
					{
						ret.Add(x);
						return ret;
					}
				}
				return null;
			}
		}
	}
}