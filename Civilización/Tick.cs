using System;
using System.Collections.Generic;
using Basic;

namespace Civ
{
	public partial class Civilizacion: ITickable
	{
		// Ticks
		/// <summary>
		/// Realiza un FullTick en cada ciudad, además revisa ciencias aprendidas.
		/// Básicamente hace todo lo necesario y suficiente que le corresponde entre turnos.
		/// </summary>
		/// <param name="t">Diración del tick</param>
		public void Tick(float t = 1)
		{
			Random r = new Random();
			foreach (var x in Ciudades.ToArray())
			{
				{
					x.FullTick(t);
				}
			}

			// Las ciencias.
			List<Ciencia> Investigado = new List<Ciencia>();

			foreach (Recurso Rec in Global.g_.Data.ObtenerRecursosCientificos())
			{
				// Lista de ciencias abiertas que aún requieren el recurso Rec.
				List<Ciencia> CienciaInvertibleRec = CienciasAbiertas().FindAll(z => z.Reqs.Recursos.ContainsKey(Rec) && // Que la ciencia requiera de tal recurso
					(!Investigando.Exists(w => w.Ciencia == z) ||
					Investigando.EncuentraInstancia(z)[Rec] < z.Reqs.Recursos[Rec])); // Y que aún le falte de tal recurso.
				float[] sep = r.Separadores(CienciaInvertibleRec.Count, Almacen[Rec]);

				int i = 0;
				foreach (var y in CienciaInvertibleRec)
				{
					// En este momento, se está investigando "y" con el recurso "Rec".
					Investigando.Invertir(y, Rec, sep[i++]);
				}
			}

			// Revisar cuáles ciencias se investigaron
			foreach (var x in CienciasAbiertas ())
			{
				if (SatisfaceRequerimientosRecursos(x))
					Investigado.Add(x);
			}

			// Agregar las ciencias termiandas a la lista Investigado
			foreach (Ciencia Avan in Investigado)
			{
				Avances.Add(Avan);
				Investigando.RemoveAll(x => x.Ciencia == Avan);
				AgregaMensaje("Investigación terminada: {0}", Avan);
			}

			// Fase final, desaparecer recursos.
			Almacen.RemoverRecursosDesaparece();
		}
	}
}