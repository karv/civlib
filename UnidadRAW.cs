using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ListasExtra;
using Civ.Comandos;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace Civ.Data
{
	//[DataContract(Name = "Unidad", IsReference = true)]
	public class UnidadRAW : IUnidadRAW, IXmlSerializable
	{
		
		#region XmlSerial

		XmlSchema IXmlSerializable.GetSchema ()
		{
			return(null);
		}

		public void WriteXml (XmlWriter writer)
		{

			writer.WriteElementString ("Nombre", Nombre);
			writer.WriteElementString ("Peso", Peso.ToString ());
			writer.WriteElementString ("Población", CostePoblacional.ToString ());
			writer.WriteElementString ("Valor", Puntuacion.ToString ());
			new XmlSerializer (typeof (Ciencia)).Serialize (writer, ReqCiencia);
			new XmlSerializer (typeof (ListaPeso<Recurso>)).Serialize (writer, Reqs);
			//writer.WriteElementString("Ciencia", cien);
			//new XmlSerializer(typeof(float)).Serialize(writer, Peso);
			//new XmlSerializer(typeof(ulong)).Serialize(writer, CostePoblacional);
			//new XmlSerializer(typeof(float)).Serialize(writer, Puntuacion);

			//new XmlSerializer(typeof(ListasExtra.ListaPeso<Recurso>)).Serialize(writer, Reqs);





			//			serializer.Serialize(writer, CostePoblacional);
		}

		public void ReadXml (XmlReader reader)
		{			
			//reader.ReadStartElement("Unidades");
			while (!reader.EOF)
			{
				reader.Read ();
				switch (reader.Name)
				{
					case "Nombre":
						Nombre = reader.ReadElementString ();
						break;
					case "Peso":
						Peso = reader.ReadElementContentAsFloat ();
						break;
					case "Población":
						CostePoblacional = (ulong)reader.ReadElementContentAsLong ();
						break;
					case "Valor":
						Puntuacion = reader.ReadElementContentAsFloat ();
						break;
				}
			}

		}

		#endregion

		[DataMember (Name = "Requerimientos")]
		readonly ListaPeso<Recurso> _Reqs = new ListaPeso<Recurso> ();

		List<string> Flags { get; set; }

		/// <summary>
		/// Coste poblacional por cada unidad
		/// </summary>
		[DataMember (Name = "CostePoblación")]
		public ulong CostePoblacional;

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

		public float Puntuación
		{
			get
			{
				throw new NotImplementedException ();
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
		/// Gets or sets the position.
		/// </summary>
		/// <value>The position.</value>
		public Pseudoposición Pos { get; set; }

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



		[DataMember]
		public float Puntuacion { get; set; }

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

	}
}

