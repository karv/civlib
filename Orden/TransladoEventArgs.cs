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
		public readonly Continuo<Terreno>.Ruta Ruta;

		/// <summary>
		/// Devuelve el destino
		/// </summary>
		/// <value>The final.</value>
		public Continuo<Terreno>.ContinuoPunto Final
		{
			get
			{
				return Ruta.NodoFinal;
			}
		}

		/// <param name="ruta">Ruta.</param>
		public TransladoEventArgs (Continuo<Terreno>.Ruta ruta)
		{
			Ruta = ruta;
		}
		
	}
	
}