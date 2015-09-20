using System;
using ListasExtra;
using System.Collections.Generic;
using Global;

namespace Civ
{
	/// <summary>
	/// Representa una instancia de ciudad.
	/// </summary>
	public class Ciudad : ICiudad
	{
		#region ICiudad

		ICollection<UnidadRAW> ICiudad.UnidadesConstruibles()
		{
			return UnidadesConstruibles().Keys;
		}

		public string Nombre
		{
			get
			{
				return _nombre;
			}
			set
			{
				_nombre = value;
			}
		}

		public InfoPoblacion GetPoblacionInfo
		{ 
			get
			{
				return new InfoPoblacion(getPoblacionPreProductiva, getPoblacionProductiva, getPoblacionPostProductiva);
			}
		}

		ICivilizacion ICiudad.CivDueño
		{
			get
			{
				return _CivDueño;
			}
		}

		ICollection<TrabajoRAW> ICiudad.ObtenerTrabajosAbiertos()
		{
			return obtenerTrabajosAbiertos();
		}

		Armada ICiudad.Defensa
		{
			get
			{
				return Defensa;
			}
		}

		#endregion

		#region General

		/// <summary>
		/// Posición de la ciudad.
		/// </summary>
		/// <value>The position.</value>
		public Pseudoposicion Pos
		{
			get
			{
				return Terr;
			}
		}

		public ICollection<Ciencia> Avances
		{ 
			get
			{
				return CivDueno.Avances;
			}
		}


		#region IPosicionable implementation

		public Pseudoposicion getPosicion()
		{
			return Terr;
		}

		#endregion

		public override string ToString()
		{
			return Nombre;
		}

		/// <summary>
		/// Nombre de la ciudad.
		/// </summary>
		public string _nombre;
		ICivilizacion _CivDueño;

		/// <summary>
		/// Determina si se autoreclutan trabajadores, por prioridad, al aumentar la población.
		/// </summary>
		public bool AutoReclutar = false;

		/// <summary>
		/// Devuelve o establece la civilización a la cual pertecene esta ciudad.
		/// </summary>
		/// <value>The civ dueño.</value>
		public ICivilizacion CivDueno
		{
			get
			{
				return _CivDueño;
			}
			set
			{
				if (_CivDueño != null)
					_CivDueño.Ciudades.Remove(this);
				_CivDueño = value;
				if (_CivDueño != null)
					_CivDueño.Ciudades.Add(this);
			}
		}

		public Ciudad(ICivilizacion dueño, Terreno t, float inipop = 1)
			: this(g_.getUniqueCityName(), dueño, t, inipop)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.Ciudad"/> class.
		/// </summary>
		/// <param name="Nom">Nombre de la ciudad.</param>
		/// <param name="Dueño">Civ a la que pertenece esta ciudad.</param>
		/// <param name="T">Terreno de contrucción de la ciudad.</param>
		public Ciudad(string Nom, ICivilizacion Dueño, Terreno T, float iniPop = 1)
		{
			_PoblacionProductiva = iniPop;
			Nombre = Nom;
			Almacen = new AlmacenCiudad(this);
			T.CiudadConstruida = this;
			Terr = T;

			// Inicializar la armada
			Defensa = new Armada(Dueño, this.Pos, true);
			Defensa.MaxPeso = float.PositiveInfinity;
			Defensa.Posicion.FromGrafica(T);

			// Importar desde T.

			foreach (var x in T.Innatos)
			{
				// Si r.next < (algo):
				AgregaPropiedad(x);
			}

			CivDueno = Dueño;	//Éste debe ser último.

			IntentaConstruirAutoconstruibles(); // Construir autoconstruibles
		}
		// Partial no asinado.
		/// <summary>
		/// Terreno donde se contruye la ciudad.
		/// </summary>
		public Terreno Terr;

		/// <summary>
		/// Devuelve un nuevo diccionario cuyas entradas son el número de unidades que puede construir la ciudad, por cada unidad.
		/// </summary>
		/// <returns>The construibles.</returns>
		public Dictionary<UnidadRAW, ulong> UnidadesConstruibles()
		{
			Dictionary <UnidadRAW, ulong> ret = new Dictionary<UnidadRAW, ulong>();

			foreach (var x in Global.g_.Data.Unidades)
			{
				if (x.ReqCiencia == null || CivDueno.Avances.Contains(x.ReqCiencia))
				{
					ret.Add(x, UnidadesConstruibles(x));
				}
			}
			return ret;
		}

