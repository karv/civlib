using ListasExtra;
using System;
using System.Collections.Generic;
using System.Linq;
using Civ.Combate;
using Civ.Orden;
using Civ.Global;
using Civ;
using Civ.RAW;
using Civ.Topología;
using Civ.IU;
using Civ.Almacén;
using System.Diagnostics;

namespace Civ.ObjetosEstado
{
	/// <summary>
	/// Representa un conjunto de unidades.
	/// </summary>
	[Serializable]
	public class Armada : IPosicionable, IPuntuado, IDefensor, IDisposable
	{
		#region General

		/// <summary>
		/// Elimina esta armada del universo.
		/// </summary>
		public void Eliminar ()
		{
			if (Unidades.Count != 0)
				throw new Exception ("No se puede desechar una armada con unidades en ella.");
			((IDisposable)this).Dispose ();
			Debug.WriteLine ("Eliminando armada " + ToString (), "Dispose");

			CivDueño?.Armadas.Remove (this);
		}

		/// <summary>
		/// Diccionario privado UnidadRAW-Stack
		/// </summary>
		//readonly ListaArmada _unidades = new ListaArmada ();
		readonly ListaPeso<IUnidadRAW, Stack> _unidades = new ListaPeso<IUnidadRAW, Stack> (
			                                                  Stack.Merge,
			                                                  null);
		/*new ListaPeso<IUnidadRAW, Stack> (;
			                                                  (x, y) => Stack.Merge (x, y),
			                                                  null);*/
		/// <summary>
		/// Devuelve true si esta armada es una armada intrínseca de una ciudad.
		/// </summary>
		public bool EsDefensa;

		/// <summary>
		/// Devuelve la lista de unidades en la armada.
		/// </summary>
		/// <value>The lista unidades.</value>
		public ICollection<Stack> Unidades
		{
			get
			{
				return _unidades.Values;
			}
		}

		/// <summary>
		/// Devuelve una colección con los tipos de unidades que hay en esta armada.
		/// </summary>
		/// <returns>The unidades.</returns>
		public ICollection<IUnidadRAW> TiposUnidades ()
		{
			return _unidades.Keys;
		}

		/// <summary>
		/// Devuelve las unidades que son de una clase específica.
		/// </summary>
		/// <param name="raw">Tipo de unidades.</param>
		/// <returns></returns>
		[Obsolete ("Usar this[]")]
		public Stack UnidadesAgrupadas (IUnidadRAW raw)
		{
			return _unidades [raw];
		}

		/// <summary>
		/// Crea una nueva armada
		/// </summary>
		/// <param name="civilizacion">Civilización</param>
		/// <param name="posición">Posición de la armada (se clona) </param>
		public Armada (ICivilización civilizacion, Pseudoposición posición)
		{
			CivDueño = civilizacion;
			EsDefensa = false;
			Posición = new Pseudoposición (posición);
			civilizacion.Armadas.Add (this);

			posición.AlColisionar += Posición_AlColisionar;
			posición.AlDesplazarse += Posición_AlDesplazarse;
		}

		void Posición_AlDesplazarse ()
		{
			// Para saber si dropea cosas
			var drops = new DropStack (Posición);
			foreach (var u in _unidades)
			{
				while (u.Value.Carga.CargaRestante < 0)
				{
					var t = u.Value.Carga.Elegir ();
					drops.Almacén [t.Key] += t.Value;
					u.Value.Carga.Remove (t.Key);
				}
			}

			// Si dropeó algo, se agrega.
			if (drops.Almacén.Count > 0)
				Juego.State.Drops.Add (drops);
		}

		void Posición_AlColisionar (Graficas.Continuo.Punto<Terreno> obj)
		{
			var arm = (obj as Pseudoposición)?.Objeto as Armada;

			if (arm == null) // No hacer nada si colisionó con algo que no es Armada.
				return;
			
			if (!CivDueño.Diplomacia.PermitePaso (arm))
			{
				arm.Orden = new OrdenEstacionado ();
				arm.CivDueño.AgregaMensaje (new Mensaje (
					"Bloqueo de armada",
					"Nuestra Armada {0} detenida por armada {1} de {2} en {3}",
					TipoRepetición.ArmadaTerminaOrden,
					this,
					this,
					CivDueño,
					Posición));
			}
		}

