using System;
using System.Collections.Generic;
using Civ.Global;
using ListasExtra;

namespace Civ.RAW
{
	/// <summary>
	/// Representa una clase de unidad
	/// </summary>
	[Serializable]
	public class UnidadRAWCombate : UnidadRAW, IPuntuado, IUnidadRAWCombate
	{
		/// <summary>
		/// Modificadores de combate por flag objetivo.
		/// </summary>
		[Serializable]
		public class Modificadores : ListaPeso<string>
		{
		}

		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.RAW.UnidadRAWCombate"/> class.
		/// </summary>
		public UnidadRAWCombate ()
		{
			Mods = new Modificadores ();
		}

		#endregion

		#region Combate

		/// <summary>
		/// Devuelve o establece el ataque de esta clase de unidad
		/// </summary>
		/// <value>The ataque.</value>
		public float Ataque { get; set; }

		/// <summary>
		/// Lista de modificadores de combate de la unidad.
		/// </summary>        
		public Modificadores Mods { get; }

		float IUnidadRAWCombate.getModificador (string modificador)
		{
			return Mods [modificador];
		}

		IEnumerable<string> IUnidadRAWCombate.Modificadores{ get { return Mods.Keys; } }

		/// <summary>
		/// Flotante en [0, 1]
		/// Qué tanto se dispersa el daño entre el stack enemigo.
		/// </summary>
		public float Dispersión { get; set; }

		#endregion

		#region IPuntuado

		float IPuntuado.Puntuación
		{
			get
			{
				return Puntuación;
			}
		}

		#endregion
	}
}