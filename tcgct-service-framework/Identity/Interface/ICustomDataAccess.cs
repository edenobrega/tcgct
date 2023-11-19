using Microsoft.AspNetCore.Identity;

namespace tcgct_services_framework.Identity.Interface
{
    public interface ICustomDataAccess<TIdentity> where TIdentity : ICustomIdentityUser
    {
        Task<IdentityResult> CreateUser(TIdentity user);
        Task<TIdentity> GetByID(Guid ID);
        Task<TIdentity> GetNameFromName(string Name);
    }
}