		/// <summary>
		/// Devuelve la cantidad de unidades que puede construir esta ciudad de una unidadRAW específica.
		/// Tiene en cuenta sólo los recursos y la población desocupada.
		/// </summary>
		/// <returns>The construibles.</returns>
		/// <param name="unid">Unid.</param>
		public ulong UnidadesConstruibles(UnidadRAW unid)
		{
			ulong max = (ulong)(getTrabajadoresDesocupados / unid.CostePoblacion);
			if (unid.Reqs != null)
				foreach (var y in unid.Reqs)
				{
					// ¿Cuántas unidades puede hacer, limitando por recursos?
					max = (ulong)Math.Min(Almacen[y.Key] / y.Value, max);
				}
			return max;
		}

		#endregion

		#region Almacén

		IAlmacén IAlmacenante.Almacen
		{
			get
			{
				return (IAlmacén)Almacen;
			}
		}

		/// <summary>
		/// Almacén de recursos.
		/// </summary>
		public AlmacenCiudad Almacen;

		/// <summary>
		/// Calcula el cambio en el almacén de un recurso.
		/// </summary>
		/// <returns>The delta recurso.</returns>
		/// <param name="Recurso">Recurso.</param>
		public float CalculaDeltaRecurso(Recurso Recurso)
		{
			float ret = 0;

			// Trabajos
			foreach (var x in ObtenerListaTrabajos())
			{
				if (x.RAW.EntradaBase.ContainsKey(Recurso))
					ret -= x.Trabajadores * x.RAW.EntradaBase[Recurso];
				if (x.RAW.SalidaBase.ContainsKey(Recurso))
					ret -= x.Trabajadores * x.RAW.SalidaBase[Recurso];

			}

			// Propiedades
			foreach (var x in _Prop)
			{
				foreach (var y in x.Salida)
				{
					if (y.recurso == Recurso)
					{
						ret += y.DeltaEsperado(this);
					}
				}
			}
			return ret;
		}


		#endregion

		#region Armada

		public readonly Armada Defensa;

		public List<Armada> ArmadasEnCiudad()
		{
			//List<Armada> ret = CivDueno.Armadas.FindAll(x => x.Posicion.Equals(posCiudad));
			List<Armada> rat = new List<Armada>();
			foreach (var x in CivDueno.Armadas)
			{
				if (!x.esDefensa && x.Posicion.Equals(Pos))
					rat.Add(x);
			}
			return rat;
		}

		// Todo para crear unidades
		/// <summary>
		/// Entrena una cantidad de unidades de una clase fija.
		/// Incluye la unidad en la armada de la ciudad.
		/// </summary>
		/// <param name="uRAW">Clase de unidades a entrenar</param>
		/// <param name="Cantidad">Cantidad</param>
		/// <returns>Devuelve un arreglo con las unidades que se pudieron entrenar.</returns>
		public Stack Reclutar(UnidadRAW uRAW, ulong cantidad = 1)
		{
			//Stack ret = null;
			if (uRAW.CostePoblacion * cantidad <= getTrabajadoresDesocupados &&
			    Almacen.PoseeRecursos((ListasExtra.ListaPeso<Recurso>)uRAW.Reqs, cantidad))	//Si puede pagar
			{
				_PoblacionProductiva -= uRAW.CostePoblacion * cantidad;	// Recluta desde la poblaci�n productiva.
				foreach (var x in uRAW.Reqs.Keys)				// Quita los recursos que requiere.
				{
					Almacen[x] -= uRAW.Reqs[x] * cantidad;
				}

				Defensa.AgregaUnidad(uRAW, cantidad);
				//ret = new Stack(uRAW, cantidad, this);
				//Defensa.AgregaUnidad(ret);						// Agregar la unidad a la defensa de la ciudad.
			}
			return Defensa.UnidadesAgrupadas(uRAW);											// Devuelve la unidad creada.
		}

		#endregion

