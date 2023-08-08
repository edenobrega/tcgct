using tcgct_services_framework.MTG.Models;
using tcgct_services_interfaces.MTG;

namespace tcgct_mtg.Services
{
	public class MTGSqlService : IMTGService
	{
		public int CreateCard(Card card)
		{
			throw new NotImplementedException();
		}

		public int CreateCardFace(CardFace card)
		{
			throw new NotImplementedException();
		}

		public int CreateCardPart(CardPart cardPart)
		{
			throw new NotImplementedException();
		}

		public int CreateCardType(string name)
		{
			throw new NotImplementedException();
		}

		public Task CreatePinnedSetAsync(string UserID, int SetID)
		{
			throw new NotImplementedException();
		}

		public int CreateRarity(string name)
		{
			throw new NotImplementedException();
		}

		public int CreateSet(Set set)
		{
			throw new NotImplementedException();
		}

		public int CreateSetType(string name)
		{
			throw new NotImplementedException();
		}

		public void CreateTypeLine(int type_id, int card_id, int order)
		{
			throw new NotImplementedException();
		}

		public Task DeletePinnedSetAsync(string UserID, int SetID)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<CardFace> GetAllCardFaces()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<CardPart> GetAllCardParts()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Card> GetAllCards()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<CardType> GetAllCardTypes()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Set> GetAllSets()
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Set>> GetAllSetsAsync()
		{
			throw new NotImplementedException();
		}

		public IEnumerable<SetType> GetAllSetTypes()
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<SetType>> GetAllSetTypesAsync()
		{
			throw new NotImplementedException();
		}

		public CardTypeLine GetCardTypeLine(int card_id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Set>> GetCollectingSetsAsync(string UserID)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<PinnedSet>> GetPinnedSetsAsync(string UserID)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<Rarity> GetRarities()
		{
			throw new NotImplementedException();
		}

		public Set GetSet(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Set> GetSetAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Card>> GetSetCardsAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Set>> GetUserPinnedSetsAsync(string UserID)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Set>> PopulateSetCollectedAsync(IEnumerable<Set> Data, string UserID)
		{
			throw new NotImplementedException();
		}

		public void UpdateCollected(List<Collection> newCollection, string UserID)
		{
			throw new NotImplementedException();
		}
	}
}
