//
//  ListaInvestigación.cs
//
//  Author:
//       Edgar Carballo <karvayoEdgar@gmail.com>
//
//  Copyright (c) 2015 edgar
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using Civ.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Civ
{
	[Serializable]
	/// <summary>
	/// Representa la lista de ciencias que se están investigando.
	/// </summary>
	public class ListaInvestigación : HashSet<InvestigandoCiencia>
	{
		public ListaInvestigación ()
		{
		}

		protected ListaInvestigación (System.Runtime.Serialization.SerializationInfo info,
		                              System.Runtime.Serialization.StreamingContext context)
			: base (info, context)
		{
		}

		/// <summary>
		/// Agrega cierta cantidad de recursos, a la investigación de una ciencia.
		/// </summary>
		/// <param name="ciencia">Ciencia investigando.</param>
		/// <param name="recurso">Recurso del que se agrega.</param>
		/// <param name="cantidad">Cantidad de tal recurso.</param>
		public void Invertir (Ciencia ciencia, Recurso recurso, float cantidad)
		{
			if (this.All (x => x.Ciencia != ciencia)) // Si no existe la ciencia C en la lista, se agrega
				Add (new InvestigandoCiencia (ciencia));

			InvestigandoCiencia IC;
			IC = this.First (x => x.Ciencia == ciencia);
			IC [recurso] += cantidad;
		}

		/// <summary>
		/// Encuentra la instancia (si existe) de una ciencia.
		/// </summary>
		/// <returns>The instancia.</returns>
		/// <param name="ciencia">Ciencia a buscar</param>
		public InvestigandoCiencia EncuentraInstancia (Ciencia ciencia)
		{
			return this.FirstOrDefault (x => x.Ciencia == ciencia);
		}

		/// <Docs>The item to remove from the current collection.</Docs>
		/// <para>Removes the first occurrence of an item from the current collection.</para>
		/// <summary>
		/// Elimina una ciencia de esta lista
		/// </summary>
		/// <param name="ciencia">Ciencia a eliminar</param>
		public bool Remove (Ciencia ciencia)
		{
			return base.Remove (EncuentraInstancia (ciencia));
		}
	}
}