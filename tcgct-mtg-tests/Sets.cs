using tcgct_mtg;
using tcgct_mtg.Services;
using tcgct_mtg_tests;

//Initialize.service
namespace tcgct_mtg_tests
{
	[TestClass]
	public class Sets
	{
		[TestMethod]
		public void GetCollectingSets()
		{
			var data = Initialize.service.GetCollectingSets("0000000001");
			Assert.IsTrue(data.First().Name == "Default Test Set");
		}
		[TestMethod]
		public void GetSet_Int()
		{
			var data = Initialize.service.GetSet(1);
			Assert.IsTrue(data.Name == "Default Test Set");
		}
		[TestMethod]
		public void GetSet_String()
		{
			var data = Initialize.service.GetSet("00001abcdedf");
			Assert.IsTrue(data.Name == "Default Test Set");
		}
		[TestMethod]
		public void GetAllSets()
		{
			var result = Initialize.service.GetAllSets();
			Assert.IsTrue(result.Count() == 2);
			Assert.IsTrue(result.First(f => f.ID == 1).Name == "Default Test Set");
			Assert.IsTrue(result.First(f => f.ID == 2).Name == "Unit Test Set");
		}

		[TestMethod]
		public void CreateSet()
		{
			var data = Initialize.service.CreateSet(new tcgct_mtg.Models.Set
			{
				Name = "Unit Test Set",
				Shorthand = "UTS1",
				Icon = "#####################",
				Search_Uri = "https://www.google.com",
				Scryfall_id = "000002fghijklm",
				Set_Type_id = 1
			});
			var result = Initialize.service.GetSet(data);
			Assert.IsTrue(result.Name == "Unit Test Set");
		}
	}
}