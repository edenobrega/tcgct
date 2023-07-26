using tcgct_mtg;
using tcgct_mtg.Services;

namespace tcgct_mtg_tests
{
	[TestClass]
	public class Rarity
	{
		[TestMethod]
		public void GetAllRarities()
		{
			var actual = Initialize.service.GetRarities().ToList();
			Assert.IsTrue(actual.Any(a => a.ID == 1 && a.Name == "Normal"));
			Assert.IsTrue(actual.Any(a => a.ID == 2 && a.Name == "Uncommon"));
		}

		[TestMethod]
		public void GetRarity()
		{
			var actual = Initialize.service.GetRarity(1);
			Assert.IsTrue(actual.ID == 1 && actual.Name == "Normal");
		}

		[TestMethod]
		public void CreateRarity()
		{
			var id = Initialize.service.CreateRarity("Rare");
			var actual = Initialize.service.GetRarity(id);
			Assert.IsTrue(actual.ID == id && actual.Name == "Rare");
		}
	}
}