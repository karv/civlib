using ListasExtra;
using System.Collections.Generic;
using System;
using Civ.ObjetosEstado;
using Civ.Global;

namespace Civ.RAW
{
	/// <summary>
	/// Representa un trabajo en un edificioRAW
	/// </summary>	
	[Serializable]
	public class TrabajoRAW
	{
		#region ctor

		/// <summary>
		/// Initializes a new instance of the <see cref="Civ.RAW.TrabajoRAW"/> class.
		/// </summary>
		public TrabajoRAW ()
		{
			EntradaBase = new ListaPeso<Recurso> ();
			SalidaBase = new ListaPeso<Recurso> ();
		}

		#endregion

		#region General

		/// <summary>
		/// Nombre
		/// </summary>
		public string Nombre;

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Civ.RAW.TrabajoRAW"/>.
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="Civ.RAW.TrabajoRAW"/>.</returns>
		public override string ToString ()
		{
			return string.Format ("{0} @ {1}", Nombre, Edificio);
		}

		#endregion

		#region Edificio

		EdificioRAW _edificio;

		/// <summary>
		/// EdificioRAW vinculado a este trabajo.
		/// </summary>
		public EdificioRAW Edificio
		{
			get
			{
				return _edificio;
			}
			set
			{
				if (_edificio != null)
				{
					_edificio.Trabajos.Remove (this);
					#if DEBUG
					Console.WriteLine (string.Format (
						"Se ha cambiado edificio del trabajo {0} desde {1} hasta {2}",
						this,
						_edificio,
						value));
					#endif
				}
				_edificio = value;
				_edificio.Trabajos.Add (this);
			}
		}

		#endregion

		#region Producción

		/// <summary>
		/// Recursos producidos por trabajador*turno (Base)
		/// </summary>
		public ListaPeso<Recurso> SalidaBase { get; }

		/// <summary>
		/// Recursos consumidos por trabajador*turno (Base)
		/// </summary>
		public ListaPeso<Recurso> EntradaBase { get; }

		#endregion

		#region Reqs

		/// <summary>
		/// Lista de requerimientos.
		/// </summary>
		public Requerimiento Requiere = new Requerimiento ();

		/// <summary>
		/// Devuelve la lista de requerimientos.
		/// </summary>
		/// <value>El IRequerimiento</value> 
		public ICollection<IRequerimiento<ICiudad>> Reqs ()
		{
			return Requiere.Requiere ();
		}

		#endregion
	}
}