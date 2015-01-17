using System;
using ListasExtra;
using System.Collections.Generic;


namespace Civ
{
	public partial class Ciudad
	{
		//Población y crecimiento.

		/// <summary>
		/// Recurso que será el alimento
		/// </summary>
		public static Recurso RecursoAlimento {
			get {
				return Global.g_.Data.RecAlimento;
			}
		}

		/// <summary>
		/// Número de infantes que nacen por (PoblaciónProductiva*Tick) Base.
		/// </summary>
		public static float TasaNatalidadBase = 0.2f;

		/// <summary>
		/// Probabilidad base de un infante arbitrario de morir en cada tick.
		/// </summary>
		public static float TasaMortalidadInfantilBase = 0.01f;

		/// <summary>
		/// Probabilidad base de un habitante productivo arbitrario de morir en cada tick.
		/// </summary>
		public static float TasaMortalidadProductivaBase = 0.02f;

		/// <summary>
		/// Probabilidad base de un adulto de la tercera edad arbitrario de morir en cada tick.
		/// </summary>
		public static float TasaMortalidadVejezBase = 0.1f;

		/// <summary>
		/// Probabilidad de que un infante se convierta en productivo
		/// </summary>
		public static float TasaDesarrolloBase = 0.2f;

		/// <summary>
		/// Probabilidad de que un Productivo envejezca
		/// </summary>
		public static float TasaVejezBase = 0.05f;

		/// <summary>
		/// Consumo base de alimento por ciudadano.
		/// </summary>
		public static float ConsumoAlimentoPorCiudadanoBase = 1f;

		//Población
		float _PoblaciónProductiva = 10f;
		float _PoblaciónPreProductiva = 0;
		float _PoblaciónPostProductiva = 0;

		/// <summary>
		/// Devuelve la población real y total de la ciudad.
		/// </summary>
		/// <value>The get real población.</value>
		public float getRealPoblación {
			get {
				return _PoblaciónProductiva + _PoblaciónPreProductiva+ _PoblaciónPostProductiva;
			}
		}

		/// <summary>
		/// Devuelve la población de la ciudad.
		/// </summary>
		/// <value>The get poplación.</value>
		public ulong getPoblación {
			get {
				return PoblaciónProductiva + getPoblaciónPreProductiva + getPoblaciónPostProductiva;
			}
		}

		/// <summary>
		/// Devuelve la población productiva.
		/// </summary>
		/// <value></value>
		public ulong PoblaciónProductiva {
			get {
				return (ulong)Math.Floor (_PoblaciónProductiva);
			}
			set	{
				_PoblaciónProductiva = value;
			}
		}

		/// <summary>
		/// Devuelve la población pre productiva.
		/// </summary>
		/// <value></value>
		public ulong getPoblaciónPreProductiva {
			get {
				return (ulong)Math.Floor (_PoblaciónPreProductiva);
			}
		}

		/// <summary>
		/// Devuelve la población post productiva.
		/// </summary>
		/// <value></value>
		public ulong getPoblaciónPostProductiva {
			get {
				return (ulong)Math.Floor (_PoblaciónPostProductiva);
			}
		}
	}
}

