using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_mtg_tests
{
    [TestClass]
	public class Initialize
	{
		public const string DB_NAME = "tcgct_unittest_db";
		[AssemblyInitialize]
		public static void TestsInitialize(TestContext testContext)
		{
			DatabaseBuilder.DatabaseBuilder.BuildTestDB(DB_NAME);
		}

		[AssemblyCleanup]
		public static void TestsCleanup()
		{
			DatabaseBuilder.DatabaseBuilder.DropTestDB(DB_NAME);
		}
	}
}