		/// <summary>
		/// Crea una nueva armada
		/// </summary>
		/// <param name="ciudad">Ciudad donde estará</param>
		/// <param name="esDefensa">If set to <c>true</c> es defensa.</param>
		public Armada (ICiudad ciudad, bool esDefensa = false)
			: this (ciudad.CivDueño,
			        ciudad.Posición ())
		{
			EsDefensa = esDefensa;
		}

		/// <summary>
		/// Coeficiente de MaxPeso para que tal vez, con exp, aumente la máxima cantidad de peso que
		/// esta armada puede cargar.
		/// </summary>
		float _personalPesoMejoraCoef = 1f;

		//  Probablemente, _MaxPeso sea una función que dependa de CivDueño.
		/// <summary>
		/// Devuelve o establece el máximo peso que puede cargar esta armada.
		/// </summary>
		/// <value>The max peso.</value>
		public float MaxPeso
		{
			get
			{
				return EsDefensa ? float.PositiveInfinity : _personalPesoMejoraCoef * CivDueño.MaxPeso;
			}
		}

		/// <summary>
		/// Devuelve el peso actual de la armada. (A lo reduccionista)
		/// </summary>
		public float Peso
		{
			get
			{
				float ret = 0;
				foreach (var x in Unidades)
				{
					ret += x.Peso;
				}
				return ret;
			}
		}

		/// <summary>
		/// Devuelve el peso de la armada que le resta.
		/// </summary>
		/// <value>The peso libre.</value>
		public float PesoLibre
		{
			get
			{
				return MaxPeso - Peso;
			}
		}

		/// <summary>
		/// Devuelve o establece el lugar donde está la armada.
		/// </summary>
		/// <value></value>
		public Pseudoposición Posición { get; }

		/// <summary>
		/// Agrega, mueve o junta unidad(es) a esta armada.
		/// </summary>
		/// <param name="stack">El stack que se agregará o moverá.</param>
		public void AgregaUnidad (Stack stack)
		{
			if (stack.Peso > PesoLibre)
				throw new Exception (string.Format (
					"No hay peso libre en la armada {0} para el stack {1}.",
					this,
					stack));
			
			if (PosicionConsistente (stack))
			{
				if (_unidades.ContainsKey (stack.RAW))
				{
					var stackArmada = _unidades [stack.RAW];

					stackArmada.HP = (stackArmada.Cantidad * stackArmada.HP + stack.Cantidad * stack.HP) / (stack.Cantidad + stackArmada.Cantidad);
					stackArmada.Entrenamiento = (stackArmada.Cantidad * stack.Entrenamiento + stack.Cantidad * stack.Entrenamiento) / (stack.Cantidad + stackArmada.Cantidad);

					stackArmada.Cantidad += stack.Cantidad;
				}
				else
					_unidades.Add (stack.RAW, stack);
				
				stack.Destruir ();
			}
			else
			{
				#if DEBUG
				Console.WriteLine ("No se puede agregar unidad a armada si éstas no están en el mismo lugar.");
				#endif
			}
		}

		/// <summary>
		/// Agrega algunas unidades a la armada
		/// </summary>
		/// <param name="raw">Tipo de unidad</param>
		/// <param name="cantidad">Cantidad</param>
		public void AgregaUnidad (IUnidadRAW raw, long cantidad)
		{
			if (cantidad <= 0)
			{
				Debug.WriteLine ("Imposible agregar <=0 unidades."); // TODO: Exception
				throw new ArgumentException ("Imposible agregar <=0 unidades.", "cantidad");
			}

			var realCantidad = 
				float.IsPositiveInfinity (PesoLibre) ?
				cantidad :
				Math.Min (cantidad, (long)(PesoLibre / raw.Peso));
			if (realCantidad == 0)
				return;
			if (_unidades.ContainsKey (raw))
			{
				var stack = _unidades [raw];
				var antes = stack.Cantidad;
				stack.Cantidad += realCantidad;

				stack.HP = (antes * stack.HP + realCantidad) / stack.Cantidad;
				stack.Entrenamiento = (antes * stack.Entrenamiento) / stack.Cantidad;
			}
			else
			{
				var stack = new Stack (raw, realCantidad, this);
				_unidades.Add (raw, stack);
			}
		}

