using System;
using System.Collections.Generic;

namespace Civ
{
	public partial class Ciudad
	{
		//Edificios
		System.Collections.Generic.List <Edificio> _Edif = new System.Collections.Generic.List<Edificio>();
		/// <summary>
		/// Devuelve la lista de instancias de edicio de la ciudad.
		/// </summary>
		/// <value></value>
		public List <Edificio> Edificios {
			get {
				return _Edif;
			}
		}

		/// <summary>
		/// Revisa si existe una clase de edificio en esta ciudad.
		/// </summary>
		/// <param name="Edif">La clase de edificio buscada</param>
		/// <returns><c>true</c> si existe el edificio, <c>false</c> si no.</returns>
		public bool ExisteEdificio (EdificioRAW Edif)
		{
			foreach (var x in Edificios) {
				if (x.RAW == Edif)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Devuelve el edificio en la ciudad con un nombre específico.
		/// </summary>
		/// <param name="Ed">RAW del edificio.</param>
		/// <returns>La instancia de edificio en la ciudad; si no existe devuelve <c>null</c>.</returns>
		public Edificio EncuentraInstanciaEdificio(EdificioRAW Ed)
		{
			foreach (Edificio x in Edificios)
			{
				if (x.RAW == Ed)
				{
					return x;
				}
			}
			return null;
		}

		/// <summary>
		/// Devuelve el edificio en la ciudad con un nombre específico.
		/// </summary>
		/// <param name="Ed">Nombre del edificio.</param>
		/// <returns>La instancia de edificio en la ciudad; si no existe devuelve <c>null</c>.</returns>
		public Edificio EncuentraInstanciaEdificio(string Ed)
		{
			if (!Global.g_.Data.ExisteEdificio(Ed)) return null;       //Si no existe el edificio, devuelve nulo
			EdificioRAW Edif = Global.g_.Data.EncuentraEdificio(Ed);   //La clase de edificio que puede contener este trabajo.
			return EncuentraInstanciaEdificio(Edif);
		}

		/// <summary>
		/// Agrega una instancia de edicifio a la ciudad.
		/// </summary>
		/// <returns>La instancia de edificio que se agregó.</returns>
		/// <param name="Edif">RAW dek edificio a agregar.</param>
		public Edificio AgregaEdificio(EdificioRAW Edif)
		{
			Edificio ret = new Edificio (Edif, this);

			return ret;
		}

		/// <summary>
		/// Devuelve la lista de edificios contruibles por esta ciudad; los que se pueden hacer yno estpan hechos.
		/// </summary>
		/// <returns></returns>
		public List<EdificioRAW> Construibles()
		{
			List<EdificioRAW> ret = new List<EdificioRAW>();
			foreach (EdificioRAW x in Global.g_.Data.Edificios)
			{
				if (!ExisteEdificio(x) && SatisfaceReq(x.Reqs()))
				{
					ret.Add(x);
				}
			}
			return ret;
		}
		// Propiedades	//TODO: Ponerlos en otro archivo.
		System.Collections.Generic.List <Propiedad> _Prop = new System.Collections.Generic.List<Propiedad>();
		/// <summary>
		/// Devuelve la lista de Propiedades de la ciudad.
		/// </summary>
		/// <value>The propiedades.</value>
		public List <Propiedad> Propiedades {
			get {
				return _Prop;
			}
		}

		/// <summary>
		/// Revisa si existe una propiedad P en la ciudad.
		/// </summary>
		/// <returns><c>true</c>, si la propiedad existe, <c>false</c> en caso contrario.</returns>
		/// <param name="P">La propiedad.</param>
		public bool ExistePropiedad (Propiedad P)
		{
			foreach (var x in Propiedades) {
				if (x == P)
					return true;
			}
			return false;
		}

	}
}

