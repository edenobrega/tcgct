using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcgct_services_framework.MTG.Models;

namespace tcgct_services_framework.MTG.Services
{
    public interface IMTGSetService
    {
        Set GetSet(int id);
        Task<Set> GetSetAsync(int id);
        int CreateSet(Set set);
        int CreateSetType(string name);
        IEnumerable<Set> GetAllSets();
        IEnumerable<SetType> GetAllSetTypes();
        IEnumerable<SetType> GetSetTypesByID(IEnumerable<int> ids);
        Task<IEnumerable<Set>> GetSets(IEnumerable<int> ids);
        Task CreatePinnedSetAsync(int UserID, int SetID);
        Task DeletePinnedSetAsync(int UserID, int SetID);
        Task<IEnumerable<Set>> GetAllSetsAsync();
        Task<IEnumerable<SetType>> GetAllSetTypesAsync();
        Task<IEnumerable<Set>> GetCollectingSetsAsync(int UserID);
        Task<IEnumerable<PinnedSet>> GetPinnedSetsAsync(int UserID);
        Task<IEnumerable<Set>> GetUserPinnedSetsAsync(int UserID);
    }
}
