using System;
using System.Collections.Generic;
using ListasExtra;
using System.Xml.Serialization;


namespace Civ
{
	[Serializable]
	/// <summary>
	/// Representa un trabajo en un edificioRAW
	/// </summary>	
	public class TrabajoRAW
	{
		/// <summary>
		/// Nombre
		/// </summary>
		public string Nombre;

		public override string ToString()
		{
			return string.Format("{0} @ {1}", Nombre, Edificio);
		}

		/// <summary>
		/// Recursos consumidos por trabajador*turno (Base)
		/// </summary>
		public List<Basic.Par<string, float>> EntradaStr = new List<Basic.Par<string, float>>();

		/// <summary>
		/// Recursos consumidos por trabajador*turno (Base)
		/// </summary>
		[XmlIgnore()]
		public ListaPeso<Recurso> EntradaBase 
		{
			get
			{
				ListaPeso<Recurso> ret = new ListaPeso<Recurso>();
				foreach (var x in EntradaStr)
				{
					ret[Global.g_.Data.EncuentraRecurso(x.x)] = x.y;
				}
				return ret;
			}
		}

		/// <summary>
		/// Recursos producidos por trabajador*turno (Base)
		/// </summary>
		public List<Basic.Par<string, float>> SalidaStr = new List<Basic.Par<string, float>>();

		[XmlIgnore()]
		/// <summary>
		/// Recursos producidos por trabajador*turno (Base)
		/// </summary>
		public ListaPeso<Recurso> SalidaBase
		{
			get
			{
				ListaPeso<Recurso> ret = new ListaPeso<Recurso>();
				foreach (var x in SalidaStr)
				{
					ret[Global.g_.Data.EncuentraRecurso(x.x)] = x.y;
				}
				return ret;
			}
		}

		// Requiere
		/// <summary>
		/// IRequerimientos necesarios para construir.
		/// No se requiere listar al edificio vinculado. Su necesidad es implícita.
		/// </summary>
		public List<String> Requiere = new List<string>();

		/// <summary>
		/// Devuelve la lista de requerimientos.
		/// </summary>
		/// <value>El IRequerimiento</value> 
		public List<IRequerimiento> Reqs()
		{
			// TODO: Que funcione, debería revisar la lista de cada Edificio, Ciencias y los demás IRequerimientos
			// y convertirlos a su respectivo objeto. Devolver esa lista.
            List<IRequerimiento> ret = Basic.Covertidor<string, IRequerimiento>.ConvertirLista(
                Requiere,
                x => Global.g_.Data.EncuentraRequerimiento(x));

            return ret;
		}

		/// <summary>
		/// EdificioRAW vinculado a este trabajo.
		/// </summary>
		public string Edificio;



	}


}

