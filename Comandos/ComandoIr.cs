using System;
using Civ.ObjetosEstado;
using Civ.Topología;

namespace Civ.Comandos
{
	/// <summary>
	/// Representa un tipo de comando para una armada: 
	/// ir a un destino específico.
	/// </summary>
	[Serializable]
	public class ComandoIr : IComandoEspecial
	{
		/// <summary>
		/// nombre del comando
		/// </summary>
		/// <value>The nombre.</value>
		public string Nombre { get { return "Ir a"; } }

		/// <summary>
		/// Es posible realizar esta acción cuando la rapidez del Stack sea positiva.
		/// </summary>
		public bool Habilitado (Stack stack)
		{
			return stack.RAW.Velocidad > 0;
		}

		/// <summary>
		/// Realiza el comando para un Stack dado.
		/// </summary>
		public void Ejecutar (Stack stack, ArgComando arg)
		{
			var args = arg as Args;
			stack.ArmadaPerteneciente.Orden = new Civ.Orden.OrdenIrALugar (
				stack.ArmadaPerteneciente,
				args.Destino);
		}

		/// <summary>
		/// El contexto de una orden de Ir
		/// </summary>
		public class Args : ArgComando
		{
			/// <summary>
			/// Destino del translado.
			/// </summary>
			public Pseudoposición Destino;
		}
	}
}