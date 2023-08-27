using tcgct_service_interfaces.Generic;
using tcgct_services_framework.MTG.Models;

namespace tcgct_services_interfaces.MTG
{
    public interface IMTGService
    {
		// Async
		Task CreatePinnedSetAsync(string UserID, int SetID);
		Task DeletePinnedSetAsync(string UserID, int SetID);
		Task<IEnumerable<Set>> GetAllSetsAsync();
		Task<IEnumerable<SetType>> GetAllSetTypesAsync();
		Task<IEnumerable<Set>> GetCollectingSetsAsync(string UserID);
		Task<IEnumerable<PinnedSet>> GetPinnedSetsAsync(string UserID);
		Task<Set> GetSetAsync(int id);
		Task<IEnumerable<Card>> GetSetCardsAsync(int id, string? user_id = null);
		Task<IEnumerable<Set>> GetUserPinnedSetsAsync(string UserID);
		Task<IEnumerable<Set>> PopulateSetCollectedAsync(IEnumerable<Set> Data, string UserID);

		// Sync
		Set GetSet(int id);
		void UpdateCollected(List<Collection> newCollection, string UserID, List<EditLog<Card>>? logs = null);
		int CreateCard(Card card);
		int CreateCardFace(CardFace card);
		int CreateCardPart(CardPart cardPart);
		int CreateCardType(string name);
		int CreateRarity(string name);
		int CreateSet(Set set);
		int CreateSetType(string name);
		void CreateTypeLine(int type_id, int card_id, int order);
		IEnumerable<CardFace> GetAllCardFaces();
		IEnumerable<CardPart> GetAllCardParts();
		IEnumerable<Card> GetAllCards();
		IEnumerable<CardType> GetAllCardTypes();
		IEnumerable<Set> GetAllSets();
		IEnumerable<SetType> GetAllSetTypes();
		CardTypeLine GetCardTypeLine(int card_id);
		IEnumerable<Rarity> GetRarities();
	}
}