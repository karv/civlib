using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ListasExtra;
using Civ.Comandos;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using Civ.Data.Import;

namespace Civ.Data
{
	//[DataContract(Name = "Unidad", IsReference = true)]
	public class UnidadRAW : IUnidadRAW
	{

		[DataMember (Name = "Requerimientos")]
		readonly ListaPeso<Recurso> _Reqs = new ListaPeso<Recurso> ();

		List<string> Flags { get; set; }


		/// <summary>
		/// Devuelve la ciencia requerida para entrenar a la unidad.
		/// </summary>
		public Ciencia ReqCiencia
		{
			get { return _ReqCiencia; }
			set { _ReqCiencia = value; }
		}

		[DataMember (Name = "Ciencia")]
		Ciencia _ReqCiencia;



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
		[DataMember]
		public float Peso { get; set; }

		/// <summary>
		/// Nombre de la unidad
		/// </summary>
		[DataMember]
		public string Nombre { get; set; }

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
			return Flags.Contains (flag);
		}

		public ulong MaxReclutables (ICiudad ciudad)
		{
			ulong MaxPorPoblacion = ciudad.TrabajadoresDesocupados / CostePoblación;

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



		[DataMember]
		public float Puntuación { get; set; }

		public bool EstaDisponible (ICivilización civil)
		{
			return civil.Avances.Contains (ReqCiencia);
		}

		#endregion

		public override string ToString ()
		{
			return Nombre;
		}

		/// <summary>
		/// Requerimientos para crearse.
		/// </summary>
		public ListaPeso<Recurso> Reqs
		{
			get { return _Reqs; }
		}

		#region IImportable

		string _req_ciencia_id;
		ListaPeso<string> _reqs_id = new ListaPeso<string> ();

		void IImportable.Importar (System.IO.StreamReader reader)
		{
			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine ();
				line.ToLower ();
				var spl = line.Split (':');
				for (int i = 0; i < spl.Length; i++)
				{
					spl [i] = spl [i].Trim ();
				}
				LeerLínea (spl);
			}
		}

		/// <summary>
		/// Importa una línea dada para configurarme.
		/// </summary>
		/// <param name="spl">Una línea, separada por sus sustantivos.</param>
		protected virtual void LeerLínea (string [] spl)
		{
			switch (spl [0])
			{
				case "nombre":
					Nombre = spl [1];
					return;
				case "población":
					CostePoblación = ulong.Parse (spl [1]);
					return;
				case "flag":
					Flags.Add (spl [1]);
					return;
				case "carga":
					MaxCarga = float.Parse (spl [1]);
					return;
				case "peso":
					Peso = float.Parse (spl [1]);
					return;
				case "avance":
					_req_ciencia_id = spl [1];
					return;
				case "requiere":
					_reqs_id.Add (spl [1], float.Parse (spl [2]));
					return;
				case "velocidad":
					Velocidad = float.Parse (spl [1]);
					return;
			}
		}

		void IImportable.Vincular ()
		{
			ReqCiencia = ImportMachine.Valor (_req_ciencia_id) as Ciencia;
			foreach (var x in _reqs_id)
			{
				Reqs.Add (ImportMachine.Valor (x.Key) as Recurso, x.Value);
			}

			// Limpiar
			_req_ciencia_id = null;
			_reqs_id = null;
		}

		#endregion
	}
}

