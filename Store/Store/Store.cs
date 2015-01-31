using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization;
using System.IO;

namespace Store
{
	public static partial class Store<T>
	{
		/// <summary>
		/// Serializa un objeto a un archivo xml
		/// </summary>
		/// <param name="xml">Archivo en dónde guardar el xml</param>
		/// <param name="Data">Datos a guardar.</param>
		public static void Serialize(string xml, T Data)
		{

			var settings = new XmlWriterSettings { Indent = true, IndentChars = "\t", OmitXmlDeclaration = true };
			XmlWriter wr = XmlWriter.Create(xml, settings);
			DataContractSerializer serializer = new DataContractSerializer(typeof(T));
			serializer.WriteObject(wr, Data);
			wr.Close();
		}

		/// <summary>
		/// Deserializa un objeto
		/// </summary>
		/// <param name="xml">nombre del archivo de dónde deserealizar.</param>
		/// <returns>Devuelve un objeto con la información deserializada.</returns>
		public static T Deserialize(string xml)
		{
			XmlTextReader xr = new XmlTextReader(new FileStream(xml, FileMode.Open));
			DataContractSerializer deserializer = new DataContractSerializer(typeof(T));
			object r = deserializer.ReadObject(xr);
			xr.Close();
			return (T)r;
		}
	}
}

