using ListasExtra;
using System;
using System.Collections.Generic;
using System.Linq;
using CivLibrary.Debug;

namespace Civ
{
	/// <summary>
	/// Representa un conjunto de unidades.
	/// </summary>
	public class Armada: IDisposable, IPosicionable
	{
		
		#region General

		ListaPeso<UnidadRAW, Stack> _Unidades = new ListaPeso<UnidadRAW, Stack>((x, y) => Stack.Merge(x, y), null);

		/// <summary>
		/// Devuelve true si esta armada es una armada intrínseca de una ciudad.
		/// </summary>
		public readonly bool esDefensa;

		/// <summary>
		/// Devuelve la lista de unidades en la armada.
		/// </summary>
		/// <value>The lista unidades.</value>
		public ICollection<Stack> Unidades
		{
			get
			{
				return _Unidades.Values;
			}
		}

		public ICollection<UnidadRAW> TiposUnidades()
		{
			return _Unidades.Keys;
		}

		/// <summary>
		/// Devuelve las unidades que son de una clase específica.
		/// </summary>
		/// <param name="RAW">Tipo de unidades.</param>
		/// <returns></returns>
		public Stack UnidadesAgrupadas(UnidadRAW RAW)
		{
			return _Unidades[RAW];
		}

		/// <summary>
		/// Crea una nueva armada
		/// </summary>
		/// <param name="C">Civilización</param>
		/// <param name="esDefensa">If set to <c>true</c> es defensa.</param>
		/// <param name="posición">Posición de la armada (se clona) </param>
		public Armada(ICivilizacion C, Pseudoposicion posición, bool esDefensa = false)
		{
			_CivDueño = C;
			this.esDefensa = esDefensa;
			Posicion.A = posición.A;
			Posicion.B = posición.B;
			Posicion.loc = posición.loc;
			C.Armadas.Add(this);
		}

		public Armada(Ciudad C, bool esDefensa = false) : this(C.CivDueno, C.Pos, esDefensa)
		{
		}

