using Microsoft.AspNetCore.Identity;
using tcgct_services_framework.Identity.Interface;

namespace tcgct_services_framework.Identity.Implementations.MSSQL
{
    public class MSUserStore : ICustomUserStore
    {
        private readonly MSDataAccess _dataAccess;
        private readonly IPasswordHasher<TCGCTUser> passwordHasher;
        public MSUserStore(MSDataAccess dataAccess, IPasswordHasher<TCGCTUser> passwordHasher)
        {
            _dataAccess = dataAccess;
            this.passwordHasher = passwordHasher;
        }

        public async Task<IdentityResult> CreateAsync(TCGCTUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _dataAccess.CreateUser(user);
        }

        public Task<IdentityResult> DeleteAsync(TCGCTUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public Task<TCGCTUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return _dataAccess.GetByID(Guid.Parse(userId));
        }

        public async Task<TCGCTUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var val = await _dataAccess.GetNameFromName(normalizedUserName);
            return val;
        }

        public Task<string> GetNormalizedUserNameAsync(TCGCTUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(TCGCTUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.ID.ToString());
        }

        public Task<string> GetUserNameAsync(TCGCTUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.Name);
        }

        public Task SetNormalizedUserNameAsync(TCGCTUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(TCGCTUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.Name = userName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(TCGCTUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(TCGCTUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.Password = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(TCGCTUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.Password);
        }

        public Task<bool> HasPasswordAsync(TCGCTUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }
    }
}
