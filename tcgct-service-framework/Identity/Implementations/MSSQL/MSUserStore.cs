using Microsoft.AspNetCore.Identity;
using tcgct_services_framework.Identity.Interface;

namespace tcgct_services_framework.Identity.Implementations.MSSQL
{
    public class MSUserStore : ICustomUserStore<MSIdentityUser>
    {
        private readonly MSDataAccess _dataAccess;
        private readonly IPasswordHasher<MSIdentityUser> passwordHasher;
        public MSUserStore(MSDataAccess dataAccess, IPasswordHasher<MSIdentityUser> passwordHasher)
        {
            _dataAccess = dataAccess;
            this.passwordHasher = passwordHasher;
        }

        public async Task<IdentityResult> CreateAsync(MSIdentityUser user, CancellationToken cancellationToken)
        {
            await Console.Out.WriteLineAsync("Creating");
            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.CreateUser(user);
        }

        public Task<IdentityResult> DeleteAsync(MSIdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public Task<MSIdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return _dataAccess.GetByID(Guid.Parse(userId));
        }

        public async Task<MSIdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var val = await _dataAccess.GetNameFromName(normalizedUserName);
            return val;
        }

        public Task<string> GetNormalizedUserNameAsync(MSIdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(MSIdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.ID);
        }

        public Task<string> GetUserNameAsync(MSIdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.Name);
        }

        public Task SetNormalizedUserNameAsync(MSIdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(MSIdentityUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.Name = userName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(MSIdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(MSIdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.Password = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(MSIdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(MSIdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }
    }
}
