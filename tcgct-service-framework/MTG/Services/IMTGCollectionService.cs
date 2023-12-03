using tcgct_services_framework.Generic;
using tcgct_services_framework.MTG.Models;

namespace tcgct_services_framework.MTG.Services
{
    public interface IMTGCollectionService
    {
        Task<IEnumerable<Set>> PopulateSetCollectedAsync(IEnumerable<Set> Data, string UserID);
        void UpdateCollected(List<Collection> newCollection, string UserID, List<EditLog<Card>>? logs = null);
    }
}
