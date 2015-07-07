using ListasExtra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Civ
{
	/// <summary>
	/// Representa un conjunto de unidades.
	/// </summary>
	public class Armada
	{
		#region General

		ListaPeso<UnidadRAW, Stack> _Unidades = new ListaPeso<UnidadRAW, Stack>((x, y) => Stack.Merge(x, y), null, new System.Collections.Concurrent.ConcurrentDictionary<UnidadRAW, Stack>());

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

		public Armada(Civilizacion C, bool esDefensa = false)
		{
			_CivDueño = C;
			this.esDefensa = esDefensa;
			C.Armadas.Add(this);
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

		Pseudoposicion _Posicion;

		/// <summary>
		/// Devuelve o establece el lugar donde está la armada.
		/// </summary>
		/// <value></value>
		public Pseudoposicion Posicion
		{
			get { return _Posicion; }
			set { _Posicion = value; }
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
						_Unidades[U.RAW].cantidad += U.cantidad;
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
				_Unidades[raw].cantidad += cantidad;
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
		/// Pelea durante t chronons
		/// </summary>
		/// <param name="A">Armada</param>
		/// <param name="t">tiempo de pelea</param>
		/// <param name="r">Randomizer</param>
		public void Pelea(Armada A, float t, Random r = null)
		{
			if (r == null)
				r = new Random();

			Armada[] Arms = new Armada[2];
			Stack Ata;
			Stack Def;
			Arms[0] = this;
			Arms[1] = A;

			int i = r.Next(2); // Arms[i] Inicia
			int j = 1 - i;
			Ata = Arms[i].MayorDaño(Arms[j]);
			Def = Ata.MenorDaño(Arms[j]);
			Ata.CausaDaño(Def, t);

			i = j; // Arms[1 - 1] le sigue.
			j = 1 - i;
			Ata = Arms[i].MayorDaño(Arms[j]);
			Def = Ata.MenorDaño(Arms[j]);
			Ata.CausaDaño(Def, t);
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
				if (currDaño > maxDaño)
					ret = x;
			}
			return ret;
		}

		public override string ToString()
		{
			return string.Format("[Armada: Unidades={0}, MaxPeso={1}, Peso={2}, PesoLibre={3}, Posición={4}]", Unidades, MaxPeso, Peso, PesoLibre, Posicion);
		}

		/// <summary>
		/// Velocidad de desplazamiento
		/// </summary>
		public float Velocidad;

		/// <summary>
		/// Devuelve <c>true</c> sólo si esta armada se encuentra en terreno
		/// </summary>
		/// <value><c>true</c> if en terreno; otherwise, <c>false</c>.</value>
		public bool EnTerreno
		{
			get
			{
				return Posicion.Avance == 0;
			}
		}

		/// <summary>
		/// Un Tick de la armada
		/// </summary>
		public void Tick(float t)
		{
			if (Orden.Ejecutar(t, this))
				Orden = new Civ.Orden.OrdenEstacionado();
		}

		Civilizacion _CivDueño;

		public Civilizacion CivDueño
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

		public Civ.Orden.Orden Orden;

		#endregion
	}
}