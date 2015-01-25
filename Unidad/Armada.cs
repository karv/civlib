using System;
using ListasExtra;
using Basic;
using System.Collections.Generic;

namespace Civ
{
	/// <summary>
	/// Representa un conjunto de unidades.
	/// </summary>
	public class Armada
	{
		List<Unidad> _Unidades = new List<Unidad>();

		/// <summary>
		/// Devuelve la lista de unidades en la armada.
		/// </summary>
		/// <value>The lista unidades.</value>
		public List<Unidad> Unidades {
			get {
				return _Unidades;
			}
		}

		float _MaxPeso; //  Probablemente, _MaxPeso sea una función que dependa de CivDueño.

		/// <summary>
		/// Devuelve o establece el máximo peso que puede cargar esta armada.
		/// </summary>
		/// <value>The max peso.</value>
		public float MaxPeso {
			get {
				return _MaxPeso;
			}
			set {
				_MaxPeso = Math.Max (value, Peso);	// No puedo reducir MaxPeso a menor que Peso.
			}
		}

		/// <summary>
		/// Devuelve el peso actual de la armada. (A lo reduccionista)
		/// </summary>
		public float Peso
		{
			get {
				float ret = 0;
				foreach (var x in Unidades) {
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
			get {
				return MaxPeso - Peso;
			}
		}

		/// <summary>
		/// Devuelve el lugar donde está la armada.
		/// </summary>
		/// <value></value>
		public IPosicion Posición {
			get {
				return Unidades.Count > 0 ? Unidades [0].Posición : null;
			}
		}

		/// <summary>
		/// Agrega unidad(es) a esta armada
		/// </summary>
		/// <param name="U">La unidad que se agregará.</param>
		public void AgregaUnidad (Unidad U)
		{
			if (PosiciónConsistente(U)) {
				if (PesoLibre >= U.Peso) {
					U.AbandonaArmada ();
					U.ArmadaPerteneciente = this;
					Unidades.Add (U);
				}
			} else {
				throw new Exception ("No se puede agregar unidad a armada si éstas no están en el mismo lugar"); // Más bien no es exception, sino un msg al usuario.
			}
		}

		/// <summary>
		/// Revisa si una armada y una unidad tienen la misma posición.
		/// </summary>
		/// <returns><c>true</c> si comparten el mismo lugar; <c>false</c> otherwise.</returns>
		/// <param name="U">La unidad con la que se comparará posición.</param>
		public bool PosiciónConsistente (Unidad U)
		{
			return Posición == null || Posición == U.Posición;
		}

        /// <summary>
        /// Quita una unidad de la Armada.
        /// </summary>
        /// <param name="U">Unidad a quitar</param>
		public void QuitarUnidad (Unidad U)
		{
			Unidades.Remove (U);
		}
	}

	// TODO: Hacer clase interna "Orden", que lleve información de hacia dónde va a qué va. Necesitará gráficas.
}