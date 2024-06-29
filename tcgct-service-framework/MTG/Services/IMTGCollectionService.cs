using tcgct_services_framework.Generic;
using tcgct_services_framework.MTG.Models;

namespace tcgct_services_framework.MTG.Services
{
    public interface IMTGCollectionService
    {
        Task<IEnumerable<Set>> GetSetsCollectedAsync(IEnumerable<Set> Data, int UserID);
        IEnumerable<Collection> GetCollectionDynamic(IEnumerable<int> CardIDs, int UserID);
        void UpdateCollected(List<Collection> newCollection, int UserID, List<EditLog<Card>>? logs = null);
    }
}
