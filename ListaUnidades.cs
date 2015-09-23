using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;

namespace Civ.Data
{
	public class ListaUnidades:List<IUnidadRAW>, IXmlSerializable
	{
		
		public System.Xml.Schema.XmlSchema GetSchema ()
		{
			return null;
		}

		public void ReadXml (XmlReader reader)
		{
			reader.ReadStartElement ("Unidades");
			while (!reader.EOF)
			{
				reader.Read ();
				Type type = Type.GetType (reader.GetAttribute ("AssemblyQualifiedName"));
				//Type type = Type.GetType(reader.Name);
				var serial = new XmlSerializer (type);

				reader.ReadStartElement ("Unidad");
				var adder = serial.Deserialize (reader) as UnidadRAW;
				Add (adder);
			}
		}

		public void WriteXml (XmlWriter writer)
		{
			foreach (IUnidadRAW unid in this)
			{
				writer.WriteStartElement ("Unidad");
				writer.WriteAttributeString (
					"AssemblyQualifiedName",
					unid.GetType ().AssemblyQualifiedName);
				var xmlSerializer = new XmlSerializer (unid.GetType ());
				xmlSerializer.Serialize (writer, unid);
			}
		}
	}
}

