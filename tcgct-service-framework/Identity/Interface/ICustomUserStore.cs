using Microsoft.AspNetCore.Identity;

namespace tcgct_services_framework.Identity.Interface
{

    public interface ICustomUserStore : IUserStore<TCGCTUser>, IUserPasswordStore<TCGCTUser>
    {

    }
}

