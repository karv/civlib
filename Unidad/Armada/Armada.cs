using System;
using ListasExtra;
using Basic;
using System.Collections.Generic;

namespace Civ
{
	/// <summary>
	/// Representa un conjunto de unidades.
	/// </summary>
	public partial class Armada
	{
		List<Unidad> _Unidades = new List<Unidad>();

		/// <summary>
		/// Devuelve la lista de unidades en la armada.
		/// </summary>
		/// <value>The lista unidades.</value>
		public List<Unidad> Unidades
		{
			get
			{
				return _Unidades;
			}
		}

		public Armada(Civilizacion C)
		{
			_CivDueño = C;
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
		/// Agrega unidad(es) a esta armada
		/// </summary>
		/// <param name="U">La unidad que se agregará.</param>
		public void AgregaUnidad(Unidad U)
		{
			if (PosicionConsistente(U))
			{
				if (PesoLibre >= U.Peso)
				{
					U.AbandonaArmada();
					U.ArmadaPerteneciente = this;
					Unidades.Add(U);
				}
			}
			else
			{
				throw new Exception("No se puede agregar unidad a armada si éstas no están en el mismo lugar"); // Más bien no es exception, sino un msg al usuario.
			}
		}

		/// <summary>
		/// Revisa si una armada y una unidad tienen la misma posición.
		/// </summary>
		/// <returns><c>true</c> si comparten el mismo lugar; <c>false</c> otherwise.</returns>
		/// <param name="U">La unidad con la que se comparará posición.</param>
		public bool PosicionConsistente(Unidad U)
		{
			return Posicion == null || Posicion == U.Posición;
		}

		/// <summary>
		/// Quita una unidad de la Armada.
		/// </summary>
		/// <param name="U">Unidad a quitar</param>
		public void QuitarUnidad(Unidad U)
		{
			Unidades.Remove(U);
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
			Unidad Ata;
			Unidad Def;
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
		Unidad MayorDaño(Armada A)
		{
			float maxDaño = 0;
			float currDaño;
			Unidad ret = null;
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
			switch (Orden.TipoOrden)
			{
				case Orden.enumTipoOrden.Ir:
					Pseudoposicion PS;
					if (EnTerreno)
					{
						// Convertir Posición en Pseudoposición.
						PS = new Pseudoposicion();
						PS.Avance = 0;
						PS.Destino = Orden.Destino.Destino;
						PS.Origen = Posicion.Origen;

						Posicion = PS;
					}
					// Para este encontes, Posición debería ser una auténtica Pseudoposición
					PS = Posicion;
					// Avanzar
					PS.Avance += t * Velocidad;


					//Revisar si están en el mismo Terreno-intervalo
					if (PS.Origen == Orden.Destino.Origen && PS.Destino == Orden.Destino.Destino) // Esto debe pasar siempre, por ahora.
					{
						if (PS.Avance >= Orden.Destino.Avance)
						{
							// Ya llegó.
							CivDueño.AgregaMensaje(new IU.Mensaje("Armada {0} LLegó a su destino en {1} : Orden {2}", this, Orden.Destino, Orden));
							Orden = null;
						}
					}
					break;
				default:
					break;
			}
		}

		Civilizacion _CivDueño;

		public Civilizacion CivDueño
		{
			get
			{
				return _CivDueño;
			}
		}
	}
}