using Civ.RAW;
using Civ.ObjetosEstado;
using Civ.Almacén;

namespace Civ.IU
{
	/// <summary>
	/// Provee métodos para saber si dos mansajes son equivalentes
	/// </summary>
	public interface IRepetidor
	{
		/// <summary>
		/// Si este repetidos coincide con otro
		/// </summary>
		/// <param name="rep">Otro</param>
		bool Coincide (IRepetidor rep);
	}

	/// <summary>
	/// Ocurre cuando hay excedente (bajo o alto) de recursos en almacén. 
	/// (?)
	/// </summary>
	public struct RepetidorExcesoRecurso : IRepetidor
	{
		/// <summary>
		/// El recurso en cuestión
		/// </summary>
		public Recurso Recurso { get; }

		/// <summary>
		/// Almacén donde se dio el exceso
		/// </summary>
		/// <value>The almacén.</value>
		public IAlmacénRead Almacén { get; }

		/// <summary>
		/// Si este repetidos coincide con otro
		/// </summary>
		/// <param name="obj">Otro.</param>
		public bool Coincide (IRepetidor obj)
		{
			if (obj is RepetidorExcesoRecurso)
			{
				var otro = (RepetidorExcesoRecurso)obj;
				return Recurso == otro.Recurso && Almacén == otro.Almacén;
			}
			return false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.IU.RepetidorExcesoRecurso"/> struct.
		/// </summary>
		/// <param name="recurso">Recurso.</param>
		/// <param name="almacén">Almacén.</param>
		public RepetidorExcesoRecurso (Recurso recurso, IAlmacénRead almacén)
		{
			Recurso = recurso;
			Almacén = almacén;
		}

		/// <summary>
		/// Serves as a hash function for a <see cref="Civ.IU.RepetidorExcesoRecurso"/> object.
		/// </summary>
		/// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
		public override int GetHashCode ()
		{
			unchecked
			{
				return (Recurso != null ? Recurso.GetHashCode () : 0) ^ (Almacén != null ? Almacén.GetHashCode () : 0);
			}
		}
		
	}

	/// <summary>
	/// Ocurre cuando una armada llega a su destino
	/// </summary>
	public struct RepetidorArmadaDestino : IRepetidor
	{
		/// <summary>
		/// Armada
		/// </summary>
		public Armada Armada;

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.IU.RepetidorArmadaDestino"/> struct.
		/// </summary>
		/// <param name="armada">Armada.</param>
		public RepetidorArmadaDestino (Armada armada)
		{
			Armada = armada;
		}

		/// <summary>
		/// Si este repetidos coincide con otro
		/// </summary>
		/// <param name="obj">Otro.</param>
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

	/// <summary>
	/// Ocurre cuando la ciudad pierde trabajadores ocupados
	/// </summary>
	public struct RepetidorCiudadNoPop : IRepetidor
	{
		/// <summary>
		/// La ciudad
		/// </summary>
		public ICiudad Ciudad;

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.IU.RepetidorCiudadNoPop"/> struct.
		/// </summary>
		/// <param name="ciudad">Ciudad.</param>
		public RepetidorCiudadNoPop (ICiudad ciudad)
		{
			Ciudad = ciudad;
		}

		/// <summary>
		/// Si este repetidos coincide con otro
		/// </summary>
		/// <param name="obj">Otro</param>
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

	/// <summary>
	/// Un repetidor con Id entero
	/// </summary>
	public struct RepetidorEntero : IRepetidor
	{
		/// <summary>
		/// ID
		/// </summary>
		public int Código;

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.IU.RepetidorEntero"/> struct.
		/// </summary>
		/// <param name="code">Code.</param>
		public RepetidorEntero (int code)
		{
			Código = code;
		}

		/// <summary>
		/// Si este repetidos coincide con otro
		/// </summary>
		/// <param name="obj">Otro</param>
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