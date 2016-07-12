using System.Collections.Generic;
using Civ.ObjetosEstado;
using Civ.IU;
using System;

namespace Civ.Combate
{
	/// <summary>
	/// Se engarga de unir y organizar los <see cref="Civ.Combate.IAnálisisCombate"/> generados surante una pelea.
	/// </summary>
	public class AnálisisCombateManager
	{
		/// <summary>
		/// Civilización vinculada
		/// </summary>
		public readonly ICivilización Civil;
		readonly HashSet<IAnálisisCombate> combatesData = new HashSet<IAnálisisCombate> ();
		readonly HashSet<IAnálisisCombate> preData = new HashSet<IAnálisisCombate> ();

		/// <summary>
		/// Une, o agrega si es necesario, un análisis de combate a la colección.
		/// </summary>
		IAnálisisCombate addOrMerge (IAnálisisCombate combate)
		{
			foreach (var x in combatesData)
			{
				if (x.EsUnibleCon (combate))
				{
					x.UnirCon (combate);
					return x;
				}
			}
			combatesData.Add (combate);
			return combate;
		}

		/// <summary>
		/// Une un análisis de combate a esta instancia
		/// </summary>
		public void AddOrMerge (IAnálisisCombate combate)
		{
			//TODO pasarlo a predata
			preData.Add (combate);
		}

		/// <summary>
		/// </summary>
		/// <param name="civil">Civilización vinculada</param>
		public AnálisisCombateManager (ICivilización civil)
		{
			Civil = civil;
		}

		/// <summary>
		/// Devuelve el análisis acumulado hasta ahora entre atacante y defensor.
		/// </summary>
		/// <returns>The análisis.</returns>
		/// <param name="yo">Defensor, yo</param>
		/// <param name="otro">Atacante otro</param>
		public IAnálisisCombate GetAnálisis (IDefensor yo, IAtacante otro)
		{
			// TODO: deben ser, yo y otro, Armadas.
			foreach (var x in combatesData)
				if (x.ArmadaYo == yo && x.ArmadaOtro == otro)
					return x;
			throw new Exception ("No se encuentra combate.");
		}

		/// <summary>
		/// Procesa un análisis específico:
		/// Lo elimina de la lista interna y manda Mensaje a ambas civilizaciones implicadas.
		/// </summary>
		/// <param name="anal">Anal.</param>
		public void Procesar (IAnálisisCombate anal)
		{
			combatesData.Remove (anal);
			var msj = new Mensaje (
				          string.Format ("Resulado de batalla en {0}", anal.ArmadaYo.Posición),
				          anal.Análisis (),
				          TipoRepetición.AnálisisCombateCompleto,
				          this);
			Civil.AgregaMensaje (msj);
		}

		/// <summary>
		/// Se debe ejecutar entre ticks.
		/// Actualiza su info
		/// </summary>
		public void Fetch ()
		{
			var analNoUsados = new HashSet<IAnálisisCombate> (combatesData);
			foreach (var x in preData)
				analNoUsados.Remove (addOrMerge (x));
			// Procesar los análisis no usados

			foreach (var x in analNoUsados)
				Procesar (x);

			preData.Clear ();
		}


	}
}