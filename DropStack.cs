﻿using System;
using System.Collections.Generic;
using ListasExtra;

namespace Civ
{
	public class DropStack: IPosicionable, IAlmacén
	{
		public DropStack(Pseudoposicion pos)
		{
			Almacén = new ListaPeso<Recurso>();
			_posicion = pos;
		}

		public ListaPeso<Recurso> Almacén { get; }

		#region Posición

		readonly Pseudoposicion _posicion;

		public Pseudoposicion Posicion()
		{
			return _posicion;
		}

		#endregion

		#region Almacén

		IEnumerable<Recurso> IAlmacénRead.recursos
		{
			get
			{
				return Almacén.Keys;
			}
		}

		float IAlmacénRead.recurso(Recurso recurso)
		{
			return Almacén[recurso];
		}

		[ObsoleteAttribute]
		void IAlmacén.SetRecurso(Recurso rec, float val)
		{
			Almacén[rec] = val;
		}

		void IAlmacén.ChangeRecurso(Recurso rec, float delta)
		{
			Almacén[rec] += delta;
		}

		#endregion
	}
}
