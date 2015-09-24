using System;
using System.Collections.Generic;
using Basic;
using Global;
using System.Diagnostics;

namespace Civ
{
	public class Civilizacion : ICivilizacion
	{
		#region ICivilizacion

		/// <summary>
		/// Nombre de la <see cref="Civ.Civilizacion"/>.
		/// </summary>
		public string Nombre { get; set; }

		public IList<ICiudad> Ciudades
		{ 
			get
			{
				return _ciudades;
			}
				
		}

		ICollection<Armada> ICivilizacion.Armadas
		{
			get
			{
				return _Armadas;
			}
		}

		IDiplomacia ICivilizacion.Diplomacia
		{
			get
			{
				return Diplomacia;
			}
		}

		ICollection<Ciencia> ICivilizacion.Avances
		{ 
			get { return Avances; }
		}

		AlmacénCiv ICivilizacion.Almacen { get { return Almacen; } }



		#endregion

		#region General

		public readonly AlmacénCiv Almacen;

		public Civilizacion()
		{
			Almacen = new AlmacénCiv(this);
			Nombre = Juego.NombreCivUnico();
		}

		public override string ToString()
		{
			return Nombre;
		}

		/// <summary>
		/// Devuelve la cantidad que existe en la civilización de un cierto recurso.
		/// </summary>
		/// <returns>Devuelve la suma de la cantidad que existe de algún recurso sobre cada ciudad.</returns>
		/// <param name="recurso">Recurso que se quiere contar</param>
		[Obsolete("Use AlmacenCiv[R]")]
		public float ObtenerGlobalRecurso(Recurso recurso)
		{
			float ret = 0;
			foreach (IAlmacenante x in Ciudades)
			{
				ret += x.Almacen.recurso(recurso);
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

		/// <summary>
		/// Devuelve el estado diplomático de esta Civilización.
		/// </summary>
		/// <value>The _ diplomacia.</value>
		public ControlDiplomacia Diplomacia = new ControlDiplomacia();

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
			var ret = new List<Ciencia>();
			foreach (Ciencia x in Juego.Data.Ciencias)
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
		/// <param name="ciencia">Una ciencia</param>
		/// <returns><c>true</c> si la ciencia se puede investigar; <c>false</c> si no.</returns>
		bool EsCienciaAbierta(Ciencia ciencia)
		{
			return !Avances.Contains(ciencia) && ciencia.Reqs.Ciencias.TrueForAll(Avances.Contains);
		}

		/// <summary>
		/// Devuelve true sólo si la ciencia ya está completada.
		/// </summary>
		/// <returns><c>true</c>, if requerimientos recursos was satisfaced, <c>false</c> otherwise.</returns>
		bool SatisfaceRequerimientosRecursos(Ciencia ciencia)
		{
			// Si ya se conoce la ciencia, entonces devuelve true.
			if (Avances.Contains(ciencia))
				return true;

			Debug.Fail("No creo que sea posible llegar aquí");
			throw new Exception();
			//InvestigandoCiencia I = Investigando.EncuentraInstancia(ciencia);
			//return I?.EstaCompletada() ?? false;
		}

		#endregion

		#region Ciudades

		/// <summary>
		/// Lista de ciudades.
		/// </summary>
		List<ICiudad> _ciudades = new List<ICiudad>();

		/// <summary>
		/// Agrega una ciudad a esta civ.
		/// </summary>
		/// <param name="ciudad">C.</param>
		public void AddCiudad(ICiudad ciudad)
		{
			// obs: Los cambios en las listas de ciudades se hacne automáticamente.
			ciudad.CivDueño = this;
		}

		/// <summary>
		/// Quita una ciudad de la civilización, haciendo que quede sin <c>CivDueño</c>.
		/// </summary>
		/// <param name="ciudad">Ciudad a quitar.</param>
		public void RemoveCiudad(Ciudad ciudad)
		{
			if (ciudad.CivDueno == this)
				ciudad.CivDueno = null;
		}

		/// <summary>
		/// Agrega una nueva ciudad a esta civ.
		/// </summary>
		/// <returns>Devuelve la ciudad que se agregó.</returns>
		/// <param name="nombre">Nombre de la ciudad.</param>
		/// <param name="terreno">Terreno donde se construye la ciudad</param>
		public Ciudad AddCiudad(string nombre, Terreno terreno)
		{
			return new Ciudad(nombre, this, terreno);
		}

		/// <summary>
		/// Cuenta el número de edificios que existen en la ciudad.
		/// </summary>
		/// <param name="edif"></param>
		public int CuentaEdificios(EdificioRAW edif)
		{
			int ret = 0;
			foreach (var x in Ciudades)
			{
				ret += x.NumEdificios(edif);
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
		/// <param name="mensaje">Mensaje</param>
		public void AgregaMensaje(IU.Mensaje mensaje)
		{
			Mensajes.Enqueue(mensaje);
		}

		/// <summary>
		/// Agrega un mensaje de usuario a la cola.
		/// </summary>
		/// <param name="str">Cadena de texto, con formato de string.Format</param>
		/// <param name="referencia">Referencias u orígenes del mensaje.</param>
		public void AgregaMensaje(string str, params object[] referencia)
		{
			AgregaMensaje(new IU.Mensaje(str, referencia));
			OnNuevoMensaje?.Invoke();
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
			return ExisteMensaje ? (IU.Mensaje)Mensajes.Dequeue() : null;
		}

		/// <summary>
		/// Ocurre cuando se recibe un nuevo menaje
		/// </summary>
		public event Action OnNuevoMensaje;


		#endregion

		#region Puntuación

		float IPuntuado.Puntuacion
		{ 
			get
			{
				float ret = 0;

				// De las ciudades
				foreach (IPuntuado x in Ciudades)
				{
					ret += x.Puntuacion;
				}

				// De las ciencias
				ret += 100 * Avances.Count;

				return ret;
			}
		}

		#endregion

		#region Tick

		// Ticks
		/// <summary>
		/// Realiza un FullTick en cada ciudad, además revisa ciencias aprendidas.
		/// Básicamente hace todo lo necesario y suficiente que le corresponde entre turnos.
		/// </summary>
		/// <param name="t">Diración del tick</param>
		public void Tick(float t)
		{
			Random r = Juego.Rnd;
			foreach (var x in Ciudades)
			{
				{
					x.Tick(t);
				}
			}

			// Las ciencias.
			var Investigado = new List<Ciencia>();

			foreach (Recurso Rec in Juego.Data.ObtenerRecursosCientificos())
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
				AlDescubrir(Avan);
				AgregaMensaje("Investigación terminada: {0}", Avan);
			}

			// Fase final, desaparecer recursos.
			Almacen.RemoverRecursosDesaparece();

			// Armadas

			foreach (var x in new List<Armada> (Armadas))
			{
				if (x.Unidades.Count > 0)
					x.Tick(t);
			}
		}

		/// <summary>
		/// Se ejecuta al descubrir una ciencia
		/// </summary>
		/// <param name="c">Ciencia descubierta</param>
		protected virtual void AlDescubrir(Ciencia c)
		{
			foreach (var ciudad in Ciudades)
			{
				ciudad.IntentaConstruirAutoconstruibles();
			}
		}

		#endregion
	}
}
