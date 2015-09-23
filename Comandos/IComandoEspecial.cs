namespace Civ.Comandos
{
	/// <summary>
	/// Representa un comando especial para un stack.
	/// pe Colonizar, Ir
	/// </summary>
	public interface IComandoEspecial
	{
		/// <summary>
		/// Es posible realizar esta acción ahora
		/// </summary>
		bool Habilitado (Stack stack);

		/// <summary>
		/// nombre del comando
		/// </summary>
		string Nombre { get; }

		/// <summary>
		/// Realiza el comando para un Stack dado.
		/// </summary>
		void Ejecutar (Stack stack, ArgComando arg);
	}

	/// <summary>
	/// Argumentos de un comando
	/// </summary>
	public class ArgComando
	{
	}
}