		/// <summary>
		/// Revisa si una armada y una unidad tienen la misma posición.
		/// </summary>
		/// <returns><c>true</c> si comparten el mismo lugar; <c>false</c> otherwise.</returns>
		/// <param name="stack">El stack con la que se comparará posición.</param>
		public bool PosicionConsistente (Stack stack)
		{
			System.Diagnostics.Debug.Assert (Posición != null);
			return Posición.Equals (stack.Posición);
		}

		/// <summary>
		/// Quita un stack de la Armada.
		/// </summary>
		/// <param name="stack">Unidad a quitar</param>
		public void QuitarUnidad (Stack stack)
		{
			_unidades.Remove (stack.RAW);
			if (!_unidades.Any ())
				invocaAlVaciarse ();
		}

		void invocaAlVaciarse ()
		{
			AlVaciarse?.Invoke (this, EventArgs.Empty);

			Eliminar ();
		}

		/// <summary>
		/// Releases all resource used by the <see cref="Civ.ObjetosEstado.Armada"/> object.
		/// </summary>
		void IDisposable.Dispose ()
		{
			if (!EsDefensa)
				((IDisposable)Posición).Dispose ();
			// THINK: ¿Hacer que tire exception si EsDefensa?
		}

		/// <summary>
		/// Pelea durante t horas
		/// </summary>
		/// <param name="armada">Armada</param>
		/// <param name="t">tiempo de pelea</param>
		public void Pelea (Armada armada, TimeSpan t)
		{
			var cbt = new AnálisisCombate (this, armada, t);
			//Debug.WriteLine (
			//	string.Format ("{0} peleando contra {1}", CivDueño, armada.CivDueño),
			//	"Pelea");
			foreach (Stack x in Unidades)
			{
				var btl = new AnálisisBatalla (x, armada, t);
				if (btl.Defensor != null)
					cbt.Batallas.Add (btl);
/*					Debug.WriteLine (
						string.Format (
							"{0} dañó a {1} {2:P}",
							x,
							btl.Defensor,
							btl.Defensor.HP),
						"Pelea");
					btl.Ejecutar ();
*/
			}

			cbt.Ejecutar ();

			// Informe
			var civ = CivDueño as Civilización;
			if (civ != null)
			{
				civ.Combates.AddOrMerge (cbt);
			}
		}

		/// <summary>
		/// Devuelve la suma de las vitalidades de sus <see cref="Civ.ObjetosEstado.Stack"/>
		/// </summary>
		public float Vitalidad
		{
			get
			{
				float ret = 0;
				foreach (var s in Unidades)
				{
					ret += s.Vitalidad;
				}
				return ret;
			}
		}

