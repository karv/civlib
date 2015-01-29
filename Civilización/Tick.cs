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
		public void doTick()
		{
			Random r = new Random();
			foreach (var x in Ciudades.ToArray()) {
				x.FullTick ();
			}

			// Las ciencias.
			List<Ciencia> Investigado = new List<Ciencia> ();

			foreach (Recurso x in Global.g_.Data.ObtenerRecursosCientíficos())
			{
				List<Ciencia> SemiListaCiencias = CienciasAbiertas().FindAll(z => z.Reqs.Rec.Nombre == x.Nombre);  // Lista de ciencias abiertas que usan el recurso x.
				float[] sep = r.Separadores(SemiListaCiencias.Count, ObtenerGlobalRecurso(x));

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

			foreach (var x in Investigado) {
				Avances.Add(x);
				Investigando.Data.Remove (x);
                Msj.Add("Investigación terminada: " + x.Nombre);
			}

			// Fase final, desaparecer recursos.
			foreach (Ciudad x in Ciudades)
			{
				x.DestruirRecursosTemporales();
			}
		}

	}
}

