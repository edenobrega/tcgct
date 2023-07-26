using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcgct_mtg.Services;
using tcgct_mtg;
using tcgct_mtg_tests;

//Initialize.service

namespace tcgct_mtg_tests
{
    [TestClass]
	public class Initialize
	{
		public const string DB_NAME = "tcgct_unittest_db";
		internal static MTGService service;
		[AssemblyInitialize]
		public static void TestsInitialize(TestContext testContext)
		{
			DatabaseBuilder.DatabaseBuilder.BuildTestDB(DB_NAME);
			configuration.ConfigureConnectionString($"Server=localhost\\SQLEXPRESS;Database={DB_NAME};Trusted_Connection=True;");
			service = new MTGService();
		}

		[AssemblyCleanup]
		public static void TestsCleanup()
		{
			DatabaseBuilder.DatabaseBuilder.DropTestDB(DB_NAME);
		}
	}
}