		/// <summary>
		/// Debe ejecutarse cuando esta armada es objetivo de algún ataque.
		/// </summary>
		/// <param name="anal">El análisis del ataque.</param>
		public void FueAtacado (IAnálisisBatalla anal)
		{
			if (!Unidades.Any ())
			{
				// invocaAlVaciarse ();
				AlSerDestruido?.Invoke (this, new CombateEventArgs (anal));
			}
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Civ.ObjetosEstado.Armada"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Civ.ObjetosEstado.Armada"/>.</returns>
		public override string ToString ()
		{
			string ret = string.Format (
				             "Pos: {0}\tFuerza: {1}",
				             Posición,
				             ((IPuntuado)this).Puntuación);
			foreach (var u in Unidades)
			{
				ret += string.Format (
					"\n\tClase:{0}\tCantidad:{1}\tVitalidad:{2}",
					u.RAW,
					u.Cantidad,
					u.Vitalidad);
			}
			return ret;
		}

		/// <summary>
		/// Velocidad de desplazamiento.
		/// Es la velocidad mínima de sus Stacks.
		/// </summary>
		public float Velocidad
		{
			get
			{
				return Unidades.Count == 0 ? 0 : Unidades.Min (x => x.RAW.Velocidad);
			}
		}

		/// <summary>
		/// Devuelve <c>true</c> sólo si esta armada se encuentra en terreno
		/// </summary>
		/// <value><c>true</c> if en terreno; otherwise, <c>false</c>.</value>
		public bool EnTerreno{ get { return Posición.EnOrigen; } }

		/// <summary>
		/// Un Tick de la armada
		/// </summary>
		public void Tick (TiempoEventArgs t)
		{
			if (Orden.Ejecutar (t.GameTime))
			{
				Orden = new OrdenEstacionado ();
				CivDueño.AgregaMensaje (new Mensaje (
					"Llegada alcanzó destino.",
					"{0} llegó a {1}",
					TipoRepetición.ArmadaTerminaOrden,
					this,
					this,
					Posición));
			}
		}

		/// <summary>
		/// Devuelve la civilización que posee esta armada
		/// </summary>
		/// <value>The civ dueño.</value>
		public ICivilización CivDueño { get; }

		/// <summary>
		/// Devuelve un nuevo diccionario que asocia a cada UnidadRAW la lista de Unidades que tiene.
		/// </summary>
		/// <returns>The dictionary.</returns>
		public Dictionary <IUnidadRAW, List<Stack>> ToDictionary ()
		{
			var ret = new Dictionary<IUnidadRAW, List<Stack>> ();
			foreach (var x in Unidades)
			{
				if (!ret.ContainsKey (x.RAW))
					ret.Add (x.RAW, new List<Stack> ());
				ret [x.RAW].Add (x);
			}
			return ret;
		}

		/// <summary>
		/// Orden actual de la armada
		/// </summary>
		/// <remarks>Nunca debe ser <c>null</c></remarks>
		public IOrden Orden = new OrdenEstacionado ();

		/// <summary>
		/// Devuelve el stack que le corresponde a una clase de unidad
		/// </summary>
		public Stack this [IUnidadRAW uRAW]
		{
			get
			{
				return _unidades [uRAW];
			}
		}

		#endregion

		#region IPuntuado

		float IPuntuado.Puntuación
		{
			get
			{
				float ret = 0;
				foreach (IPuntuado x in _unidades.Values)
				{
					ret += x.Puntuación;
				}
				return ret;
			}
		}

		#endregion

		#region IPosicionable implementation

		Pseudoposición IPosicionable.Posición ()
		{
			return Posición;
		}

		#endregion

		#region Comandos especiales

		/// <summary>
		/// Revisa si existe un IUnidadRAWColoniza que pueda colonizar y lo devuelve, 
		/// null si no existe
		/// </summary>
		public bool PuedeColonizar (out Stack unidad)
		{
			unidad = Unidades.FirstOrDefault (x => x.PuedeColonizarAqui);
			return  unidad != null;
		}

		#endregion

		#region IDefensor

		Stack IDefensor.Defensa (IAtacante atacante)
		{
			// Elegir el defensa que le cause menor daño
			Stack DefÓptimo = null;
			float MinActual = 0;
			foreach (var x in Unidades)
			{
				float daño = atacante.ProponerDaño (x.RAW);
				if (DefÓptimo == null || MinActual > daño)
				{
					DefÓptimo = x;
					MinActual = daño;
				}
			}
			if (DefÓptimo == null)
			{
				// No hay defensor
				// ¿Puede que sea un defensa de ciudad sin defensores?
				Debug.WriteLine (string.Format (
					"{0} saqueando alguna ciudad de {1}",
					atacante,
					CivDueño));
				/*
				throw new Exception (string.Format (
					"Por alguna razón, el defensor {0} no eligió un stack para defenderse contra {2}\n" +
					"¿No tiene unidades? #Unidades: {1}",
					this,
					Unidades,
					atacante));
					*/
			}
			return DefÓptimo;
		}

		#endregion

		#region Eventos

		/// <summary>
		/// Ocurre cuando esta armada se vacía.
		/// </summary>
		public event EventHandler AlVaciarse;

		/// <summary>
		/// Ocurre cuando la armada queda vacía por un combate.
		/// </summary>
		/// <remarks>
		/// Su argumento es la última iteración de su combate.
		/// </remarks>
		public event EventHandler<CombateEventArgs> AlSerDestruido;

		#endregion
	}
}