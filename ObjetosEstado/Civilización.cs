using System;
using System.Collections.Generic;
using Civ.Global;
using Civ.Ciencias;
using Civ.IU;
using ListasExtra.Extensiones;
using System.Linq;
using System.Runtime.Serialization;
using Civ.Almacén;
using Civ.RAW;
using Civ.Topología;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Representa una civilización jugable
	/// </summary>
	[Serializable]
	public class Civilización : ICivilización
	{
		#region ICivilización

		IDiplomacia ICivilización.Diplomacia
		{
			get
			{
				return Diplomacia;
			}
		}

		bool ICivilización.EsBárbaro
		{
			get
			{
				return false;
			}
		}

		string _nombre;

		/// <summary>
		/// Nombre de la civilización
		/// </summary>
		public string Nombre
		{
			get
			{
				return _nombre;
			}
			set
			{
				_nombre = value;
				AlCambiarNombre?.Invoke (this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Devuelve una lista con las ciudades de la civilización
		/// </summary>
		/// <value>The ciudades.</value>
		public IList<ICiudad> Ciudades { get; }

		/// <summary>
		/// Devuelve una colección con las armadas
		/// </summary>
		/// <value>The armadas.</value>
		public ICollection<Armada> Armadas
		{
			get
			{
				return _armadas;
			}
			private set{ _armadas = value; }
		}

		ICollection<Armada> _armadas;

		/// <summary>
		/// Devuelve el modelo diplomático.
		/// </summary>
		/// <value>The diplomacia.</value>
		public ControlDiplomacia Diplomacia { get; }

		/// <summary>
		/// Devuelve los avances científicos/culturales que posee la civilización
		/// </summary>
		/// <value>The avances.</value>
		public ICollection<Ciencia> Avances { get; }

		/// <summary>
		/// El almacén de recursos globales.
		/// </summary>
		public AlmacénCiv Almacén { get; }

		/// <summary>
		/// Destruye esta civilización.
		/// Elimina del mapa a todas sus armadas.
		/// </summary>
		public void Destruirse ()
		{
			Juego.State.Civs.Remove (this);
			foreach (var x in Armadas)
				((IDisposable)x.Posición).Dispose ();
		}

		#endregion

		#region General

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.ObjetosEstado.Civilización"/> class.
		/// </summary>
		public Civilización ()
		{
			Almacén = new AlmacénCiv (this);
			Nombre = Juego.NombreCivÚnico ();
			Armadas = new List<Armada> ();
			Avances = new List<Ciencia> ();
			Diplomacia = new ControlDiplomacia ();
			Ciudades = new List<ICiudad> ();
			MaxPeso = Juego.PrefsJuegoNuevo.MaxPesoInicial;
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Civ.ObjetosEstado.Civilización"/>.
		/// </summary>
		/// <returns>El nombre de la civilización</returns>
		public override string ToString ()
		{
			return Nombre;
		}

		/// <summary>
		/// Devuelve la cantidad que existe en la civilización de un cierto recurso.
		/// </summary>
		/// <returns>Devuelve la suma de la cantidad que existe de algún recurso sobre cada ciudad.</returns>
		/// <param name="recurso">Recurso que se quiere contar</param>
		[Obsolete ("Usar AlmacenCiv[R]")]
		public float ObtenerGlobalRecurso (Recurso recurso)
		{
			float ret = 0;
			foreach (var x in Ciudades)
			{
				ret += x.Almacén [recurso];
			}
			return ret;
		}

		/// <summary>
		/// Devuelve el (base) peso mayor que puede tener una armada
		/// </summary>
		public float MaxPeso { get; private set; }

		#endregion

		#region Defaults

		[OnDeserialized]
		void SetDefaults ()
		{
			Mensajes = new ManejadorMensajes ();
		}

		#endregion

		#region Ciencia

		/// <summary>
		/// Ciencias que han sido parcialmente investigadas.
		/// </summary>
		public ListaInvestigación Investigando = new ListaInvestigación ();

		/// <summary>
		/// Devuelve las ciencias que no han sido investigadas y que comple todos los requesitos para investigarlas.
		/// </summary>
		public ICollection<Ciencia> CienciasAbiertas ()
		{
			var ret = new List<Ciencia> ();
			foreach (Ciencia x in Juego.Data.Ciencias)
			{
				if (EsCienciaAbierta (x))
				{
					ret.Add (x);
				}
			}
			return ret;
		}

		/// <summary>
		/// Revisa si una ciencia se puede investigar.
		/// </summary>
		/// <param name="ciencia">Una ciencia</param>
		/// <returns><c>true</c> si la ciencia se puede investigar; <c>false</c> si no.</returns>
		bool EsCienciaAbierta (Ciencia ciencia)
		{
			return !Avances.Contains (ciencia) && ciencia.Reqs.Ciencias.All (Avances.Contains);
		}

		/// <summary>
		/// Devuelve true sólo si la ciencia ya está completada.
		/// </summary>
		bool SatisfaceRequerimientosRecursos (Ciencia ciencia)
		{
			return Investigando.EncuentraInstancia (ciencia)?.EstáCompletada () ?? false;
		}

		#endregion

		#region Ciudades

		/// <summary>
		/// Agrega una ciudad a esta civ.
		/// </summary>
		/// <param name="ciudad">C.</param>
		public void AddCiudad (ICiudad ciudad)
		{
			// obs: Los cambios en las listas de ciudades se hacne automáticamente.
			ciudad.CivDueño = this;
		}

		/// <summary>
		/// Quita una ciudad de la civilización, haciendo que quede sin <c>CivDueño</c>.
		/// </summary>
		/// <param name="ciudad">Ciudad a quitar.</param>
		public void RemoveCiudad (ICiudad ciudad)
		{
			if (ciudad.CivDueño == this)
			{
				ciudad.CivDueño = null;
				AlPerderCiudad?.Invoke (
					this,
					new TransferirObjetoEventArgs (
						this,
						null,
						ciudad));
			}
		}

		/// <summary>
		/// Agrega una nueva ciudad a esta civ.
		/// </summary>
		/// <returns>Devuelve la ciudad que se agregó.</returns>
		/// <param name="nombre">Nombre de la ciudad.</param>
		/// <param name="terreno">Terreno donde se construye la ciudad</param>
		public Ciudad AddCiudad (string nombre, Terreno terreno)
		{
			var ret = new Ciudad (nombre, this, terreno);
			AlGanarCiudad?.Invoke (
				this,
				new TransferirObjetoEventArgs (null, this, ret));
			return ret;
		}

		/// <summary>
		/// Cuenta el número de edificios que existen en la ciudad.
		/// </summary>
		/// <param name="edif"></param>
		public int CuentaEdificios (EdificioRAW edif)
		{
			int ret = 0;
			foreach (var x in Ciudades)
			{
				ret += x.NumEdificios (edif);
			}
			return ret;
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre cuando se cambia el nombre
		/// </summary>
		public event EventHandler AlCambiarNombre;

		/// <summary>
		/// Ocurre cuando una ciudad se une a esta civilización
		/// </summary>
		public event EventHandler AlGanarCiudad;

		/// <summary>
		/// Ocurre cuando una ciudad se retira de esta civilización
		/// </summary>
		public event EventHandler AlPerderCiudad;

		/// <summary>
		/// Ocurre cuando se recibe un nuevo menaje
		/// </summary>
		public event EventHandler AlNuevoMensaje;

		/// <summary>
		/// Ocurre antes del tick
		/// </summary>
		public event EventHandler AlTickAntes;

		/// <summary>
		/// Ocurre después del tick
		/// </summary>
		public event EventHandler AlTickDespués;

		/// <summary>
		/// Ocurre cuando la civilización recibe un nuevo avance
		/// </summary>
		public event EventHandler AlDescubrirAvance;

		#endregion

		#region Mensajes

		/// <summary>
		/// Lista de mensajes de eventos para el usuario.
		/// </summary>
		[NonSerialized]
		public ManejadorMensajes Mensajes = new ManejadorMensajes ();

		/// <summary>
		/// Agrega un mensaje de usuario a la cola.
		/// </summary>
		/// <param name="mensaje">Mensaje</param>
		public void AgregaMensaje (Mensaje mensaje)
		{
			Mensajes.Add (mensaje);
			AlNuevoMensaje?.Invoke (this, new MensajeEventArgs (mensaje, Mensajes));
		}

		/// <summary>
		/// Agrega un mensaje de usuario a la cola.
		/// </summary>
		/// <param name="str">Cadena de texto, con formato de string.Format</param>
		/// <param name="referencia">Referencias u orígenes del mensaje.</param>
		public void AgregaMensaje (string str, params object [] referencia)
		{
			AgregaMensaje (new Mensaje (str, null, referencia));
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
		public Mensaje SiguienteMensaje ()
		{
			return Mensajes.Siguiente;
		}

		/// <summary>
		/// Devuelve todos los menajes acumulados
		/// </summary>
		/// <returns>The todos los mensajes.</returns>
		public ICollection<Mensaje> ObtenerTodosLosMensajes ()
		{
			return Mensajes;
		}

		#endregion

		#region Puntuación

		float IPuntuado.Puntuación
		{ 
			get
			{
				float ret = 0;

				// De las ciudades
				foreach (IPuntuado x in Ciudades)
					ret += x.Puntuación;


				// De las ciencias
				foreach (IPuntuado x in Avances)
					ret += x.Puntuación;

				// Armadas
				foreach (IPuntuado x in Armadas)
					ret += x.Puntuación;

				return ret;
			}
		}

		#endregion

		#region Tick

		/// <summary>
		/// Realiza un FullTick en cada ciudad, además revisa ciencias aprendidas.
		/// Básicamente hace todo lo necesario y suficiente que le corresponde entre turnos.
		/// </summary>
		/// <param name="t">Diración del tick</param>
		public void Tick (TimeEventArgs t)
		{
			AlTickAntes?.Invoke (this, t);
			foreach (var x in new List<ICiudad> (Ciudades))
			{
				x.Tick (t);
			}

			// Las ciencias.
			var Investigado = new List<Ciencia> ();

			foreach (Recurso Rec in Juego.Data.ObtenerRecursosCientificos())
			{
				// Lista de ciencias abiertas que aún requieren el recurso Rec.
				var CienciaInvertibleRec = new List<Ciencia> (CienciasAbiertas ().Where (z => z.Reqs.Recursos.ContainsKey (Rec) && // Que la ciencia requiera de tal recurso
				                           (!Investigando.Any (w => w.Ciencia == z) ||
				                           Investigando.EncuentraInstancia (z) [Rec] < z.Reqs.Recursos [Rec]))); // Y que aún le falte de tal recurso.
				if (CienciaInvertibleRec.Count > 0)
				{
					var ciencia = CienciaInvertibleRec.Aleatorio ();
					Investigando.Invertir (ciencia, Rec, Almacén [Rec]);
					Almacén [Rec] = 0;
				}
				else
				{
					if (Almacén [Rec] > 0)
					{					
						// Se está desperdiciando Rec
						AgregaMensaje (new Mensaje (
							"Se está desperdiciando recurso científico {0}",
							new RepetidorExcesoRecurso (Rec, Almacén),
							Rec.Nombre));
					}
				}
			}

			// Revisar cuáles ciencias se investigaron
			foreach (var x in CienciasAbiertas ())
			{
				if (SatisfaceRequerimientosRecursos (x))
					Investigado.Add (x);
			}

			// Agregar las ciencias termiandas a la lista Investigado
			foreach (Ciencia Avan in Investigado)
			{
				Avances.Add (Avan);
				Investigando.Remove (Avan);
				OnDescubrir (Avan);
				AgregaMensaje ("Investigación terminada: {0}", Avan);
			}

			// Fase final, desaparecer recursos.
			Almacén.RemoverRecursosDesaparece ();
			foreach (var x in Ciudades)
			{
				foreach (var y in x.Almacén.ToDictionary().Keys.Where (z => z.Desaparece))
				{
					x.Almacén [y] = 0;
				}
			}

			// Armadas

			foreach (var x in new List<Armada> (Armadas))
			{
				if (x.Unidades.Count > 0)
					x.Tick (t);
			}
			AlTickDespués?.Invoke (this, t);
		}

		/// <summary>
		/// Se ejecuta al descubrir una ciencia
		/// </summary>
		/// <param name="c">Ciencia descubierta</param>
		protected virtual void OnDescubrir (Ciencia c)
		{
			foreach (var ciudad in Ciudades)
			{
				ciudad.IntentaConstruirAutoconstruibles ();
			}
			AlDescubrirAvance?.Invoke (this, new AvanceEventArgs (c, this));
		}

		#endregion
	}
}