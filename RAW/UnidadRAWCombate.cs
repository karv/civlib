﻿using System.Collections.Generic;
using ListasExtra;
using Civ;
using System;

namespace Civ.RAW
{
	/// <summary>
	/// Representa una clase de unidad
	/// </summary>
	[Serializable]
	public class UnidadRAWCombate : UnidadRAW, IPuntuado, IUnidadRAWCombate
	{
		[Serializable]
		public class Modificadores : ListaPeso<string>
		{
		}

		public UnidadRAWCombate ()
		{
			Mods = new Modificadores ();
		}

		public float Ataque { get; }

		#region IPuntuado

		float IPuntuado.Puntuación
		{
			get
			{
				return Defensa;
			}
		}

		#endregion

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
	}
}