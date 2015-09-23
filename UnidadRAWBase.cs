using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ListasExtra;
using Civ.Comandos;

namespace Civ.Data
{
	public class UnidadRAWBase : IUnidadRAW
	{
		List<String> _flags = new List<string> ();

		ListaPeso<Recurso> _Reqs = new ListaPeso<Recurso> ();

		/// <summary>
		/// Coste poblacional por cada unidad
		/// </summary>
		[DataMember (Name = "CostePoblación")]
		public ulong CostePoblacional;

		List<IComandoEspecial> _comandos = new List<IComandoEspecial> ();

		/// <summary>
		/// Devuelve los comandos especiales de la unidad
		/// </summary>
		IEnumerable<IComandoEspecial> IUnidadRAW.Comandos
		{
			get
			{
				return _comandos;
			}
		}

		/// <summary>
		/// Devuelve los comandos especiales de la unidad
		/// </summary>
		IList<IComandoEspecial> Comandos
		{
			get
			{
				return _comandos;
			}
		}

		/// <summary>
		/// Peso de cada unidad de este tipo
		/// </summary>
		[DataMember]
		public float Peso { get; set; }

		/// <summary>
		/// Nombre de la unidad
		/// </summary>
		[DataMember]
		public string Nombre { get; set; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>The position.</value>
		public Pseudoposicion Pos { get; set; }

		/// <summary>
		/// Velocidad de desplazamiento (unidades por hora)
		/// </summary>
		/// <value>The velocidad.</value>
		[DataMember]
		public float Velocidad { get; set; }

		/// <summary>
		/// Población productiva que requiere para entrenar.
		/// </summary>
		/// <value>The coste poblacion.</value>
		[DataMember (Name = "CostePoblación")]
		public ulong CostePoblación { get; set; }

		/// <summary>
		/// Cantidad de peso que puede cargar
		/// </summary>
		/// <value>The max carga.</value>
		[DataMember]
		public float MaxCarga { get; set; }

		/// <summary>
		/// Revisa si esta unidad tiene un flag
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		/// <param name="flag">Modificador.</param>
		public bool TieneFlag (string flag)
		{
			return _flags.Contains (flag);
		}

		public ulong MaxReclutables (ICiudad ciudad)
		{
			ulong MaxPorPoblacion = ciudad.TrabajadoresDesocupados / CostePoblacional;

			ulong MaxPorRecursos = ulong.MaxValue;
			foreach (var x in _Reqs)
			{
				MaxPorRecursos = (ulong)Math.Min (
					MaxPorRecursos,
					ciudad.Almacén [x.Key] / x.Value);
			}

			return Math.Min (MaxPorRecursos, MaxPorPoblacion);
		}

		/// <summary>
		/// Recluta una cantidad de estas unidades en una ciudad.
		/// </summary>
		/// <param name="cantidad">Cantidad a reclutar</param>
		/// <param name="ciudad">Ciudad dónde reclutar</param>
		public void Reclutar (ulong cantidad, ICiudad ciudad)
		{
			ulong realCantidad = Math.Min (cantidad, MaxReclutables (ciudad));

			#if DEBUG
			if (cantidad > realCantidad)
				Console.WriteLine ("Se pidió reclutar más de lo que puede.");
			#endif

			ciudad.Defensa.AgregaUnidad (this, realCantidad);
		}
	}
}

