using Microsoft.AspNetCore.Identity;
using tcgct_services_framework.Identity.Interface;

namespace tcgct_services_framework.Identity.Implementations.MSSQL
{
    public class MSDataAccess : ICustomDataAccess<MSIdentityUser>
    {
        public Task<IdentityResult> CreateUser(MSIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<MSIdentityUser> GetByID(Guid ID)
        {
            throw new NotImplementedException();
        }

        public Task<MSIdentityUser> GetNameFromName(string Name)
        {
            throw new NotImplementedException();
        }
    }
}
