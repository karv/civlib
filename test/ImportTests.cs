using NUnit.Framework;
using Civ.Data.Import;

namespace test
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

