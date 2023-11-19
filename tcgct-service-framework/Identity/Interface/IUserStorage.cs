using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_services_framework.Identity.Interface
{
    public interface IUserStorage
    {
        Task<IUser> CreateUserAsync();
        Task<bool> DeleteUserAsync();
        Task<IUser> FindByIDAsync(Guid ID);
        Task<IUser> FindByUserNameAsync(string UserName);
        Task<IUser> UpdateAsync(IUser User);
    }
}
