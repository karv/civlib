using ListasExtra;
using System;
using System.Collections.Generic;
using System.Linq;
using Civ.Data;

namespace Civ
{
	/// <summary>
	/// Representa un conjunto de unidades.
	/// </summary>
	public class Armada: IPosicionable, IPuntuado
	{
		#region General

		/// <summary>
		/// Elimina esta armada del universo.
		/// </summary>
		public void Eliminar ()
		{
			if (Unidades.Count == 0)
				throw new Exception ("No se puede desechar una armada con unidades en ella.");
			((IDisposable)Posición).Dispose ();

			CivDueño?.Armadas.Remove (this);
		}


		/// <summary>
		/// Diccionario privado UnidadRAW-Stack
		/// </summary>
		readonly ListaPeso<UnidadRAW, Stack> _unidades = new ListaPeso<UnidadRAW, Stack> (
			                                                 Stack.Merge,
			                                                 null);

		/// <summary>
		/// Devuelve true si esta armada es una armada intrínseca de una ciudad.
		/// </summary>
		public bool EsDefensa { get; }

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

		public ICollection<UnidadRAW> TiposUnidades ()
		{
			return _unidades.Keys;
		}

		/// <summary>
		/// Devuelve las unidades que son de una clase específica.
		/// </summary>
		/// <param name="raw">Tipo de unidades.</param>
		/// <returns></returns>
		[Obsolete ("Usar this[]")]
		public Stack UnidadesAgrupadas (UnidadRAW raw)
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
			Posición.A = posición.A;
			Posición.B = posición.B;
			Posición.Loc = posición.Loc;
			civilizacion.Armadas.Add (this);
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

		float _maxPeso;
		//  Probablemente, _MaxPeso sea una función que dependa de CivDueño.
		/// <summary>
		/// Devuelve o establece el máximo peso que puede cargar esta armada.
		/// </summary>
		/// <value>The max peso.</value>
		public float MaxPeso
		{
			get
			{
				return _maxPeso;
			}
			set
			{
				_maxPeso = Math.Max (value, Peso);	// No puedo reducir MaxPeso a menor que Peso.
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
			if (PosicionConsistente (stack))
			{
				if (PesoLibre >= stack.Peso)
				{
					stack.ArmadaPerteneciente = this;
					if (_unidades.ContainsKey (stack.RAW))
					{
						_unidades [stack.RAW].Cantidad += stack.Cantidad;
					}
					else
						_unidades.Add (stack.RAW, stack);
				}
			}
			else
			{
				#if DEBUG
				Console.WriteLine ("No se puede agregar unidad a armada si éstas no están en el mismo lugar");
				#endif
			}
		}

		public void AgregaUnidad (UnidadRAW raw, ulong cantidad)
		{
			if (cantidad <= 0)
			{
				#if DEBUG
				Console.WriteLine ("Imposible agregar <=0 unidades.");
				#endif
				return;
			}
			if (_unidades.ContainsKey (raw))
			{
				_unidades [raw].Cantidad += cantidad;
			}
			else
			{
				_unidades.Add (raw, new Stack (raw, cantidad, this));
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
		}

		/// <summary>
		/// Pelea durante t horas
		/// </summary>
		/// <param name="armada">Armada</param>
		/// <param name="t">tiempo de pelea</param>
		public void Pelea (Armada armada, TimeSpan t)
		{
			if (Unidades.Count == 0 || armada.Unidades.Count == 0)
				return;

			int i, j;

			var Arms = new Armada[2];
			Stack Ata;
			Stack Def;
			Arms [0] = this;
			Arms [1] = armada;

			i = Global.Juego.Rnd.Next (2); // Arms[i] Inicia
			j = 1 - i;
			if (Arms [i].Unidades.Count > 0)
			{
				Ata = Arms [i].MayorDaño (Arms [j]);
				Def = Ata.MenorDaño (Arms [j]);
				Ata.CausaDaño (Def.ArmadaPerteneciente, Def.RAW, Ata, (float)t.TotalHours);
				if (Def.Muerto)
					return;
			}
				
			i = j; // Arms[1 - 1] le sigue.
			j = 1 - i;
			if (Arms [i].Unidades.Count > 0)
			{
				Ata = Arms [i].MayorDaño (Arms [j]);
				Def = Ata.MenorDaño (Arms [j]);
				Ata.CausaDaño (Def.ArmadaPerteneciente, Def.RAW, Ata, (float)t.TotalHours);
			}
		}

		/// <summary>
		/// Devuelve la unidad de maximin daño de this.Unidades a A.Unidades
		/// </summary>
		/// <returns>La unidad que hace el mayor daño menor.</returns>
		/// <param name="armada">A.</param>
		Stack MayorDaño (Armada armada)
		{
			float maxDaño = 0;
			float currDaño;
			Stack ret = null;
			foreach (var x in Unidades)
			{
				currDaño = x.DañoPropuesto (x.MenorDaño (armada));
				if (currDaño >= maxDaño)
				{
					ret = x;
					maxDaño = currDaño;
				}
			}
			System.Diagnostics.Debug.Assert (ret != null);
			return ret;
		}

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
					u,
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
		public void Tick (TimeSpan t)
		{
			if (Orden.Ejecutar (t))
			{
				Orden = new Civ.Orden.OrdenEstacionado ();
				CivDueño.AgregaMensaje (new IU.Mensaje ("{0} llegó a {1}", this, Posición));
			}
		}

		public ICivilización CivDueño { get; }

		/// <summary>
		/// Devuelve un nuevo diccionario que asocia a cada UnidadRAW la lista de Unidades que tiene.
		/// </summary>
		/// <returns>The dictionary.</returns>
		public Dictionary <UnidadRAW, List<Stack>> ToDictionary ()
		{
			var ret = new Dictionary<UnidadRAW, List<Stack>> ();
			foreach (var x in Unidades)
			{
				if (!ret.ContainsKey (x.RAW))
					ret.Add (x.RAW, new List<Stack> ());
				ret [x.RAW].Add (x);
			}
			return ret;
		}

		public Civ.Orden.IOrden Orden = new Civ.Orden.OrdenEstacionado ();

		/// <summary>
		/// Devuelve el stack que le corresponde a una clase de unidad
		/// </summary>
		public Stack this [UnidadRAW uRAW]
		{
			get
			{
				return this [uRAW];
			}
		}

		#endregion

		#region Daño

		/// <summary>
		/// Daña un stack
		/// </summary>
		/// <param name="unidad">Unidad.</param>
		/// <param name="deltaHP">Daño o cura (negativo es daño)</param>
		/// <param name="atacante">Stack atacante </param>
		public void DañarStack (UnidadRAW unidad, Stack atacante, float deltaHP)
		{
			Stack currStack = this [unidad];
			currStack.Dañar (-deltaHP, atacante.RAW.Dispersion);
			if (currStack.HP < 0)
			{
				_unidades.Remove (unidad);
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

		#region Colonizar

		/// <summary>
		/// Devuelve true si al menos un stack puede colonizar.
		/// </summary>
		/// <returns><c>true</c>, if colonizar was pueded, <c>false</c> otherwise.</returns>
		public bool PuedeColonizar ()
		{
			foreach (var x in _unidades.Keys)
			{
				if (x.PuedeColonizar)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Coloniza y contruye una ciudad. Usa sólo un stack que pueda colonizar.
		/// Devuelve la ciudad colonizada.
		/// </summary>
		public Ciudad Coloniza ()
		{
			foreach (var x in _unidades)
			{
				return x.Value.Colonizar ();
			}
			return null;
		}

		#endregion

		#endregion
	}
}