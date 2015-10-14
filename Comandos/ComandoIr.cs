namespace Civ.Comandos
{
	public class ComandoIr : IComandoEspecial
	{
		public string Nombre { get { return "Ir a"; } }

		public bool Habilitado (Stack stack)
		{
			return stack.RAW.Velocidad > 0;
		}

		public void Ejecutar (Stack stack, ArgComando arg)
		{
			var args = arg as Args;
			stack.ArmadaPerteneciente.Orden = new Civ.Orden.OrdenIr (
				stack.ArmadaPerteneciente,
				args.Destino);
		}

		public class Args : ArgComando
		{
			public Pseudoposición Destino;
		}
	}
}

