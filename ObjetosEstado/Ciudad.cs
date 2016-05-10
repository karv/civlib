using System;
using System.Collections.Generic;
using Civ.Global;
using Civ.RAW;
using ListasExtra;
using ListasExtra.Extensiones;
using System.Linq;
using System.Runtime.Serialization;
using Civ.Almacén;
using Civ.Topología;
using Civ.IU;
using System.Security.Policy;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Representa una instancia de ciudad.
	/// </summary>
	[Serializable]
	public class Ciudad : ICiudad
	{
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
				AlCambiarNombre?.Invoke ();
			}
		}

		public InfoPoblación GetPoblacionInfo
		{ 
			get
			{
				return new InfoPoblación (
					PoblacionPreProductiva,
					PoblacionProductiva,
					PoblacionPostProductiva);
			}
		}

		ICollection<TrabajoRAW> ICiudad.ObtenerTrabajosAbiertos ()
		{
			return TrabajosAbiertos ();
		}

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
				if (_civDueño != null)
					_civDueño.Ciudades.Remove (this);
				_civDueño = value;
				if (_civDueño != null)
					_civDueño.Ciudades.Add (this);
				AlCambiarDueño?.Invoke ();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Ciudad"/> class.
		/// Usa nombre únici de ciudad
		/// </summary>
		/// <param name="dueño">Dueño.</param>
		/// <param name="t">T.</param>
		/// <param name="inipop">Inipop.</param>
		public Ciudad (ICivilización dueño, Terreno t, float inipop = 1)
			: this (Juego.NombreCiudadÚnico (), dueño, t, inipop)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Ciudad"/> class.
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
			RealPoblaciónProductiva = iniPop;
			Edificios = new HashSet<Edificio> ();
			Propiedades = new HashSet<Propiedad> ();
			CivDueño = dueño;
			Nombre = nombre;
			_almacén = new AlmacénCiudad (this);
			terreno.CiudadConstruida = this;
			Terr = terreno;

			// Inicializar la armada
			_defensa = new Armada (this, true);
			Defensa.Posición.FromGrafica (terreno);

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
		public Dictionary<IUnidadRAW, ulong> UnidadesConstruibles ()
		{
			var ret = new Dictionary<IUnidadRAW, ulong> ();

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
		public ulong UnidadesConstruibles (IUnidadRAW unid)
		{
			return unid.MaxReclutables (this);
		}

		[OnDeserialized]
		void Defaults ()
		{
			DeltaRec = new ListaPeso<Recurso> ();
		}

		#endregion

		#region Almacén

		/// <summary>
		/// Almacén de recursos.
		/// </summary>
		public IAlmacén Almacén
		{
			get
			{
				return _almacén;
			}
		}

		IAlmacén _almacén;

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
		public Stack Reclutar (IUnidadRAW uRAW, ulong cantidad = 1)
		{
			uRAW.Reclutar (cantidad, this);
			RealPoblaciónProductiva -= cantidad;

			AlReclutar?.Invoke (uRAW, cantidad);

			return Defensa [uRAW]; // Devuelve la unidad creada.
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
					AlCambiarConstrucción?.Invoke ();
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
			AlObtenerNuevoEdificio?.Invoke (ret);
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

		public const float TasaMortalidadHambrunaBase = 0.5f;


		//Población
		protected float RealPoblaciónProductiva { get; set; }

		protected float RealPoblaciónPreProductiva { get; set; }

		protected float RealPoblaciónPostProductiva { get; set; }

		/// <summary>
		/// Devuelve la población real y total de la ciudad.
		/// </summary>
		public float RealPoblacion
		{
			get
			{
				return RealPoblaciónProductiva + RealPoblaciónPreProductiva + RealPoblaciónPostProductiva;
			}
		}

		/// <summary>
		/// Devuelve la población de la ciudad.
		/// </summary>
		/// <value>The get poplación.</value>
		public ulong Poblacion
		{
			get
			{
				return PoblacionProductiva + PoblacionPreProductiva + PoblacionPostProductiva;
			}
		}

		/// <summary>
		/// Devuelve la población productiva.
		/// </summary>
		/// <value></value>
		public ulong PoblacionProductiva
		{
			get
			{
				return (ulong)Math.Floor (RealPoblaciónProductiva);
			}
		}

		/// <summary>
		/// Devuelve la población pre productiva.
		/// </summary>
		/// <value></value>
		public ulong PoblacionPreProductiva
		{
			get
			{
				return (ulong)Math.Floor (RealPoblaciónPreProductiva);
			}
		}

		/// <summary>
		/// Devuelve la población post productiva.
		/// </summary>
		/// <value></value>
		public ulong PoblacionPostProductiva
		{
			get
			{
				return (ulong)Math.Floor (RealPoblaciónPostProductiva);
			}
		}

		#endregion

		#region Trabajadores

		/// <summary>
		/// Recluta a trabajadores desocupados a los trabajos de mayos prioridad.
		/// </summary>
		public void AutoReclutarTrabajadores ()
		{
			// Autoacomodar trabajadores desocupados
			var Lst = new HashSet<Trabajo> (ObtenerListaTrabajos ());
			var OrderLst = Lst.OrderBy (x => x.Prioridad);

			//FIXME: Esto
			/*
			for (int i = 0; i < Lst.Count && TrabajadoresDesocupados > 0; i++)
			{
				Lst [i].Trabajadores = Lst [i].MaxTrabajadores;
			}
			*/
		}

		/// <summary>
		/// Devuelve en número de trabajadores ocupados en algún edificio.
		/// </summary>
		/// <value>Población ocupada.</value>
		public ulong NumTrabajadores
		{
			get
			{
				ulong ret = 0;
				foreach (var x in Edificios)
				{
					ret += x.Trabajadores;
				}
				return ret;
			}
		}

		/// <summary>
		/// Devuelve el número de trabajadores desocupados en la ciudad.
		/// </summary>
		/// <value>Trabajadores desocupados.</value>
		public ulong TrabajadoresDesocupados
		{
			get
			{
				return PoblacionProductiva - NumTrabajadores;
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
			System.Diagnostics.Debug.Assert (raw != null);
			EdificioRAW Ed = raw.Edificio;   // La clase de edificio que puede contener este trabajo.
			Edificio Edif = EncuentraInstanciaEdificio (Ed); // La instancia del edificio en esta ciudad.

			System.Diagnostics.Debug.Assert (Edif != null);
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
		public void LiberarTrabajadores (ulong n)
		{
			var L = new HashSet<Trabajo> (ObtenerListaTrabajos ().OrderBy (x => x.Prioridad));
			while (L.Count > 0 && TrabajadoresDesocupados < n && TrabajadoresDesocupados != Poblacion)
			{
				var rm = L.First ();
				var removing = Math.Min (n - TrabajadoresDesocupados, rm.Trabajadores);
				rm.Trabajadores -= removing;
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
			foreach (var x in Juego.Data.Trabajos ())
			{
				if (SatisfaceReq (x.Reqs ()) && ExisteEdificio (x.Edificio))
				{
					ret.Add (x);
				}
			}
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
				foreach (var x in Almacén.Recursos)
				{
					ret += x.Valor * Almacén [x];
				}

				// Población
				ret += PoblacionPreProductiva * 2 + PoblacionProductiva * 3 + PoblacionPostProductiva;

				return ret;
			}
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre cuando el nombre de la ciudad es cambiado
		/// </summary>
		public event Action AlCambiarNombre;

		/// <summary>
		/// Ocurre cuando esta ciudad cambia de dueño
		/// </summary>
		public event Action AlCambiarDueño;

		/// <summary>
		/// Ocurre cuando se recluta unidades en esta ciudad
		/// </summary>
		public event Action<IUnidadRAW, ulong> AlReclutar;

		/// <summary>
		/// Ocurre cuando se cambia un proyecto de construcción
		/// </summary>
		public event Action AlCambiarConstrucción;

		/// <summary>
		/// Ocurre cuando hay un edificio nuevo en la ciudad
		/// </summary>
		public event Action<Edificio> AlObtenerNuevoEdificio;

		/// <summary>
		/// Ocurre cuando la ciudad se convierte en ruinas
		/// </summary>
		public event Action AlConvertirseRuinas;

		#endregion

		#region DeltaRecurso

		/// <summary>
		/// Cambio de recursos
		/// </summary>
		[NonSerialized]
		ListaPeso<Recurso> DeltaRec = new ListaPeso<Recurso> ();

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

			//Crecimiento prometido por sector de edad.
			var Crecimiento = new float[3];
			float Consumo = Poblacion * ConsumoAlimentoPorCiudadanoBase * (float)t.TotalHours;

			if (float.IsInfinity (AlimentoAlmacen) || float.IsNaN (AlimentoAlmacen))
			{
				System.Diagnostics.Debugger.Break ();
			}
			System.Diagnostics.Debug.Assert (
				!float.IsInfinity (AlimentoAlmacen) && !float.IsNaN (AlimentoAlmacen),
				"Se acaba de obtener alimento infinito.");
			//Que coman
			//Si tienen qué comer
			if (Consumo <= AlimentoAlmacen)
			{
				Almacén [RecursoAlimento] -= Consumo;
			}
			else
			{
				//El porcentage de muertes
				float pctMuerte = (1 - (AlimentoAlmacen / (Poblacion * ConsumoAlimentoPorCiudadanoBase))) * TasaMortalidadHambrunaBase;
				if (!(0 <= pctMuerte && pctMuerte <= 1))
					System.Diagnostics.Debugger.Break ();
				System.Diagnostics.Debug.Assert (0 <= pctMuerte && pctMuerte <= 1, "wat?");
				AlimentoAlmacen = 0;
				//Promesas de muerte por sector.
				Crecimiento [0] -= PoblacionPreProductiva * pctMuerte;
				Crecimiento [1] -= PoblacionProductiva * pctMuerte;
				Crecimiento [2] -= PoblacionPostProductiva * pctMuerte;
			}

			//Crecimiento poblacional
			//Infantil a productivo.
			float Desarrollo = TasaDesarrolloBase * PoblacionPreProductiva * (float)t.TotalHours;
			Crecimiento [0] -= Desarrollo;
			Crecimiento [1] += Desarrollo;
			//Productivo a viejo
			float Envejecer = TasaVejezBase * PoblacionProductiva * (float)t.TotalHours;
			Crecimiento [1] -= Envejecer;
			Crecimiento [2] += Envejecer;
			//Nuevos infantes
			float Natalidad = TasaNatalidadBase * PoblacionProductiva * (float)t.TotalHours;
			Crecimiento [0] += Natalidad;
			//Mortalidad
			Crecimiento [0] -= PoblacionPreProductiva * TasaMortalidadInfantilBase * (float)t.TotalHours;
			Crecimiento [1] -= PoblacionProductiva * TasaMortalidadProductivaBase * (float)t.TotalHours;
			Crecimiento [2] -= PoblacionPostProductiva * TasaMortalidadVejezBase * (float)t.TotalHours;

			// Aplicar cambios.
			// Población que tendrá después del tick
			var futProd = (ulong)(RealPoblaciónProductiva + Crecimiento [1]);
			ulong decrec = Math.Max (0, PoblacionProductiva - futProd);
			if (decrec > TrabajadoresDesocupados)
			{
				CivDueño.AgregaMensaje (new IU.Mensaje (
					"La ciudad {0} ha perdido trabajadores productivos ocupados.",
					new RepetidorCiudadNoPop (this),
					this));
				LiberarTrabajadores (decrec - TrabajadoresDesocupados);

			}

			RealPoblaciónPreProductiva = Math.Max (
				RealPoblaciónPreProductiva + Crecimiento [0],
				0);
			RealPoblaciónProductiva = Math.Max (
				RealPoblaciónProductiva + Crecimiento [1],
				0);
			RealPoblaciónPostProductiva = Math.Max (
				RealPoblaciónPostProductiva + Crecimiento [2],
				0);
		}

		/// <summary>
		/// Da un tick hereditario.
		/// </summary>
		public void ResourceTick (TimeSpan t)
		{
			foreach (ITickable x in Edificios)
			{
				x.Tick (t);
			}

			foreach (var x in Propiedades)
			{
				x.Tick (Almacén, t);
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
			var Alm = new List<Recurso> (Almacén.Recursos);
			foreach (Recurso x in Alm)
			{
				if (x.Desaparece)
				{
					Almacén [x] = 0;
				}
			}

		}

		/// <summary>
		/// Ejecuta PopTick (), ResourseTick Y calcula delta
		/// En ese orden.
		/// </summary>
		public void Tick (TimeSpan t)
		{
			AlTickAntes?.Invoke (t);
			var dictTmp = ((IDictionary<Recurso, float>)Almacén).Clonar ();
			var RecAntes = new ListaPeso<Recurso> (dictTmp);
			PopTick (t);
			ResourceTick (t);

			// Hacer la suma manual 
			dictTmp = ((IDictionary<Recurso, float>)Almacén).Clonar ();

			DeltaRec.Clear ();
			foreach (var x in dictTmp)
			{
				DeltaRec.Add (x);
			}

			foreach (var x in RecAntes)
			{
				DeltaRec [x.Key] = (DeltaRec [x.Key] - x.Value);
			}

			foreach (var x in new List<Recurso> (DeltaRec.Keys))
			{
				DeltaRec [x] /= (float)(t.TotalHours);
			}
			//var xx = tmp + alm;
			//DeltaRec = tmp + alm;
			//DeltaRec = ((ListaPeso<Recurso>)(Almacén) + (-1f) * RecAntes);// * (float)(t.TotalHours);
			if (CivDueño != null && Poblacion == 0)
			{		// Si la población de una ciudad llega a cero, se hacen ruinas (ciudad sin civilización)
				{
					AlConvertirseRuinas?.Invoke ();
					CivDueño.RemoveCiudad (this);
				}
			}
			AlTickDespués?.Invoke (t);
		}

		/// <summary>
		/// Ocurre antes del tick
		/// </summary>
		public event Action<TimeSpan> AlTickAntes;

		/// <summary>
		/// Ocurre después del tick
		/// </summary>
		public event Action<TimeSpan> AlTickDespués;

		#endregion
	}
}