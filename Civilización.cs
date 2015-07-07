using System;
using System.Collections.Generic;
using Basic;

namespace Civ
{
	public class Civilizacion : ITickable
	{
		#region General

		public readonly AlmacénCiv Almacen;

		public Civilizacion()
		{
			Almacen = new AlmacénCiv(this);
		}

		/// <summary>
		/// Nombre de la <see cref="Civ.Civilización"/>.
		/// </summary>
		public string Nombre;
		// **** Economía
		[Obsolete("Use AlmacenCiv[R]")]
		/// <summary>
		/// Devuelve la cantidad que existe en la civilización de un cierto recurso.
		/// </summary>
		/// <returns>Devuelve la suma de la cantidad que existe de algún recurso sobre cada ciudad.</returns>
		/// <param name="R">Recurso que se quiere contar</param>
		public float ObtenerGlobalRecurso(Recurso R)
		{
			float ret = 0;
			foreach (IAlmacenante x in Ciudades)
			{
				ret += x.Almacen.recurso(R);
			}
			return ret;
		}

		List<Armada> _Armadas = new List<Armada>();

		/// <summary>
		/// Devuelve la lista de armadas de la civ.
		/// </summary>
		/// <value>la list que enlista a las larmadas de esta civ.</value>
		public List<Armada> Armadas
		{
			get
			{
				return _Armadas;
			}
		}

		Dictionary<Civilizacion, EstadoDiplomatico> _Diplomacia = new Dictionary<Civilizacion, EstadoDiplomatico>();

		/// <summary>
		/// Devuelve el estado diplomático de esta Civilización.
		/// </summary>
		/// <value>The _ diplomacia.</value>
		public Dictionary<Civilizacion, EstadoDiplomatico> Diplomacia
		{
			get
			{
				return _Diplomacia;
			}
		}

		#endregion

		#region Ciencia

		// Avances
		/// <summary>
		/// Lista de avances de la civilización
		/// </summary>
		public List<Ciencia> Avances = new List<Ciencia>();
		/// <summary>
		/// Ciencias que han sido parcialmente investigadas.
		/// </summary>
		public ListaInvestigación Investigando = new ListaInvestigación();

		/// <summary>
		/// Devuelve las ciencias que no han sido investigadas y que comple todos los requesitos para investigarlas.
		/// </summary>
		public List<Ciencia> CienciasAbiertas()
		{
			List<Ciencia> ret = new List<Ciencia>();
			foreach (Ciencia x in Global.g_.Data.Ciencias)
			{
				if (EsCienciaAbierta(x))
				{
					ret.Add(x);
				}
			}
			return ret;
		}

		/// <summary>
		/// Revisa si una ciencia se puede investigar.
		/// </summary>
		/// <param name="C">Una ciencia</param>
		/// <returns><c>true</c> si la ciencia se puede investigar; <c>false</c> si no.</returns>
		bool EsCienciaAbierta(Ciencia C)
		{
			return !Avances.Contains(C) && C.Reqs.Ciencias.TrueForAll(z => Avances.Contains(z));
		}

		/// <summary>
		/// Devuelve true sólo si la ciencia ya está completada.
		/// </summary>
		/// <returns><c>true</c>, if requerimientos recursos was satisfaced, <c>false</c> otherwise.</returns>
		/// <param name="C">C.</param>
		bool SatisfaceRequerimientosRecursos(Ciencia C)
		{
			// Si ya se conoce la ciencia, entonces devuelve true.
			if (Avances.Contains(C))
				return true;
			InvestigandoCiencia I = Investigando.EncuentraInstancia(C);
			if (I == null)
				return false; // Si no se empieza a investigar aún, regresa false.
			return I.EstaCompletada(); // Si está en la lista, revisar si está completada.
		}

		#endregion

		#region Ciudades

		/// <summary>
		/// Lista de ciudades.
		/// </summary>
		List<Ciudad> Ciudades = new List<Ciudad>();

		/// <summary>
		/// Devuelve la lista de ciudades que pertenecen a esta <see cref="Civ.Civilización"/>.
		/// </summary>
		/// <value>The get ciudades.</value>
		public List<Ciudad> getCiudades
		{
			get
			{
				return Ciudades;
			}
		}

		/// <summary>
		/// Agrega una ciudad a esta civ.
		/// </summary>
		/// <param name="C">C.</param>
		public void addCiudad(Ciudad C)
		{
			if (C.CivDueno != this)
				C.CivDueno = this;
		}

		/// <summary>
		/// Quita una ciudad de la civilización, haciendo que quede sin <c>CivDueño</c>.
		/// </summary>
		/// <param name="C">Ciudad a quitar.</param>
		public void removeCiudad(Ciudad C)
		{
			if (C.CivDueno == this)
				C.CivDueno = null;
		}

		/// <summary>
		/// Agrega una nueva ciudad a esta civ.
		/// </summary>
		/// <returns>Devuelve la ciudad que se agregó.</returns>
		/// <param name="Nom">Nombre de la ciudad.</param>
		public Ciudad addCiudad(string Nom, Terreno T)
		{
			Ciudad C = new Ciudad(Nom, this, T);
			return C;
		}

		/// <summary>
		/// Cuenta el número de edificios que existen en la ciudad.
		/// </summary>
		/// <param name="Edif"></param>
		/// <returns></returns>
		public int CuentaEdificios(EdificioRAW Edif)
		{
			int ret = 0;
			foreach (var x in Ciudades)
			{
				ret += x.NumEdificios(Edif);
			}
			return ret;
		}

		#endregion

		#region Mensajes

		/// <summary>
		/// Lista de mensajes de eventos para el usuario.
		/// </summary>
		System.Collections.Queue Mensajes = new System.Collections.Queue();

		/// <summary>
		/// Agrega un mensaje de usuario a la cola.
		/// </summary>
		/// <param name="Mens">Mensaje</param>
		public void AgregaMensaje(IU.Mensaje Mens)
		{
			Mensajes.Enqueue(Mens);
		}

		/// <summary>
		/// Agrega un mensaje de usuario a la cola.
		/// </summary>
		/// <param name="str">Cadena de texto, con formato de string.Format</param>
		/// <param name="Ref">Referencias u orígenes del mensaje.</param>
		public void AgregaMensaje(string str, params object[] Ref)
		{
			AgregaMensaje(new IU.Mensaje(str, Ref));
			if (OnNuevoMensaje != null)
				OnNuevoMensaje.Invoke();
		}

		/// <summary>
		/// Devuelve <c>true</c> sólo si existe algún mensaje.
		/// </summary>
		public bool ExisteMensaje
		{
			get
			{
				return Mensajes.Count > 0;
			}
		}

		/// <summary>
		/// Toma de la cola el siguiente mensaje.
		/// </summary>
		/// <returns>Devuelve el mensaje siguiente en la cola.</returns>
		public IU.Mensaje SiguitenteMensaje()
		{
			if (ExisteMensaje)
			{
				IU.Mensaje ret = (IU.Mensaje)Mensajes.Dequeue();
				return ret;
			}
			else
				return null;
		}

		public event Action OnNuevoMensaje;


		#endregion

		#region Tick

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

		#endregion
	}
}
