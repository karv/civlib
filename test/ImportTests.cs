using NUnit.Framework;
using Civ.Data.Import;

namespace Test
{
	[TestFixture]
	public class ImportTests
	{
		[Test]
		public void ImportAllTest ()
		{
			ImportMachine.Importar ();
		}
	}
}