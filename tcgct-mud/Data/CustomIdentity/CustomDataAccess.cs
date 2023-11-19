using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace tcgct_mud.Data.Identity
{
    public class CustomDataAccess
    {
        private readonly SqlConnection _connection;
        public CustomDataAccess(SqlConnection connection)
        {
            this._connection = connection;
        }

        public async Task<IdentityResult> CreateUser(CustomIdentityUser user)
        {
            await Console.Out.WriteLineAsync("Creating in dataaccess");
            string sql = "insert into [Account].[User] (ID, [Name], Password) values (@ID, @Name, @Password)";
            int success = await _connection.ExecuteAsync(sql, new { user.ID, user.Name, user.Password });
            return success == 1 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Something went fucky"});
        }

        public async Task<CustomIdentityUser> GetByID(Guid ID)
        {
            string sql = "select * from [Account].[User] where ID = @ID";
            return await _connection.QuerySingleAsync<CustomIdentityUser>(sql, new { ID });
        }

        /// <summary>
        ///  This is to comply with aspnet identity thingy
        /// </summary>
        /// <returns></returns>
        public async Task<CustomIdentityUser> GetNameFromName(string Name)
        {
            string sql = "select * from [Account].[User] where [Name] = @Name";
            var v = await _connection.QuerySingleOrDefaultAsync<CustomIdentityUser>(sql, new { Name });
            return v;
        }
    }
}
