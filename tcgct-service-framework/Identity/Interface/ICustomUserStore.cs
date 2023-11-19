using Microsoft.AspNetCore.Identity;

namespace tcgct_services_framework.Identity.Interface
{

    public interface ICustomUserStore<TIdentity> : IUserStore<TIdentity>, IUserPasswordStore<TIdentity> where TIdentity : class, ICustomIdentityUser
    {

    }
}

