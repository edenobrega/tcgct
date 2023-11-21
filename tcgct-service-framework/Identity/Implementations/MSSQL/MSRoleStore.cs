using Microsoft.AspNetCore.Identity;

namespace tcgct_services_framework.Identity.Implementations.MSSQL
{
    // todo: implement
    public class MSRoleStore : IRoleStore<TCGCTRole>
    {
        public Task<IdentityResult> CreateAsync(TCGCTRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(TCGCTRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {

        }

        public Task<TCGCTRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<TCGCTRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(TCGCTRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(TCGCTRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(TCGCTRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(TCGCTRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(TCGCTRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(TCGCTRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
