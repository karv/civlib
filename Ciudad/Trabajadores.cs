using System;
using System.Collections.Generic;

namespace Civ
{
	public partial class Ciudad
	{
		// Trabajadores
		/// <summary>
		/// Devuelve en número de trabajadores ocupados en algún edificio.
		/// </summary>
		/// <value>The get población ocupada.</value>
		public ulong getNumTrabajadores
		{
			get
			{
				ulong ret = 0;
				foreach (var x in Edificios)
				{
					ret += x.getTrabajadores;
				}
				return ret;
			}
		}

		/// <summary>
		/// Devuelve el número de trabajadores desocupados en la ciudad.
		/// </summary>
		/// <value>The get trabajadores desocupados.</value>
		public ulong getTrabajadoresDesocupados
		{
			get
			{
				return getPoblacionProductiva - getNumTrabajadores;
			}
		}

		/// <summary>
		/// Devuelve la lista de trabajos que se pueden realizar en una ciudad.
		/// </summary>
		public List<TrabajoRAW> ObtenerListaTrabajosRAW
		{
			get
			{
				List<TrabajoRAW> ret = new List<TrabajoRAW>();
				foreach (var x in Global.g_.Data.Trabajos)
				{
					List<IRequerimiento<Ciudad>> Req = new List<IRequerimiento<Ciudad>>();
					foreach (var y in x.Requiere.Requiere())
					{
						Req.Add(y);
					}

					if (SatisfaceReq(Req) && ExisteEdificio(x.Edificio))
					{
						ret.Add(x);
					}
				}
				return ret;
			}
		}

		/// <summary>
		/// Devuelve la lista de trabajos actuales en esta  <see cref="Civ.Ciudad"/>. 
		/// </summary>
		public List<Trabajo> ObtenerListaTrabajos
		{
			get
			{
				List<Trabajo> ret = new List<Trabajo>();
				foreach (var x in Edificios)
				{
					foreach (var y in x.Trabajos)
					{
						ret.Add(y);
					}
				}
				return ret;
			}

		}

		/// <summary>
		/// Devuelve la instancia de trabajo en esta ciudad, si existe. Si no, la crea y la devuelve cuando <c>CrearInstancia</c>.
		/// </summary>
		/// <param name="TRAW"></param>
		/// TrabajoRAW que se busca
		/// <param name="CrearInstancia">Si no existe tal instancia y <c>CrearInstancia</c>, la crea; si no, tira excepción.</param>
		/// <returns>Devuelve el trabajo en la ciudad correspondiente a este TrabajoRAW.</returns>
		public Trabajo EncuentraInstanciaTrabajo(TrabajoRAW TRAW)
		{
			if (TRAW == null)
				return null;
			EdificioRAW Ed = TRAW.Edificio;   // La clase de edificio que puede contener este trabajo.
			Edificio Edif = EncuentraInstanciaEdificio(Ed); // La instancia del edificio en esta ciudad.

			if (Edif == null)
				return null;    // Devuelve nulo si no existe el edificio donde se trabaja.
			foreach (Trabajo x in ObtenerListaTrabajos)
			{
				if (x.RAW == TRAW)
					return x;
			}
			return null;
		}

		/// <summary>
		/// Devuelve la instancia de trabajo en esta ciudad, si existe. Si no, la crea y la devuelve cuando <c>CrearInstancia</c>.
		/// </summary>
		/// <param name="TRAW"></param>
		/// Nombre del trabajo que se busca.
		/// <param name="CrearInstancia">Si no existe tal instancia y <c>CrearInstancia</c>, la crea; si no, tira excepción.</param>
		/// <returns>Devuelve el trabajo en la ciudad con el nombre buscado.</returns>
		public Trabajo EncuentraInstanciaTrabajo(string TRAW)
		{
			TrabajoRAW Tr = Global.g_.Data.EncuentraTrabajo(TRAW);
			if (Tr == null)
				return null;
			return EncuentraInstanciaTrabajo(Tr);
		}

		/// <summary>
		/// Hacer que la ciudad tenga al menos un número de trabajadores libres. Liberando por prioridad.
		/// </summary>
		/// <param name="n">Número de trabajadores a forzar que sean libres.</param>
		public void LiberarTrabajadores(ulong n)
		{
			List<Trabajo> L = ObtenerListaTrabajos;
			L.Sort((x, y) => x.Prioridad < y.Prioridad ? -1 : 1); // Ordenar por prioridad.
			while (L.Count > 0 && getTrabajadoresDesocupados < n && getTrabajadoresDesocupados != getPoblacion)
			{
				L[0].Trabajadores = 0;
				L.RemoveAt(0);
			}
		}
	}
}