		#region Construcción

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
				if (value == null || PuedeConstruir(value))
					EdifConstruyendo = new EdificioConstruyendo(value, this);
				else
					throw new Exception(string.Format("No se puede construir {0} en {1}.", value, this));
			}
		}

		#endregion

		#region Edificios

		//Edificios
		System.Collections.Generic.List<Edificio> _Edif = new System.Collections.Generic.List<Edificio>();

		/// <summary>
		/// Devuelve la lista de instancias de edicio de la ciudad.
		/// </summary>
		/// <value></value>
		public List<Edificio> Edificios
		{
			get
			{
				return _Edif;
			}
		}

		/// <summary>
		/// Revisa si existe una clase de edificio en esta ciudad.
		/// </summary>
		/// <param name="Edif">La clase de edificio buscada</param>
		/// <returns><c>true</c> si existe el edificio, <c>false</c> si no.</returns>
		public bool ExisteEdificio(EdificioRAW Edif)
		{
			foreach (var x in Edificios)
			{
				if (x.RAW == Edif)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Devuelve el edificio en la ciudad con un nombre específico.
		/// </summary>
		/// <param name="Ed">RAW del edificio.</param>
		/// <returns>La instancia de edificio en la ciudad; si no existe devuelve <c>null</c>.</returns>
		public Edificio EncuentraInstanciaEdificio(EdificioRAW Ed)
		{
			foreach (Edificio x in Edificios)
			{
				if (x.RAW == Ed)
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
		/// <param name="Edif">RAW dek edificio a agregar.</param>
		public Edificio AgregaEdificio(EdificioRAW Edif)
		{
			Edificio ret = new Edificio(Edif, this);

			return ret;
		}

		/// <summary>
		/// Devuelve la lista de edificios contruibles por esta ciudad; los que se pueden hacer y no están hechos.
		/// </summary>
		/// <returns></returns>
		public List<EdificioRAW> Construibles()
		{
			List<EdificioRAW> ret = new List<EdificioRAW>();
			foreach (EdificioRAW x in Global.g_.Data.Edificios)
			{
				if (PuedeConstruir(x))
					ret.Add(x);
			}
			return ret;
		}

		/// <summary>
		/// Devuelve <c>true</c> si un edificio se puede contruir en esta ciudad.
		/// <c>false</c> en caso contrario.
		/// </summary>
		/// <param name="Edif">Clase de edificio</param>
		/// <returns></returns>
		public bool PuedeConstruir(EdificioRAW Edif)
		{
			if (!SatisfaceReq(Edif.Reqs()))
				return false;
			if (ExisteEdificio(Edif))
				return false;	// Por ahora no se permite múltiples instancias del mismo edificio en una ciudad.
			if (Edif.MaxPorCivilizacion > 0 && Edif.MaxPorCivilizacion <= CivDueno.CuentaEdificios(Edif))
				return false;
			if (Edif.MaxPorMundo > 0 && Edif.MaxPorCivilizacion <= Global.g_.State.CuentaEdificios(Edif))
				return false;
			return true;
		}

		/// <summary>
		/// Devuelve el número de edificios de una clase que se encutrnan en la ciudad.
		/// </summary>
		/// <param name="Edif">Clase de edificio.</param>
		/// <returns></returns>
		public int NumEdificios(EdificioRAW Edif)
		{
			int ret = 0;
			foreach (var x in Edificios)
			{
				if (x.RAW == Edif)
					ret++;
			}
			return ret;
		}
		// Propiedades	//TODO: Ponerlos en otro archivo.
		System.Collections.Generic.List<Propiedad> _Prop = new System.Collections.Generic.List<Propiedad>();

		/// <summary>
		/// Devuelve la lista de Propiedades de la ciudad.
		/// </summary>
		/// <value>The propiedades.</value>
		public List<Propiedad> Propiedades
		{
			get
			{
				return _Prop;
			}
		}

		/// <summary>
		/// Revisa si existe una propiedad P en la ciudad.
		/// </summary>
		/// <returns><c>true</c>, si la propiedad existe, <c>false</c> en caso contrario.</returns>
		/// <param name="P">La propiedad.</param>
		public bool ExistePropiedad(Propiedad P)
		{
			foreach (var x in Propiedades)
			{
				if (x == P)
					return true;
			}
			return false;
		}

		#region Propiedades

		/// <summary>
		/// Agrega una instancia de <c>Propiedad</c> a la ciudad.
		/// </summary>
		/// <param name="Prop">Propiedad a agregar.</param>
		public void AgregaPropiedad(Propiedad Prop)
		{
			if (!Propiedades.Contains(Prop))
			{
				Propiedades.Add(Prop);
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
				return Global.g_.Data.RecursoAlimento;
			}
		}

		/// <summary>
		/// Número de infantes que nacen por (PoblaciónProductiva*Tick) Base.
		/// </summary>
		public static float TasaNatalidadBase = 0.12f;
		/// <summary>
		/// Probabilidad base de un infante arbitrario de morir en cada tick.
		/// </summary>
		public static float TasaMortalidadInfantilBase = 0.01f;
		/// <summary>
		/// Probabilidad base de un habitante productivo arbitrario de morir en cada tick.
		/// </summary>
		public static float TasaMortalidadProductivaBase = 0.02f;
		/// <summary>
		/// Probabilidad base de un adulto de la tercera edad arbitrario de morir en cada tick.
		/// </summary>
		public static float TasaMortalidadVejezBase = 0.1f;
		/// <summary>
		/// Probabilidad de que un infante se convierta en productivo
		/// </summary>
		public static float TasaDesarrolloBase = 0.2f;
		/// <summary>
		/// Probabilidad de que un Productivo envejezca
		/// </summary>
		public static float TasaVejezBase = 0.05f;
		/// <summary>
		/// Consumo base de alimento por ciudadano.
		/// </summary>
		public static float ConsumoAlimentoPorCiudadanoBase = 1f;
		//Población
		float _PoblacionProductiva = 10f;
		float _PoblacionPreProductiva = 0;
		float _PoblacionPostProductiva = 0;
		float _TasaMortalidadHambruna = 0.5f;

		/// <summary>
		/// Devuelve la población real y total de la ciudad.
		/// </summary>
		/// <value>The get real población.</value>
		public float getRealPoblacion
		{
			get
			{
				return _PoblacionProductiva + _PoblacionPreProductiva + _PoblacionPostProductiva;
			}
		}

		/// <summary>
		/// Devuelve la población de la ciudad.
		/// </summary>
		/// <value>The get poplación.</value>
		public ulong getPoblacion
		{
			get
			{
				return getPoblacionProductiva + getPoblacionPreProductiva + getPoblacionPostProductiva;
			}
		}

		/// <summary>
		/// Devuelve la población productiva.
		/// </summary>
		/// <value></value>
		public ulong getPoblacionProductiva
		{
			get
			{
				return (ulong)Math.Floor(_PoblacionProductiva);
			}
			set
			{
				_PoblacionProductiva = value;
			}
		}

		/// <summary>
		/// Devuelve la población pre productiva.
		/// </summary>
		/// <value></value>
		public ulong getPoblacionPreProductiva
		{
			get
			{
				return (ulong)Math.Floor(_PoblacionPreProductiva);
			}
		}

		/// <summary>
		/// Devuelve la población post productiva.
		/// </summary>
		/// <value></value>
		public ulong getPoblacionPostProductiva
		{
			get
			{
				return (ulong)Math.Floor(_PoblacionPostProductiva);
			}
		}

		#endregion

		#region Trabajadores

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
					List<IRequerimiento<ICiudad>> Req = new List<IRequerimiento<ICiudad>>();
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
		public List<Trabajo> ObtenerListaTrabajos()
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

		/// <summary>
		/// Devuelve la instancia de trabajo en esta ciudad, si existe. Si no, la crea y la devuelve cuando <c>CrearInstancia</c>.
		/// </summary>
		/// <param name="TRAW"></param>
		/// TrabajoRAW que se busca
		/// <param name="CrearInstancia">Si no existe tal instancia y <c>CrearInstancia</c>, la crea; si no, tira excepción.</param>
		/// <returns>Devuelve el trabajo en la ciudad correspondiente a este TrabajoRAW.</returns>
		public Trabajo EncuentraInstanciaTrabajo(TrabajoRAW TRAW)
		{
			System.Diagnostics.Debug.Assert(TRAW != null);
			EdificioRAW Ed = TRAW.Edificio;   // La clase de edificio que puede contener este trabajo.
			Edificio Edif = EncuentraInstanciaEdificio(Ed); // La instancia del edificio en esta ciudad.

			System.Diagnostics.Debug.Assert(Edif != null);
			if (Edif == null)
				return null;    // Devuelve nulo si no existe el edificio donde se trabaja.
			foreach (Trabajo x in ObtenerListaTrabajos())
			{
				if (x.RAW == TRAW)
					return x;
			}
			Trabajo ret = new Trabajo(TRAW, this);
			// Agregar este trabajo al edificio. Sin trabajadores.
			//Edif.Trabajos.Add(ret);
			//throw new Exception("No existe el trabajo buscado.");
			return ret;
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
			List<Trabajo> L = ObtenerListaTrabajos();
			L.Sort((x, y) => x.Prioridad < y.Prioridad ? -1 : 1); // Ordenar por prioridad.
			while (L.Count > 0 && getTrabajadoresDesocupados < n && getTrabajadoresDesocupados != getPoblacion)
			{
				L[0].Trabajadores = 0;
				L.RemoveAt(0);
			}
		}

		/// <summary>
		/// Crea una nueva lista de todos los trabajos abiertos para una ciudad, tomando en cuenta sus edificios y ciencias.
		/// threadsafe.
		/// </summary>
		/// <returns>Devuelve una nueva lista.</returns>
		public List<TrabajoRAW> obtenerTrabajosAbiertos()
		{
			List<TrabajoRAW> ret = new List<TrabajoRAW>();
			foreach (var x in Global.g_.Data.Trabajos)
			{
				if (SatisfaceReq(x.Reqs()) && this.ExisteEdificio(x.Edificio))
				{
					ret.Add(x);
				}
			}
			return ret;
		}

		#endregion

		#region Requerimientos

		/// <summary>
		/// Revisa si esta ciudad satisface un Irequerimiento.
		/// </summary>
		/// <param name="Req">Un requerimiento</param>
		/// <returns>Devuelve <c>true</c> si esta ciudad satisface un Irequerimiento. <c>false</c> en caso contrario.</returns>
		public bool SatisfaceReq(IRequerimiento<ICiudad> Req)
		{
			return  Req.LoSatisface(this);
		}

		/// <summary>
		/// Revisa si esta ciudad satisface una lista de requerimientos.
		/// </summary>
		/// <param name="Req"></param>
		/// <returns>Devuelve <c>true</c> si esta ciudad satisface todos los IRequerimiento. <c>false</c> en caso contrario.</returns>
		public bool SatisfaceReq(List<IRequerimiento<ICiudad>> Req)
		{
			return Req.TrueForAll(x => x.LoSatisface(this));
		}

		#endregion

		#region Puntuación

		float IPuntuado.Puntuacion
		{
			get
			{
				float ret = 0;
				// Recursos
				foreach (var x in Almacen)
				{
					ret += x.Value;
				}

				// Población
				ret += getPoblacionPreProductiva * 2 + getPoblacionProductiva * 3 + getPoblacionPostProductiva;

				return ret;
			}
		}

		#endregion

		public float AlimentoAlmacen
		{ 
			get
			{
				return Almacen[RecursoAlimento];
			}
			set
			{
				Almacen[RecursoAlimento] = value;
			}
		}

		#region ITickable

		// Tick
		/// <summary>
		/// Da un tick poblacional
		/// </summary>
		/// <param name="t">Diración del tick</param>
		public void PopTick(float t = 1)
		{
			// Se está suponiendo crecimiento constante entre ticks!!!

			//Crecimiento prometido por sector de edad.
			float[] Crecimiento = new float[3];
			float Consumo = getPoblacion * ConsumoAlimentoPorCiudadanoBase * t;
			if (float.IsInfinity(AlimentoAlmacen) || float.IsNaN(AlimentoAlmacen))
			{
				System.Diagnostics.Debugger.Break();
			}
			System.Diagnostics.Debug.Assert(!float.IsInfinity(AlimentoAlmacen) && !float.IsNaN(AlimentoAlmacen), "Se acaba de obtener alimento infinito.");
			//Que coman
			//Si tienen qué comer
			if (Consumo <= AlimentoAlmacen)
			{
				Almacen.changeRecurso(RecursoAlimento, -Consumo);

			}
			else
			{
				//El porcentage de muertes
				float pctMuerte = (1 - (AlimentoAlmacen / (getPoblacion * ConsumoAlimentoPorCiudadanoBase))) * _TasaMortalidadHambruna;
				if (!(0 <= pctMuerte && pctMuerte <= 1))
					System.Diagnostics.Debugger.Break();
				System.Diagnostics.Debug.Assert(0 <= pctMuerte && pctMuerte <= 1, "wat?");
				AlimentoAlmacen = 0;
				//Promesas de muerte por sector.
				Crecimiento[0] -= getPoblacionPreProductiva * pctMuerte;
				Crecimiento[1] -= getPoblacionProductiva * pctMuerte;
				Crecimiento[2] -= getPoblacionPostProductiva * pctMuerte;
			}

			//Crecimiento poblacional
			//Infantil a productivo.
			float Desarrollo = TasaDesarrolloBase * getPoblacionPreProductiva * t;
			Crecimiento[0] -= Desarrollo;
			Crecimiento[1] += Desarrollo;
			//Productivo a viejo
			float Envejecer = TasaVejezBase * getPoblacionProductiva * t;
			Crecimiento[1] -= Envejecer;
			Crecimiento[2] += Envejecer;
			//Nuevos infantes
			float Natalidad = TasaNatalidadBase * getPoblacionProductiva * t;
			Crecimiento[0] += Natalidad;
			//Mortalidad
			Crecimiento[0] -= getPoblacionPreProductiva * TasaMortalidadInfantilBase * t;
			Crecimiento[1] -= getPoblacionProductiva * TasaMortalidadProductivaBase * t;
			Crecimiento[2] -= getPoblacionPostProductiva * TasaMortalidadVejezBase * t;

			// Aplicar cambios.

			if (Crecimiento[1] < -(long)getTrabajadoresDesocupados)
			{
				CivDueno.AgregaMensaje(new IU.Mensaje("La ciudad {0} ha perdido trabajadores productivos ocupados.", this));
				LiberarTrabajadores(getPoblacionProductiva - (ulong)Crecimiento[1]);

			}

			_PoblacionPreProductiva = Math.Max(_PoblacionPreProductiva + Crecimiento[0], 0);
			_PoblacionProductiva = Math.Max(_PoblacionProductiva + Crecimiento[1], 0);
			_PoblacionPostProductiva = Math.Max(_PoblacionPostProductiva + Crecimiento[2], 0);

			if (AutoReclutar)
			{
				// Autoacomodar trabajadores desocupados
				List<Trabajo> Lst = ObtenerListaTrabajos();

				Lst.Sort(((x, y) => x.Prioridad < y.Prioridad ? -1 : 1));

				for (int i = 0; i < Lst.Count && getTrabajadoresDesocupados > 0; i++)
				{
					Lst[i].Trabajadores = Lst[i].MaxTrabajadores;
				}
			}
		}

		/// <summary>
		/// Da un tick hereditario.
		/// </summary>
		public void ResourceTick(float t = 1)
		{
			foreach (ITickable x in Edificios)
			{
				x.Tick(t);
			}

			foreach (var x in Propiedades)
			{
				x.Tick(this, t);
			}
			// Construir edificio.
			if (EdifConstruyendo != null)
			{
				EdifConstruyendo.AbsorbeRecursos();
				if (EdifConstruyendo.EstaCompletado())
				{
					EdifConstruyendo.Completar();
					EdifConstruyendo = null;    //  Ya no se contruye edificio. Para evitar error de duplicidad.
				}
			}
		}

		/// <summary>
		/// Intenta construir edificios autoconstruibles
		/// </summary>
		public void IntentaConstruirAutoconstruibles()
		{
			// Autocontruible
			List<EdificioRAW> PosiblesEdif = Global.g_.Data.EdificiosAutoconstruibles().FindAll(x => !ExisteEdificio(x)); 	// Obtener lista de edificios autocontruibles no construidos.
			foreach (var x in PosiblesEdif)
			{
				if (PuedeConstruir(x))
				{	// Si satisface requerimientos de construcción:
					AgregaEdificio(x);
				}
			}
		}

		/// <summary>
		/// Destruye los recursos con el flag <c>.desaparecen</c>.
		/// </summary>
		[Obsolete]
		public void DestruirRecursosTemporales()
		{
			// Desaparecen algunos recursos
			List<Recurso> Alm = new List<Recurso>(Almacen.Keys);
			foreach (Recurso x in Alm)
			{
				if (x.Desaparece)
				{
					Almacen[x] = 0;
				}
			}

		}

		/// <summary>
		/// Ejecuta ambos: Tick () y PopTick ().
		/// En ese orden.
		/// </summary>
		public void Tick(float t = 1)
		{
			PopTick(t);
			ResourceTick(t);
			if (CivDueno != null && getPoblacion == 0)
			{		// Si la población de una ciudad llega a cero, se hacen ruinas (ciudad sin civilización)
				CivDueno.removeCiudad(this);
			}
		}

		#endregion
	}
}