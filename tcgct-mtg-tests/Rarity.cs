using tcgct_mtg;
using tcgct_mtg.Services;

namespace tcgct_mtg_tests
{
	[TestClass]
	public class Rarity
	{
		internal static MTGService service;
		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
			configuration.ConfigureConnectionString($"Server=localhost\\SQLEXPRESS;Database={Initialize.DB_NAME};Trusted_Connection=True;");
			service = new MTGService();
		}

		[TestMethod]
		public void GetAllRarities()
		{
			var actual = service.GetRarities().ToList();
			Assert.IsTrue(actual.Any(a => a.ID == 1 && a.Name == "Normal"));
			Assert.IsTrue(actual.Any(a => a.ID == 2 && a.Name == "Uncommon"));
		}

		[TestMethod]
		public void GetRarity()
		{
			var actual = service.GetRarity(1);
			Assert.IsTrue(actual.ID == 1 && actual.Name == "Normal");
		}

		[TestMethod]
		public void CreateRarity()
		{
			var id = service.CreateRarity("Rare");
			var actual = service.GetRarity(id);
			Assert.IsTrue(actual.ID == id && actual.Name == "Rare");
		}
	}
}