		float _MaxPeso;
		//  Probablemente, _MaxPeso sea una función que dependa de CivDueño.
		/// <summary>
		/// Devuelve o establece el máximo peso que puede cargar esta armada.
		/// </summary>
		/// <value>The max peso.</value>
		public float MaxPeso
		{
			get
			{
				return _MaxPeso;
			}
			set
			{
				_MaxPeso = Math.Max(value, Peso);	// No puedo reducir MaxPeso a menor que Peso.
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

		readonly Pseudoposicion _Posicion = new Pseudoposicion();

		/// <summary>
		/// Devuelve o establece el lugar donde está la armada.
		/// </summary>
		/// <value></value>
		public Pseudoposicion Posicion
		{
			get { return _Posicion; }
		}

		/// <summary>
		/// Agrega, mueve o junta unidad(es) a esta armada.
		/// </summary>
		/// <param name="U">El stack que se agregará o moverá.</param>
		public void AgregaUnidad(Stack U)
		{
			if (PosicionConsistente(U))
			{
				if (PesoLibre >= U.Peso)
				{
					U.AbandonaArmada();
					U.ArmadaPerteneciente = this;
					if (_Unidades.ContainsKey(U.RAW))
					{
						_Unidades[U.RAW].Cantidad += U.Cantidad;
						U = _Unidades[U.RAW];
					}
					else
						_Unidades.Add(U.RAW, U);
				}
			}
			else
			{
				// Más bien no es exception, sino un msg al usuario. //TODO
				System.Diagnostics.Debug.WriteLine("No se puede agregar unidad a armada si éstas no están en el mismo lugar");
			}
		}

		public void AgregaUnidad(UnidadRAW raw, ulong cantidad)
		{
			if (_Unidades.ContainsKey(raw))
			{
				_Unidades[raw].Cantidad += cantidad;
			}
			else
			{
				_Unidades.Add(raw, new Stack(raw, cantidad, this));
			}
		}

		/// <summary>
		/// Revisa si una armada y una unidad tienen la misma posición.
		/// </summary>
		/// <returns><c>true</c> si comparten el mismo lugar; <c>false</c> otherwise.</returns>
		/// <param name="U">La unidad con la que se comparará posición.</param>
		public bool PosicionConsistente(Stack U)
		{
			return Posicion == null || Posicion.Equals(U.Posicion);
		}

		/// <summary>
		/// Quita una unidad de la Armada.
		/// </summary>
		/// <param name="U">Unidad a quitar</param>
		public void QuitarUnidad(Stack U)
		{
			_Unidades.Remove(U.RAW);
		}

		/// <summary>
		/// Pelea durante t horas
		/// </summary>
		/// <param name="A">Armada</param>
		/// <param name="t">tiempo de pelea</param>
		/// <param name="r">Randomizer</param>
		public void Pelea(Armada A, float t, Random r = null)
		{
			if (this.Unidades.Count == 0 || A.Unidades.Count == 0)
				return;

			int i, j;
			if (r == null)
				r = new Random();

			Armada[] Arms = new Armada[2];
			Stack Ata;
			Stack Def;
			Arms[0] = this;
			Arms[1] = A;

			i = r.Next(2); // Arms[i] Inicia
			j = 1 - i;
			if (Arms[i].Unidades.Count > 0)
			{
				Ata = Arms[i].MayorDaño(Arms[j]);
				Def = Ata.MenorDaño(Arms[j]);
				Ata.CausaDaño(Def.ArmadaPerteneciente, Def.RAW, Ata, t);
				if (Def.Muerto)
					return;
			}
				
			i = j; // Arms[1 - 1] le sigue.
			j = 1 - i;
			if (Arms[i].Unidades.Count > 0)
			{
				Ata = Arms[i].MayorDaño(Arms[j]);
				Def = Ata.MenorDaño(Arms[j]);
				Ata.CausaDaño(Def.ArmadaPerteneciente, Def.RAW, Ata, t);
			}
		}

		/// <summary>
		/// Devuelve la unidad de maximin daño de this.Unidades a A.Unidades
		/// </summary>
		/// <returns>La unidad que hace el mayor daño menor.</returns>
		/// <param name="A">A.</param>
		Stack MayorDaño(Armada A)
		{
			float maxDaño = 0;
			float currDaño;
			Stack ret = null;
			foreach (var x in Unidades)
			{
				currDaño = x.DañoPropuesto(x.MenorDaño(A));
				if (currDaño >= maxDaño)
					ret = x;
			}
			System.Diagnostics.Debug.Assert(ret != null);
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

		public override string ToString()
		{
			string ret = string.Format("Pos: {0}", Posicion);
			foreach (var u in Unidades)
			{
				ret += string.Format("\n\tClase:{0}\tCantidad:{1}\tVitalidad:{2}", u, u.Cantidad, u.Vitalidad);
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
				if (Unidades.Count == 0)
					return 0;
				return Unidades.Min(x => x.RAW.Velocidad);
			}
		}

		/// <summary>
		/// Devuelve <c>true</c> sólo si esta armada se encuentra en terreno
		/// </summary>
		/// <value><c>true</c> if en terreno; otherwise, <c>false</c>.</value>
		public bool EnTerreno{ get { return Posicion.enOrigen(); } }

		/// <summary>
		/// Un Tick de la armada
		/// </summary>
		public void Tick(float t)
		{
			if (Orden.Ejecutar(t, this))
			{
				Orden = new Civ.Orden.OrdenEstacionado();
				CivDueño.AgregaMensaje(new IU.Mensaje("{0} llegó a {1}", this, Posicion));
			}
		}

		ICivilizacion _CivDueño;

		public ICivilizacion CivDueño
		{
			get
			{
				return _CivDueño;
			}
		}

		/// <summary>
		/// Devuelve un nuevo diccionario que asocia a cada UnidadRAW la lista de Unidades que tiene.
		/// </summary>
		/// <returns>The dictionary.</returns>
		public Dictionary <UnidadRAW, List<Stack>> ToDictionary()
		{
			Dictionary <UnidadRAW, List<Stack>> ret = new Dictionary<UnidadRAW, List<Stack>>();
			foreach (var x in Unidades)
			{
				if (!ret.ContainsKey(x.RAW))
					ret.Add(x.RAW, new List<Stack>());
				ret[x.RAW].Add(x);
			}
			return ret;
		}

		public Civ.Orden.Orden Orden = new Civ.Orden.OrdenEstacionado();

		/// <summary>
		/// Devuelve el stack que le corresponde a una clase de unidad
		/// </summary>
		public Stack this [UnidadRAW uRAW]
		{
			get
			{
				return UnidadesAgrupadas(uRAW);
			}
		}

		#endregion

		#region Daño

		/// <summary>
		/// Daña un stack
		/// </summary>
		/// <param name="unidad">Unidad.</param>
		/// <param name="daltaHP">Daño o cura (negativo es daño)</param>
		public void DañarStack(UnidadRAW unidad, Stack Atacante, float deltaHP)
		{
			Stack currStack = this[unidad];
			currStack.Dañar(-deltaHP, Atacante.RAW.Dispersion);
			if (currStack.HP < 0)
			{
				this._Unidades.Remove(unidad);
			}
		}

		#endregion

		#region IDisposable implementation

		/// <summary>
		/// Releases all resource used by the <see cref="Civ.Armada"/> object.
		/// Libera su posición en el mapa, así como sus apuntadores en su civilización.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Civ.Armada"/>. The <see cref="Dispose"/>
		/// method leaves the <see cref="Civ.Armada"/> in an unusable state. After calling <see cref="Dispose"/>, you must
		/// release all references to the <see cref="Civ.Armada"/> so the garbage collector can reclaim the memory that the
		/// <see cref="Civ.Armada"/> was occupying.</remarks>
		public void Dispose()
		{
			if (Unidades.Count == 0)
				throw new Exception("No se puede desechar una armada con unidades en ella.");
			IDisposable destr = (IDisposable)_Posicion;
			destr.Dispose();
			if (this.CivDueño != null)
				CivDueño.Armadas.Remove(this);
		}

		#endregion

		#region IPosicionable implementation

		Pseudoposicion IPosicionable.getPosicion()
		{
			return Posicion;
		}

		#endregion

		#region Comandos especiales

		#region Colonizar

		/// <summary>
		/// Devuelve true si al menos un stack puede colonizar.
		/// </summary>
		/// <returns><c>true</c>, if colonizar was pueded, <c>false</c> otherwise.</returns>
		public bool PuedeColonizar()
		{
			foreach (var x in _Unidades.Keys)
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
		public Ciudad Coloniza()
		{
			foreach (var x in _Unidades)
			{
				return x.Value.Colonizar();
			}
			return null;
		}

		#endregion

		#endregion
	}
}