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
			}

			// Las ciencias.
			List<Ciencia> Investigado = new List<Ciencia>();

			foreach (Recurso Rec in Global.g_.Data.ObtenerRecursosCientíficos())
			{
				// Lista de ciencias abiertas que aún requieren el recurso Rec.
				List<Ciencia> CienciaInvertibleRec = CienciasAbiertas().FindAll(z => z.Reqs.Recursos.ContainsKey(Rec) && Investigando[z][Rec] < z.Reqs.Recursos[Rec]);  
				float[] sep = r.Separadores(CienciaInvertibleRec.Count, Almacen[Rec]);

				int i = 0;
				foreach (var y in CienciaInvertibleRec)
				{
					// En este momento, se está investigando "y" con el recurso "Rec".
					Investigando[y][Rec] += sep[i++];
				}
			}

			foreach (var x in CienciasAbiertas ())
			{

			}

			foreach (Ciencia Avan in Investigado)
			{
				Avances.Add(Avan);
				Investigando.Data.Remove(Avan);
				AgregaMensaje("Investigación terminada: {0}", Avan);
			}

			// Fase final, desaparecer recursos.
				// TODO

				foreach (var Rec in Almacen.Keys)
			{
					Almacen[Rec] = 0;
			}

		}
	}
}