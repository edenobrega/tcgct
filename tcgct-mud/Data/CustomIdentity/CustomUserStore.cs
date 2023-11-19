using Microsoft.AspNetCore.Identity;

namespace tcgct_mud.Data.Identity
{
    public class CustomUserStore : IUserStore<CustomIdentityUser>, IUserPasswordStore<CustomIdentityUser>
    {
        private readonly CustomDataAccess _dataAccess;
        private readonly IPasswordHasher<CustomIdentityUser> passwordHasher;
        public CustomUserStore(CustomDataAccess dataAccess, IPasswordHasher<CustomIdentityUser> passwordHasher)
        {
            this._dataAccess = dataAccess;
            this.passwordHasher = passwordHasher;
        }


        public async Task<IdentityResult> CreateAsync(CustomIdentityUser user, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync("Creating");
            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.CreateUser(user);
        }

        public Task<IdentityResult> DeleteAsync(CustomIdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public Task<CustomIdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return _dataAccess.GetByID(Guid.Parse(userId));
        }

        public async Task<CustomIdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var val = await _dataAccess.GetNameFromName(normalizedUserName);
            return val;
        }

        public Task<string> GetNormalizedUserNameAsync(CustomIdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(CustomIdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.ID);
        }

        public Task<string> GetUserNameAsync(CustomIdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.Name);
        }

        public Task SetNormalizedUserNameAsync(CustomIdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(CustomIdentityUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.Name = userName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(CustomIdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(CustomIdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.Password = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(CustomIdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(CustomIdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }
    }
}
