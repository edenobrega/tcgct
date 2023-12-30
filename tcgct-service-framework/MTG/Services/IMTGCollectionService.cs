using tcgct_services_framework.Generic;
using tcgct_services_framework.MTG.Models;

namespace tcgct_services_framework.MTG.Services
{
    public interface IMTGCollectionService
    {
        Task<IEnumerable<Set>> GetSetsCollectedAsync(IEnumerable<Set> Data, Guid UserID);
        IEnumerable<Collection> GetCollectionDynamic(IEnumerable<int> CardIDs, Guid UserID);
        void UpdateCollected(List<Collection> newCollection, Guid UserID, List<EditLog<Card>>? logs = null);
    }
}
