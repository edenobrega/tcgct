using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcgct_services_framework.MTG.Models;

namespace tcgct_services_framework.MTG.Services
{
	public interface IMTGCustomSetService
	{
		int CreateSet(string Name, Guid Owner, int CollectedTarget);
		void DeleteSet(int ID);
		void UpdateCards(int ID, IEnumerable<int> CardIDs);
		IEnumerable<CustomSet> GetSets(Guid Owner);
		Task<IEnumerable<Card>> GetCards(int ID, Guid Owner);
		int GetCollectionCount(int SetID);
	}
}
