using Microsoft.AspNetCore.Identity;

namespace tcgct_services_framework.Identity.Interface
{
    public interface ICustomDataAccess
    {
        Task<IdentityResult> CreateUser(TCGCTUser user);
        Task<TCGCTUser> GetByID(Guid ID);
        Task<TCGCTUser> GetNameFromName(string Name);
    }
}
