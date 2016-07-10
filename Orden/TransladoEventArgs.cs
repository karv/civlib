using Graficas.Continuo;
using Civ.Global;
using System;
using Civ.Topología;
using Civ.ObjetosEstado;

namespace Civ.Orden
{
	/// <summary>
	/// Translado event arguments.
	/// </summary>
	[Serializable]
	public class TransladoEventArgs : EventArgs
	{
		/// <summary>
		/// La ruta de translado
		/// </summary>
		public readonly Ruta<Terreno> Ruta;

		/// <summary>
		/// Devuelve el destino
		/// </summary>
		/// <value>The final.</value>
		public Punto<Terreno> Final
		{
			get
			{
				return Ruta.NodoFinal;
			}
		}

		/// <param name="ruta">Ruta.</param>
		public TransladoEventArgs (Ruta<Terreno> ruta)
		{
			Ruta = ruta;
		}
		
	}
	
}