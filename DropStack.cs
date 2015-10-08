using System;
using System.Collections.Generic;
using ListasExtra;
using Civ.Data;

namespace Civ
{
	public class DropStack: IPosicionable, IAlmacén
	{
		public DropStack (Pseudoposicion pos)
		{
			Almacén = new ListaPeso<Recurso> ();
			_posicion = pos;
		}

		public ListaPeso<Recurso> Almacén { get; }

		#region Posición

		readonly Pseudoposicion _posicion;

		public Pseudoposicion Posición ()
		{
			return _posicion;
		}

		#endregion

		#region Almacén

		public override int GetHashCode ()
		{
			unchecked
			{
				return (_posicion != null ? _posicion.GetHashCode () : 0);
			}
		}

		IEnumerable<Recurso> IAlmacénRead.recursos
		{
			get
			{
				return Almacén.Keys;
			}
		}

		float IAlmacénRead.recurso (Recurso recurso)
		{
			return Almacén [recurso];
		}

		[ObsoleteAttribute]
		void IAlmacén.SetRecurso (Recurso rec, float val)
		{
			Almacén [rec] = val;
		}

		void IAlmacén.ChangeRecurso (Recurso rec, float delta)
		{
			Almacén [rec] += delta;
		}

		float IAlmacén.this [Recurso recurso]
		{
			get
			{
				return Almacén [recurso];
			}
			set
			{
				Almacén [recurso] = value;
			}
		}

		float IAlmacénRead.this [Recurso recurso]
		{
			get
			{
				return Almacén [recurso];
			}
		}

		#endregion
	}
}

