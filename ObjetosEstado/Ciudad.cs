using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using Civ.Almacén;
using Civ.Global;
using Civ.IU;
using Civ.RAW;
using Civ.Topología;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Representa una instancia de ciudad.
	/// </summary>
	[Serializable]
	public class Ciudad : ICiudad
	{
		#region Inicialización

		/// <summary>
		/// Se debe ejecutar cuando Juego.Data esté totalmente cargado.
		/// </summary>
		public void Inicializar ()
		{
			DeltaRec = new AlmacénGenérico ();
		}

		#endregion

		#region ICiudad

		ICollection<IUnidadRAW> ICiudad.UnidadesConstruibles ()
		{
			return UnidadesConstruibles ().Keys;
		}

		string _nombre;

		/// <summary>
		/// Devuelve o establece el nombre de la ciudad.
		/// </summary>
		/// <value>The nombre.</value>
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
		/// Devuelve una copia de la info poblacional
		/// </summary>
		/// <value>The get poblacion info.</value>
		[Obsolete ("Usar Población")]
		public InfoPoblación GetPoblacionInfo
		{ get { return Población; } }

		ICollection<TrabajoRAW> ICiudad.ObtenerTrabajosAbiertos ()
		{
			return TrabajosAbiertos ();
		}

		/// <summary>
		/// Devuelve la armada inmovil que representa la defensa de la ciudad.
		/// </summary>
		/// <value>The defensa.</value>
		public Armada Defensa
		{
			get
			{
				return _defensa;
			}
		}

		Armada _defensa;

		#endregion

		#region IPosicionable implementation

		/// <summary>
		/// Obtener la posicion de la ciudad.
		/// </summary>
		public Pseudoposición Posición ()
		{
			return Terr.Pos;
		}

		#endregion

		#region General

		/// <summary>
		/// Posición de la ciudad.
		/// </summary>
		/// <value>The position.</value>
		public Pseudoposición Pos
		{
			get
			{
				return Terr.Pos;
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Civ.ObjetosEstado.Ciudad"/>.
		/// </summary>
		/// <returns>El nombre de la ciudad </returns>
		public override string ToString ()
		{
			return Nombre;
		}

		ICivilización _civDueño;

		/// <summary>
		/// Devuelve o establece la civilización a la cual pertecene esta ciudad.
		/// </summary>
		/// <value>The civ dueño.</value>
		public ICivilización CivDueño
		{
			get
			{
				return _civDueño;
			}
			set
			{
				AlCambiarDueño?.Invoke (
					this,
					new TransferenciaObjetoEventArgs (
						_civDueño,
						value,
						this));
				
				if (_civDueño != null)
					_civDueño.Ciudades.Remove (this);
				_civDueño = value;
				if (_civDueño != null)
					_civDueño.Ciudades.Add (this);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.ObjetosEstado.Ciudad"/> class.
		/// Usa nombre únici de ciudad
		/// </summary>
		/// <param name="dueño">Civilización dueño</param>
		/// <param name="t">Terreno</param>
		/// <param name="inipop">Población inicial</param>
		public Ciudad (ICivilización dueño, Terreno t, float inipop = 1)
			: this (Juego.NombreCiudadÚnico (), dueño, t, inipop)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.ObjetosEstado.Ciudad"/> class.
		/// </summary>
		/// <param name="nombre">Nombre de la ciudad.</param>
		/// <param name="dueño">Civ a la que pertenece esta ciudad.</param>
		/// <param name="terreno">Terreno de contrucción de la ciudad.</param>
		/// <param name="iniPop"> Población (de edad productiva) inicial </param>
		public Ciudad (string nombre,
		               ICivilización dueño,
		               Terreno terreno,
		               float iniPop = 1)
		{
			Población = new InfoPoblación (iniPop);
			Edificios = new HashSet<Edificio> ();
			Propiedades = new HashSet<Propiedad> ();
			CivDueño = dueño;
			Nombre = nombre;
			_almacén = new AlmacénCiudad (this);
			terreno.CiudadConstruida = this;
			Terr = terreno;

			// Inicializar la armada
			_defensa = new Armada (this, true);
			Defensa.Posición.DesdeGrafo (terreno);

			// Importar propiedades desde T.

			foreach (var x in terreno.Innatos)
			{
				// Si r.next < (algo):
				AgregaPropiedad (x);
			}

			IntentaConstruirAutoconstruibles (); // Construir autoconstruibles
		}

		/// <summary>
		/// Terreno donde se contruye la ciudad.
		/// </summary>
		public Terreno Terr { get; set; }


		/// <summary>
		/// Devuelve un nuevo diccionario cuyas entradas son el número de unidades que puede construir la ciudad, por cada unidad.
		/// </summary>
		/// <returns>The construibles.</returns>
		public Dictionary<IUnidadRAW, long> UnidadesConstruibles ()
		{
			var ret = new Dictionary<IUnidadRAW, long> ();

			foreach (var x in Juego.Data.Unidades)
			{
				if (x.EstaDisponible (CivDueño))
					ret.Add (x, x.MaxReclutables (this));
			}
			return ret;
		}

		/// <summary>
		/// Devuelve la cantidad de unidades que puede construir esta ciudad de una unidadRAW específica.
		/// Tiene en cuenta sólo los recursos y la población desocupada.
		/// </summary>
		/// <returns>The construibles.</returns>
		/// <param name="unid">Unid.</param>
		public long UnidadesConstruibles (IUnidadRAW unid)
		{
			return unid.MaxReclutables (this);
		}

		[OnDeserialized]
		void Defaults ()
		{
		}

		#endregion

		#region Almacén

		/// <summary>
		/// Almacén de recursos.
		/// </summary>
		public AlmacénCiudad Almacén
		{
			get
			{
				return _almacén;
			}
		}

		IAlmacén ICiudad.Almacén
		{
			get
			{
				return _almacén;
			}
		}

		AlmacénCiudad _almacén;

		/// <summary>
		/// Devuelve o establece el alimento en el almacén.
		/// </summary>
		public float AlimentoAlmacen
		{ 
			get
			{
				return Almacén [RecursoAlimento];
			}
			set
			{
				Almacén [RecursoAlimento] = value;
			}
		}

		/// <summary>
		/// DEvuelve los recursos que son visibles en la ciudad
		/// </summary>
		public ICollection<Recurso> RecursosVisibles ()
		{
			var ret = new HashSet<Recurso> ();
			ret.UnionWith (Almacén.Keys);
			ret.UnionWith (CivDueño.Almacén.Entradas);
			ret.UnionWith (Terr.Eco.AlmacénRecursos.Recursos);
			#if DEBUG
			return ret;
			#else
			return ret;
			#endif
		}

		#endregion

		#region Armada

		/// <summary>
		/// Devuelve una colección con las armadas estacionadas en la ciudad.
		/// No cuenta la Defensa inmovil
		/// </summary>
		/// <returns>The en ciudad.</returns>
		public ICollection<Armada> ArmadasEnCiudad ()
		{
			var rat = new List<Armada> ();
			foreach (var x in CivDueño.Armadas)
			{
				if (!x.EsDefensa && x.Posición.Equals (Pos))
					rat.Add (x);
			}
			return rat;
		}

		/// <summary>
		/// Entrena una cantidad de unidades de una clase fija.
		/// Incluye la unidad en la armada de la ciudad.
		/// </summary>
		/// <param name="uRAW">Clase de unidades a entrenar</param>
		/// <param name="cantidad">Cantidad</param>
		/// <returns>Devuelve un arreglo con las unidades que se pudieron entrenar.</returns>
		public Stack Reclutar (IUnidadRAW uRAW, long cantidad = 1)
		{
			// Primero construir las unidades, luego bajar la población.
			// Ya que construir unidades tiene un población-check.
			uRAW.Reclutar (cantidad, this);
			Población = Población.AgregaPoblación (-cantidad);
			AlReclutar?.Invoke (this, new ReclutarEventArgs (uRAW, cantidad));

			// Devuelve el stack la unidad creada.
			return Defensa [uRAW]; 
		}

		/// <summary>
		/// Agrega una nueva armada.
		/// </summary>
		/// <returns>La armada agregada.</returns>
		public Armada AgregaArmada ()
		{
			return new Armada (this);
		}

		#endregion

		#region Construcción

		/// <summary>
		/// Devuelve o establece el edificio que se está contruyendo, y su progreso.
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
				if (value == null || PuedeConstruir (value))
				{
					EdifConstruyendo = new EdificioConstruyendo (value, this);
					EdifConstruyendo.AlCompletar += delegate
					{
						CivDueño.AgregaMensaje (new Mensaje (
							"Edificio completado.",
							"Se terminó construcción de edificio {0} en {1}.",
							TipoRepetición.NoTipo,
							null,
							null,
							value.Nombre,
							Nombre));
					};
					AlCambiarConstrucción?.Invoke (this, EventArgs.Empty);
				}
				else
					throw new Exception (string.Format (
						"No se puede construir {0} en {1}.",
						value,
						this));
			}
		}

		#endregion

		#region Edificios

		/// <summary>
		/// Devuelve la lista de instancias de edicio de la ciudad.
		/// </summary>
		public ICollection<Edificio> Edificios { get; }

		/// <summary>
		/// Revisa si existe una clase de edificio en esta ciudad.
		/// </summary>
		/// <param name="edif">La clase de edificio buscada</param>
		/// <returns><c>true</c> si existe el edificio, <c>false</c> si no.</returns>
		public bool ExisteEdificio (EdificioRAW edif)
		{
			foreach (var x in Edificios)
			{
				if (x.RAW == edif)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Devuelve el edificio en la ciudad con un nombre específico.
		/// </summary>
		/// <param name="edif">RAW del edificio.</param>
		/// <returns>La instancia de edificio en la ciudad; si no existe devuelve <c>null</c>.</returns>
		public Edificio EncuentraInstanciaEdificio (EdificioRAW edif)
		{
			foreach (Edificio x in Edificios)
			{
				if (x.RAW == edif)
				{
					return x;
				}
			}
			return null;
		}

		/// <summary>
		/// Agrega una instancia de edicifio a la ciudad.
		/// </summary>
		/// <returns>La instancia de edificio que se agregó.</returns>
		/// <param name="edif">RAW del edificio a agregar.</param>
		public Edificio AgregaEdificio (EdificioRAW edif)
		{
			var ret = new Edificio (edif, this);
			AlObtenerNuevoEdificio?.Invoke (this, new EdificioNuevoEventArgs (ret));
			return ret;
		}

		/// <summary>
		/// Devuelve la lista de edificios contruibles por esta ciudad; los que se pueden hacer y no están hechos.
		/// </summary>
		/// <returns></returns>
		[Obsolete ("Usar Data.ObtenerEdificiosConstruíbles(this)")]
		public ICollection<EdificioRAW> Construibles ()
		{
			var ret = new List<EdificioRAW> ();
			foreach (EdificioRAW x in Juego.Data.Edificios)
			{
				if (PuedeConstruir (x))
					ret.Add (x);
			}
			return ret;
		}

		/// <summary>
		/// Devuelve <c>true</c> si un edificio se puede contruir en esta ciudad.
		/// <c>false</c> en caso contrario.
		/// </summary>
		/// <param name="edif">Clase de edificio</param>
		public bool PuedeConstruir (EdificioRAW edif)
		{
			if (!SatisfaceReq (edif.Reqs ()))
				return false;
			if (ExisteEdificio (edif))
				return false;	// Por ahora no se permite múltiples instancias del mismo edificio en una ciudad.
			if (edif.MaxPorCivilizacion > 0 && edif.MaxPorCivilizacion <= CivDueño.CuentaEdificios (edif))
				return false;
			if (edif.MaxPorMundo > 0 && edif.MaxPorCivilizacion <= Juego.State.CuentaEdificios (edif))
				return false;
			return true;
		}

		/// <summary>
		/// Devuelve el número de edificios de una clase que se encutrnan en la ciudad.
		/// </summary>
		/// <param name="edif">Clase de edificio.</param>
		/// <returns></returns>
		public int NumEdificios (EdificioRAW edif)
		{
			int ret = 0;
			foreach (var x in Edificios)
			{
				if (x.RAW == edif)
					ret++;
			}
			return ret;
		}

		// Propiedades

		/// <summary>
		/// Devuelve la lista de Propiedades de la ciudad.
		/// </summary>
		/// <value>The propiedades.</value>
		public ICollection<Propiedad> Propiedades { get; }

		/// <summary>
		/// Revisa si existe una propiedad P en la ciudad.
		/// </summary>
		/// <returns><c>true</c>, si la propiedad existe, <c>false</c> en caso contrario.</returns>
		/// <param name="prop">La propiedad.</param>
		public bool ExistePropiedad (Propiedad prop)
		{
			foreach (var x in Propiedades)
			{
				if (x == prop)
					return true;
			}
			return false;
		}

		#region Propiedades

		/// <summary>
		/// Agrega una instancia de <c>Propiedad</c> a la ciudad.
		/// </summary>
		/// <param name="prop">Propiedad a agregar.</param>
		public void AgregaPropiedad (Propiedad prop)
		{
			if (!Propiedades.Contains (prop))
			{
				Propiedades.Add (prop);
			}
		}

		#endregion

		#endregion

		#region Población

		//Población y crecimiento.
		/// <summary>
		/// Recurso que será el alimento
		/// </summary>
		public static Recurso RecursoAlimento
		{
			get
			{
				return Juego.Data.RecursoAlimento;
			}
		}

		/// <summary>
		/// Número de infantes que nacen por (PoblaciónProductiva*Tick) Base.
		/// </summary>
		public const float TasaNatalidadBase = 0.12f;
		/// <summary>
		/// Probabilidad base de un infante arbitrario de morir en cada tick.
		/// </summary>
		public const float TasaMortalidadInfantilBase = 0.01f;
		/// <summary>
		/// Probabilidad base de un habitante productivo arbitrario de morir en cada tick.
		/// </summary>
		public const float TasaMortalidadProductivaBase = 0.02f;
		/// <summary>
		/// Probabilidad base de un adulto de la tercera edad arbitrario de morir en cada tick.
		/// </summary>
		public const float TasaMortalidadVejezBase = 0.1f;
		/// <summary>
		/// Probabilidad de que un infante se convierta en productivo
		/// </summary>
		public const float TasaDesarrolloBase = 0.2f;
		/// <summary>
		/// Probabilidad de que un Productivo envejezca
		/// </summary>
		public const float TasaVejezBase = 0.05f;
		/// <summary>
		/// Consumo base de alimento por ciudadano.
		/// </summary>
		public const float ConsumoAlimentoPorCiudadanoBase = 1f;
		/// <summary>
		/// The tasa mortalidad hambruna base.
		/// Qué tan rápido muere la gente por falta de alimento.
		/// </summary>
		public const float TasaMortalidadHambrunaBase = 0.5f;


		//Población
		/// <summary>
		/// Devuelve o establece la población.
		/// </summary>
		/// <value>The población.</value>
		public InfoPoblación Población { get; set; }

		/// <summary>
		/// Devuelve o establece la población productiva.
		/// </summary>
		/// <value>The real población productiva.</value>
		protected float RealPoblaciónProductiva
		{ get { return Población.SegundaEdad; } }

		/// <summary>
		/// Devuelve o establece la población pre-productiva.
		/// </summary>
		/// <value>The real población pre-productiva.</value>
		protected float RealPoblaciónPreProductiva
		{ get { return Población.PrimeraEdad; } }

		/// <summary>
		/// Devuelve o establece la población post-productiva.
		/// </summary>
		/// <value>The real población post-productiva.</value>
		protected float RealPoblaciónPostProductiva 
		{ get { return Población.TerceraEdad; } }

		/// <summary>
		/// Devuelve la población real y total de la ciudad.
		/// </summary>
		public float RealPoblacion
		{ get { return Población.RealTotal; } }

		/// <summary>
		/// Devuelve la población de la ciudad.
		/// </summary>
		/// <value>The get poplación.</value>
		public long TotalPoblación
		{ get { return Población.Total; } }

		#endregion

		#region Trabajadores

		/// <summary>
		/// Recluta a trabajadores desocupados a los trabajos de mayos prioridad.
		/// </summary>
		public void AutoReclutarTrabajadores ()
		{
			// Autoacomodar trabajadores desocupados
			var Lst = new List<Trabajo> (ObtenerListaTrabajos ());
			var OrderLst = Lst.OrderBy (x => x.Prioridad);

			foreach (var x in OrderLst)
			{
				x.Trabajadores = x.MaxTrabajadores;
				if (TrabajadoresDesocupados == 0)
					return;
			}
		}

		/// <summary>
		/// Devuelve en número de trabajadores ocupados en algún edificio.
		/// </summary>
		/// <value>Población ocupada.</value>
		public long NumTrabajadores
		{
			get
			{
				long ret = 0;
				foreach (var x in Edificios)
					ret += x.Trabajadores;
				return ret;
			}
		}

		/// <summary>
		/// Devuelve el número de trabajadores desocupados en la ciudad.
		/// </summary>
		/// <value>Trabajadores desocupados.</value>
		public long TrabajadoresDesocupados
		{
			get
			{
				return Población.Productiva - NumTrabajadores;
			}
		}

		/// <summary>
		/// Devuelve la lista de trabajos que se pueden realizar en una ciudad.
		/// </summary>
		public ICollection<TrabajoRAW> ObtenerListaTrabajosRAW
		{
			get
			{
				var ret = new HashSet<TrabajoRAW> ();
				foreach (var x in Edificios)
					foreach (var y in x.Trabajos)
						ret.Add (y.RAW);
				return ret;
			}
		}

		/// <summary>
		/// Devuelve la lista de trabajos actuales en esta Ciudad. 
		/// </summary>
		public ICollection<Trabajo> ObtenerListaTrabajos ()
		{
			var ret = new List<Trabajo> ();
			foreach (var x in Edificios)
			{
				foreach (var y in x.Trabajos)
				{
					ret.Add (y);
				}
			}
			return ret;
		}

		/// <summary>
		/// Devuelve la instancia de trabajo en esta ciudad, si existe. Si no, la crea y la devuelve cuando <c>CrearInstancia</c>.
		/// </summary>
		/// <param name="raw">TrabajoRAW que se busca</param>
		/// <returns>Devuelve el trabajo en la ciudad correspondiente a este TrabajoRAW.</returns>
		public Trabajo EncuentraInstanciaTrabajo (TrabajoRAW raw)
		{
			Debug.Assert (raw != null);
			EdificioRAW Ed = raw.Edificio;   // La clase de edificio que puede contener este trabajo.
			Edificio Edif = EncuentraInstanciaEdificio (Ed); // La instancia del edificio en esta ciudad.

			Debug.Assert (Edif != null);
			if (Edif == null)
				return null;    // Devuelve nulo si no existe el edificio donde se trabaja.
			foreach (Trabajo x in ObtenerListaTrabajos())
			{
				if (x.RAW == raw)
					return x;
			}
			return new Trabajo (raw, this);
		}

		/// <summary>
		/// Devuelve la instancia de trabajo en esta ciudad, si existe. Si no, la crea y la devuelve cuando <c>CrearInstancia</c>.
		/// </summary>
		/// <param name="raw"></param>
		/// Nombre del trabajo que se busca.
		/// <returns>Devuelve el trabajo en la ciudad con el nombre buscado.</returns>
		public Trabajo EncuentraInstanciaTrabajo (string raw)
		{
			TrabajoRAW Tr = Juego.Data.EncuentraTrabajo (raw);
			return Tr == null ? null : EncuentraInstanciaTrabajo (Tr);
		}

		/// <summary>
		/// Hacer que la ciudad tenga al menos un número de trabajadores libres. Liberando por prioridad.
		/// </summary>
		/// <param name="n">Número de trabajadores a forzar que sean libres.</param>
		public void LiberarTrabajadores (long n)
		{
			Debug.WriteLine ("Liberando trabajadores: " + n, "Trabajadores");
			var L = new List<Trabajo> (ObtenerListaTrabajos ().OrderBy (x => x.Prioridad));
			while (L.Count > 0 && TrabajadoresDesocupados < n && TrabajadoresDesocupados != TotalPoblación)
			{
				var rm = L.First ();
				var removing = Math.Min (n - TrabajadoresDesocupados, rm.Trabajadores);
				rm.Trabajadores -= removing;
				Debug.WriteLine (
					string.Format (
						"Liberando {0} trabajadores de {1}",
						removing,
						rm),
					"Trabajadores");
				L.Remove (rm);
			}
		}

		/// <summary>
		/// Crea una nueva lista de todos los trabajos abiertos para una ciudad, tomando en cuenta sus edificios y ciencias.
		/// threadsafe.
		/// </summary>
		/// <returns>Devuelve una nueva lista.</returns>
		public ICollection<TrabajoRAW> TrabajosAbiertos ()
		{
			var ret = new List<TrabajoRAW> ();
			foreach (var x in Edificios)
				foreach (var y in x.RAW.Trabajos.Where (z => SatisfaceReq (z.Reqs())))
					ret.Add (y);
			return ret;
		}

		#endregion

		#region Requerimientos

		/// <summary>
		/// Revisa si esta ciudad satisface un IRequerimiento.
		/// </summary>
		/// <param name="reqs">Un requerimiento</param>
		/// <returns>Devuelve <c>true</c> si esta ciudad satisface un Irequerimiento. <c>false</c> en caso contrario.</returns>
		public bool SatisfaceReq (IRequerimiento<ICiudad> reqs)
		{
			return  reqs.LoSatisface (this);
		}

		/// <summary>
		/// Revisa si esta ciudad satisface una lista de requerimientos.
		/// </summary>
		/// <param name="reqs"></param>
		/// <returns>Devuelve <c>true</c> si esta ciudad satisface todos los IRequerimiento. <c>false</c> en caso contrario.</returns>
		public bool SatisfaceReq (ICollection<IRequerimiento<ICiudad>> reqs)
		{
			foreach (var x in reqs)
			{
				if (!x.LoSatisface (this))
					return false;
			}
			return true;
		}

		#endregion

		#region Puntuación

		float IPuntuado.Puntuación
		{
			get
			{
				float ret = 0;
				// Recursos
				foreach (var x in Almacén.Keys)
					ret += x.Valor * Almacén [x];

				//Edificios
				foreach (IPuntuado x in Edificios)
					ret += x.Puntuación;

				// Población
				ret += Población.Puntuación;

				return ret;
			}
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre cuando el nombre de la ciudad es cambiado
		/// </summary>
		public event EventHandler AlCambiarNombre;

		/// <summary>
		/// Ocurre cuando esta ciudad cambia de dueño
		/// </summary>
		public event EventHandler<TransferenciaObjetoEventArgs> AlCambiarDueño;

		/// <summary>
		/// Ocurre cuando se recluta unidades en esta ciudad
		/// </summary>
		public event EventHandler<ReclutarEventArgs> AlReclutar;

		/// <summary>
		/// Ocurre cuando se cambia un proyecto de construcción
		/// </summary>
		public event EventHandler AlCambiarConstrucción;

		/// <summary>
		/// Ocurre cuando hay un edificio nuevo en la ciudad
		/// </summary>
		public event EventHandler<EdificioNuevoEventArgs> AlObtenerNuevoEdificio;

		/// <summary>
		/// Ocurre cuando la ciudad se convierte en ruinas
		/// </summary>
		public event EventHandler AlConvertirseRuinas;

		#endregion

		#region DeltaRecurso

		/// <summary>
		/// Cambio de recursos
		/// </summary>
		[NonSerialized]
		AlmacénGenérico DeltaRec = new AlmacénGenérico ();

		/// <summary>
		/// Devuelve el cambio de recursos que se espera por hora.
		/// </summary>
		/// <param name="recurso">Recurso a calcular</param>
		public float CalculaDeltaRecurso (Recurso recurso)
		{
			return DeltaRec [recurso];
		}

		#endregion

		#region ITickable

		// Tick
		/// <summary>
		/// Da un tick poblacional
		/// </summary>
		/// <param name="t">Diración del tick</param>
		public void PopTick (TimeSpan t)
		{
			// Se está suponiendo crecimiento constante entre ticks!!!
			float totalHours = (float)t.TotalHours;

			//Crecimiento prometido por sector de edad.
			var crecimiento = new float[3];
			float Consumo = TotalPoblación * ConsumoAlimentoPorCiudadanoBase * totalHours;

			#if DEBUG
			if (float.IsInfinity (AlimentoAlmacen) || float.IsNaN (AlimentoAlmacen))
				Debugger.Break ();
			Debug.Assert (
				!float.IsInfinity (AlimentoAlmacen) && !float.IsNaN (AlimentoAlmacen),
				"Se acaba de obtener alimento infinito.");
			#endif
			//Que coman
			//Si tienen qué comer
			if (Consumo <= AlimentoAlmacen)
			{
				Almacén [RecursoAlimento] -= Consumo;
			}
			else
			{
				//El porcentage de muertes
				float pctMuerte = (1 - (AlimentoAlmacen / (TotalPoblación * ConsumoAlimentoPorCiudadanoBase))) * TasaMortalidadHambrunaBase;
				if (!(0 <= pctMuerte && pctMuerte <= 1))
					Debugger.Break ();
				Debug.Assert (0 <= pctMuerte && pctMuerte <= 1, "wat?");
				AlimentoAlmacen = 0;
				//Promesas de muerte por sector.
				crecimiento [0] -= Población.PrimeraEdad * pctMuerte;
				crecimiento [1] -= Población.SegundaEdad * pctMuerte;
				crecimiento [2] -= Población.TerceraEdad * pctMuerte;
			}

			//Crecimiento poblacional
			//Infantil a productivo.
			float Desarrollo = TasaDesarrolloBase * Población.PrimeraEdad * totalHours;
			crecimiento [0] -= Desarrollo;
			crecimiento [1] += Desarrollo;
			//Productivo a viejo
			float Envejecer = TasaVejezBase * Población.SegundaEdad * totalHours;
			crecimiento [1] -= Envejecer;
			crecimiento [2] += Envejecer;
			//Nuevos infantes
			float Natalidad = TasaNatalidadBase * Población.SegundaEdad * totalHours;
			crecimiento [0] += Natalidad;
			//Mortalidad
			crecimiento [0] -= Población.PrimeraEdad * TasaMortalidadInfantilBase * totalHours;
			crecimiento [1] -= Población.SegundaEdad * TasaMortalidadProductivaBase * totalHours;
			crecimiento [2] -= Población.TerceraEdad * TasaMortalidadVejezBase * totalHours;

			// Aplicar cambios.
			// Población que tendrá después del tick
			var futProd = (long)(RealPoblaciónProductiva + crecimiento [1]);
			var decrec = Math.Max (0, Población.Productiva - futProd);
			if (decrec > TrabajadoresDesocupados)
			{
				CivDueño.AgregaMensaje (new Mensaje (
					"Reasignación de población.",
					"La ciudad {0} ha perdido trabajadores productivos ocupados.",
					TipoRepetición.PerderPoblaciónOcupada,
					this,
					this));
				LiberarTrabajadores ((long)decrec - TrabajadoresDesocupados);
			}

			Población = Población.AgregaPoblación (crecimiento);
		}

		/// <summary>
		/// Da un tick hereditario.
		/// </summary>
		public void ResourceTick (TiempoEventArgs t)
		{
			foreach (ITickable x in Edificios)
			{
				x.Tick (t);
			}

			foreach (var x in Propiedades)
			{
				x.Tick (Almacén, t.GameTime);
			}
			// Construir edificio.
			if (EdifConstruyendo != null)
			{
				EdifConstruyendo.AbsorbeRecursos ();
				if (EdifConstruyendo.EstáCompletado ())
				{
					EdifConstruyendo.Completar ();
					EdifConstruyendo = null;    //  Ya no se contruye edificio. Para evitar error de duplicidad.
				}
			}
		}

		/// <summary>
		/// Intenta construir edificios autoconstruibles
		/// </summary>
		public void IntentaConstruirAutoconstruibles ()
		{
			// Autocontruible
			// Obtener lista de edificios autocontruibles no construidos.
			var PosiblesEdif = new HashSet<EdificioRAW> (); 
			foreach (var x in Juego.Data.EdificiosAutoconstruibles())
			{
				if (!ExisteEdificio (x))
					PosiblesEdif.Add (x);
			}
			foreach (var x in PosiblesEdif)
			{
				if (PuedeConstruir (x))
				{	// Si satisface requerimientos de construcción:
					AgregaEdificio (x);
				}
			}
		}

		/// <summary>
		/// Destruye los recursos con el flag <c>.desaparecen</c>.
		/// </summary>
		[Obsolete]
		public void DestruirRecursosTemporales ()
		{
			// Desaparecen algunos recursos
			var Alm = new List<Recurso> (Almacén.Keys.Where (x => x.Desaparece));
			foreach (Recurso x in Alm)
				Almacén [x] = 0;

		}

		/// <summary>
		/// Ejecuta PopTick (), ResourseTick Y calcula delta
		/// En ese orden.
		/// </summary>
		public void Tick (TiempoEventArgs t)
		{
			AlTickAntes?.Invoke (this, t);
			var RecAntes = new Dictionary<Recurso, float> (Almacén);
			PopTick (t.GameTime);
			ResourceTick (t);

			// Hacer la suma manual 
			foreach (var x in DeltaRec)
			{
				DeltaRec [x.Key] = (Almacén [x.Key] - (RecAntes [x.Key]) / (float)t.GameTime.TotalHours);
			}
			for (int i = 0; i < DeltaRec.Count; i++)
				if (CivDueño != null && TotalPoblación == 0)
				{		// Si la población de una ciudad llega a cero, se hacen ruinas (ciudad sin civilización)
					{
						AlConvertirseRuinas?.Invoke (this, EventArgs.Empty);
						CivDueño.RemoveCiudad (this);
					}
				}
			AlTickDespués?.Invoke (this, t);
		}

		/// <summary>
		/// Ocurre antes del tick
		/// </summary>
		public event EventHandler<TiempoEventArgs> AlTickAntes;

		/// <summary>
		/// Ocurre después del tick
		/// </summary>
		public event EventHandler<TiempoEventArgs> AlTickDespués;

		#endregion
	}

}