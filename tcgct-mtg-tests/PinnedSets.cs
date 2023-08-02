using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcgct_mtg.Models;

namespace tcgct_mtg_tests
{
	[TestClass]
	public class PinnedSets
	{
		[TestMethod]
		public void GetUserPinnedSets()
		{
			var actual = Initialize.service.GetPinnedSets(Initialize.USER_ID);
			var expected = new PinnedSet
			{
				UserID = Initialize.USER_ID,
				SetID = 1
			};
			Assert.IsTrue(actual.Any(a => a.UserID == Initialize.USER_ID && a.SetID == 1));
		}

		[TestMethod]
		public void CreatePinnedSet()
		{
			Initialize.service.CreatePinnedSet(Initialize.USER_ID, 2);
		}
	}
}
