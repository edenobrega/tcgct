using Microsoft.AspNetCore.Identity;

namespace tcgct_services_framework.Identity.Interface
{
	// todo: perhaps this doesnt need to exist?  
	public interface ICustomUserStore : IUserStore<TCGCTUser>, IUserPasswordStore<TCGCTUser>
    {

    }
}

