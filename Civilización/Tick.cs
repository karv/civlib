using System;
using System.Collections.Generic;
using Basic;

namespace Civ
{
	public partial class Civilizacion
	{
		// Ticks
		/// <summary>
		/// Realiza un FullTick en cada ciudad, además revisa ciencias aprendidas.
		/// Básicamente hace todo lo necesario y suficiente que le corresponde entre turnos.
		/// </summary>
		/// <param name="t">Diración del tick</param>
		public void doTick(float t = 1)
		{
			Random r = new Random();
			foreach (var x in Ciudades.ToArray())
			{
				{
					x.FullTick(t);
				}

				// Las ciencias.
				List<Ciencia> Investigado = new List<Ciencia>();

				foreach (Recurso Recr in Global.g_.Data.ObtenerRecursosCientíficos())
				{
					List<Ciencia> SemiListaCiencias = CienciasAbiertas().FindAll(z => z.Reqs.Rec.Nombre == Recr.Nombre);  // Lista de ciencias abiertas que usan el recurso x.
					float[] sep = r.Separadores(SemiListaCiencias.Count, ObtenerGlobalRecurso(Recr));

					int i = 0;
					foreach (var y in SemiListaCiencias)
					{
						// En este momento, se está investigando "y" con el recurso "x".
						Investigando[y] += sep[i];
						i++;

						// Si Tiene lo suficiente para terminar investigación
						if (Investigando[y] >= y.Reqs.Cantidad)
						{
							Investigado.Add(y);
						}
					}
				}

				foreach (var Avan in Investigado)
				{
					Avances.Add(Avan);
					Investigando.Data.Remove(Avan);
					Msj.Add("Investigación terminada: " + Avan.Nombre);
				}

				// Fase final, desaparecer recursos.
				foreach (Ciudad Cd in Ciudades)
				{
					Cd.DestruirRecursosTemporales();
				}
			}
		}

	}
}