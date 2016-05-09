using Civ.RAW;
using Civ.ObjetosEstado;

namespace Civ.IU
{
	public interface IRepetidor
	{
		bool Coincide (IRepetidor rep);
		
	}


	public struct RepetidorExcesoRecurso : IRepetidor
	{
		public Recurso Recurso { get; }

		public IAlmacénRead Almacén { get; }

		public bool Coincide (IRepetidor obj)
		{
			if (obj is RepetidorExcesoRecurso)
			{
				var otro = (RepetidorExcesoRecurso)obj;
				return Recurso == otro.Recurso && Almacén == otro.Almacén;
			}
			return false;
		}

		public RepetidorExcesoRecurso (Recurso recurso, IAlmacénRead almacén)
		{
			Recurso = recurso;
			Almacén = almacén;
		}

		public override int GetHashCode ()
		{
			unchecked
			{
				return (Recurso != null ? Recurso.GetHashCode () : 0) ^ (Almacén != null ? Almacén.GetHashCode () : 0);
			}
		}
		
	}

	public struct RepetidorArmadaDestino : IRepetidor
	{
		public Armada Armada;

		public RepetidorArmadaDestino (Armada armada)
		{
			Armada = armada;
		}

		public bool Coincide (IRepetidor obj)
		{
			if (obj is RepetidorArmadaDestino)
			{
				var otro = (RepetidorArmadaDestino)obj;
				return Armada == otro.Armada;
			}
			return false;
		}
	}

	public struct RepetidorCiudadNoPop : IRepetidor
	{
		public ICiudad Ciudad;

		public RepetidorCiudadNoPop (ICiudad ciudad)
		{
			Ciudad = ciudad;
		}

		public bool Coincide (IRepetidor obj)
		{
			if (obj is RepetidorCiudadNoPop)
			{
				var otro = (RepetidorCiudadNoPop)obj;
				return Ciudad == otro.Ciudad;
			}
			return false;
		}
	}

	public struct RepetidorEntero : IRepetidor
	{
		public int Código;

		public RepetidorEntero (int code)
		{
			Código = code;
		}

		public bool Coincide (IRepetidor obj)
		{
			if (obj is RepetidorEntero)
			{
				var otro = (RepetidorEntero)obj;
				return Código == otro.Código;
			}
			return false;
		}
	}
}