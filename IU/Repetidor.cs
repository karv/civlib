using System;
using Civ.RAW;

namespace Civ.IU
{
	public struct RepetidorExcesoRecurso : IEquatable<RepetidorExcesoRecurso>
	{
		public Recurso Recurso { get; }

		public IAlmacénRead Almacén { get; }

		public bool Equals (RepetidorExcesoRecurso obj)
		{
			return Recurso == obj.Recurso && Almacén == obj.Almacén;
		}


		public override int GetHashCode ()
		{
			unchecked
			{
				return (Recurso != null ? Recurso.GetHashCode () : 0) ^ (Almacén != null ? Almacén.GetHashCode () : 0);
			}
		}
		
	}
}