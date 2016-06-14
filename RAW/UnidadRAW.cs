using System;
using System.Collections.Generic;
using ListasExtra;
using Civ.Comandos;
using Civ.Ciencias;
using Civ.ObjetosEstado;

namespace Civ.RAW
{
	//[DataContract(Name = "Unidad", IsReference = true)]
	[Serializable]
	public class UnidadRAW : IUnidadRAW
	{

		#region ctor

		public UnidadRAW ()
		{
			Flags = new List<string> ();
		}

		#endregion

		#region Requicitos

		readonly ListaPeso<Recurso> _Reqs = new ListaPeso<Recurso> ();

		/// <summary>
		/// Devuelve la ciencia requerida para entrenar a la unidad.
		/// </summary>
		public Ciencia ReqCiencia { get; set; }

		/// <summary>
		/// Requerimientos para crearse.
		/// </summary>
		public ListaPeso<Recurso> Reqs
		{
			get { return _Reqs; }
		}

		#endregion

		#region General

		/// <summary>
		/// Flags.
		/// </summary>
		public List<string> Flags { get; }

/// 

		public override string ToString ()
		{
			return Nombre;
		}

		#endregion

		#region UnidadRAW

		/// <summary>
		/// Devuelve los comandos especiales de la unidad
		/// </summary>
		IEnumerable<IComandoEspecial> IUnidadRAW.Comandos
		{
			get
			{
				return Comandos;
			}
		}

		/// <summary>
		/// Devuelve los comandos especiales de la unidad
		/// </summary>
		public IList<IComandoEspecial> Comandos { get; }

		/// <summary>
		/// Peso de cada unidad de este tipo
		/// </summary>
		public float Peso { get; set; }

		/// <summary>
		/// Nombre de la unidad
		/// </summary>
		public string Nombre { get; set; }

		/// <summary>
		/// Velocidad de desplazamiento (unidades por hora)
		/// </summary>
		/// <value>The velocidad.</value>
		public float Velocidad { get; set; }

		/// <summary>
		/// Población productiva que requiere para entrenar.
		/// </summary>
		/// <value>The coste poblacion.</value>
		public ulong CostePoblación { get; set; }

		/// <summary>
		/// Cantidad de peso que puede cargar
		/// </summary>
		/// <value>The max carga.</value>
		public float MaxCarga { get; set; }

		/// <summary>
		/// Revisa si esta unidad tiene un flag
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="flag">Modificador.</param>
		public bool TieneFlag (string flag)
		{
			return Flags.Contains (flag);
		}

		public ulong MaxReclutables (ICiudad ciudad)
		{
			ulong MaxPorPoblación;
			if (CostePoblación != 0)
				MaxPorPoblación = ciudad.TrabajadoresDesocupados / CostePoblación;
			else
			{
				MaxPorPoblación = ulong.MaxValue;
			}

			ulong MaxPorRecursos = ulong.MaxValue;
			foreach (var x in _Reqs)
			{
				MaxPorRecursos = (ulong)Math.Min (
					MaxPorRecursos,
					ciudad.Almacén [x.Key] / x.Value);
			}

			return Math.Min (MaxPorRecursos, MaxPorPoblación);
		}

		/// <summary>
		/// Recluta una cantidad de estas unidades en una ciudad.
		/// </summary>
		/// <param name="cantidad">Cantidad a reclutar</param>
		/// <param name="ciudad">Ciudad dónde reclutar</param>
		public void Reclutar (ulong cantidad, ICiudad ciudad)
		{
			ulong realCantidad = Math.Min (cantidad, MaxReclutables (ciudad));

			ciudad.Defensa.AgregaUnidad (this, realCantidad);
		}



		public float Puntuación { get; set; }

		public bool EstaDisponible (ICivilización civil)
		{
			return ReqCiencia == null || civil.Avances.Contains (ReqCiencia);
		}

		#endregion

		#region Combate

		/// <summary>
		/// Fuerza de combate
		/// </summary>
		/// <value>La fuerza de combate</value>
		public float Defensa { get; set; }
///

		#endregion
	}
}