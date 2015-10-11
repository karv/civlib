using NUnit.Framework;
using Civ.Data.Import;
using Civ.Data;

namespace test
{
	[TestFixture]
	public class ImportTests
	{
		[Test]
		public void RecursoImport ()
		{
			IImportable imp = new Recurso ();
		}

		[Test]
		public void ImportAllTest ()
		{
			ImportMachine.Importar ();
		}

	}
}

