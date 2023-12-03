using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using tcgct_services_framework.Identity.Interface;
using System.Configuration;
using tcgct_services_framework.Generic;

namespace tcgct_services_framework.Identity.Implementations.MSSQL
{
    public class MSDataAccess : ICustomDataAccess
    {
        private readonly SqlConnection _connection;
        public MSDataAccess(ConfigService ss)
        {
            this._connection = new SqlConnection(ss.ConnectionString);
        }

        public async Task<IdentityResult> CreateUser(TCGCTUser user)
        {
            await Console.Out.WriteLineAsync("Creating in dataaccess");
            string sql = "insert into [Account].[User] (ID, [Name], Password) values (@ID, @Name, @Password)";
            int success = await _connection.ExecuteAsync(sql, new { user.ID, user.Name, user.Password });
            return success == 1 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Something went wrong." });
        }

        public async Task<TCGCTUser> GetByID(Guid ID)
        {
            string sql = "select * from [Account].[User] where ID = @ID";
            return await _connection.QuerySingleAsync<TCGCTUser>(sql, new { ID });
        }

        /// <summary>
        ///  This is to comply with aspnet identity thingy
        /// </summary>
        /// <returns></returns>
        public async Task<TCGCTUser> GetNameFromName(string Name)
        {
            string sql = "select * from [Account].[User] where [Name] = @Name";
            var v = await _connection.QuerySingleOrDefaultAsync<TCGCTUser>(sql, new { Name });
            return v;
        }
    }
}
