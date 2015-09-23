using System;

namespace Civ
{
	/// <summary>
	/// Representa un comando especial para un stack.
	/// pe Colonizar, Ir
	/// </summary>
	public struct ComandoEspecial
	{
		/// <summary>
		/// Es posible realizar esta acción ahora
		/// </summary>
		public bool Habilitado;

		/// <summary>
		/// nombre del comando
		/// </summary>
		public string Nombre;

		/// <summary>
		/// Realiza el comando para un Stack dado.
		/// </summary>
		public Action<Stack> Ejecutar;
	}
}

