using System;
using ListasExtra;

namespace Civ
{
	public partial class Ciudad
	{

		// Edificio en construcción.
		/// <summary>
		/// Representa un edificio en construcción.
		/// </summary>
		public class EdificioConstruyendo
		{
			public EdificioRAW RAW;

			/// <summary>
			/// Recursos ya usados en el edificio.
			/// </summary>
			public ListaPeso<Recurso> RecursosAcumulados = new ListaPeso<Recurso>();

			/// <summary>
			/// Devuelve la función de recursos faltantes.
			/// </summary>
			public ListaPeso<Recurso> RecursosRestantes
			{
				get
				{
					ListaPeso<Recurso> ret = new ListaPeso<Recurso>();
					foreach (var x in RAW.ReqRecursos)
					{
						Recurso r = x.Key;
						ret[r] = x.Value - RecursosAcumulados[r];
					}
					return ret;
				}
			}

			public Ciudad CiudadDueño;

			/// <summary>
			/// Crea una instancia.
			/// </summary>
			/// <param name="EdifRAW">El RAW de este edificio.</param>
			/// <param name="C">Ciudad dueño.</param>
			public EdificioConstruyendo(EdificioRAW EdifRAW, Ciudad C)
			{
				RAW = EdifRAW;
				CiudadDueño = C;
			}

			/// <summary>
			/// Absorbe los recursos de la ciudad para su construcción.
			/// </summary>
			public void AbsorbeRecursos()
			{
				foreach (Recurso x in RecursosRestantes.Keys)
				{
					float abs = Math.Min(RecursosRestantes[x], CiudadDueño.Almacén[x]);
					RecursosAcumulados[x] += abs;
					CiudadDueño.Almacén[x] -= abs;
				}
			}

			/// <summary>
			/// Revisa si este edificio está completado.
			/// </summary>
			/// <returns><c>true</c> si ya no quedan recursos restantes; <c>false</c> en caso contrario.</returns>
			public bool EstáCompletado()
			{
				return RecursosRestantes.Keys.Count == 0;
			}

			/// <summary>
			/// Contruye una instancia de su RAW en la ciudad dueño.
			/// </summary>
			/// <returns>Devuelve su edificio completado.</returns>
			public Edificio Completar()
			{
				return CiudadDueño.AgregaEdificio(RAW);
			}

			/// <summary>
			/// Devuelve el procentage construido. Número en [0,1]
			/// </summary>
			/// <returns>float entre 0 y 1.</returns>
			public float Porcentageconstruccion()
			{
				float Max = 0;
				float Act = RecursosAcumulados.SumaTotal();

				foreach (var x in RAW.ReqRecursos.Keys)
				{
					Max += RAW.ReqRecursos[x];
				}

				return Act / Max;
			}
		}

		/// <summary>
		/// Devuelve o establece El edificio que se está contruyendo, y su progreso.
		/// </summary>
		public EdificioConstruyendo EdifConstruyendo;

		/// <summary>
		/// Devuelve el RAW del edificio que se está contruyendo.
		/// </summary>
		public EdificioRAW RAWConstruyendo
		{
			get
			{
				return EdifConstruyendo == null ? null : EdifConstruyendo.RAW;
			}
			set
			{
				// TODO: ¿Qué hacer con los recursos del edificio anterior? ¿Se pierden? (por ahora sí :3)
                if (value == null || PuedeConstruir(value)) EdifConstruyendo = new EdificioConstruyendo(value, this);
                else throw new Exception (string.Format("No se puede construir {0} en {1}.", value, this));
			}
		}

	}
}

