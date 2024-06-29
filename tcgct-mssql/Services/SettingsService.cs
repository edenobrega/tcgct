using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using tcgct_services_framework.Generic;
using tcgct_services_framework.Generic.Interface;

namespace tcgct_sql.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ConfigService configService;
        public SettingsService(ConfigService config)
        {
            configService = config;
        }

		public int GetGameID(string Game)
		{
			using (var conn = new SqlConnection(configService.ConnectionString))
			{
				conn.Open();
				return conn.QuerySingle<int>("select [ID] from [TCGCT].[Games] where [Name] = @Game", new { Game });
			}
		}

        class GameIDsResult
        {
            public string Name { get; set; }
            public int ID { get; set; }
        }
		public Dictionary<string, int> GetGameIDs()
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                return conn.Query<GameIDsResult>("select [Name], [ID] from [TCGCT].[Games]").ToDictionary(
					row => row.Name,
					row => row.ID);
			}
        }
        public SettingsRow? GetSetting(string Key, int GameID, int UserID)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                string sql = @"select * from [TCGCT].[Settings] where GameID = @GameID and [UserID] = @UserID and [Key] = @Key";
                return conn.QuerySingleOrDefault<SettingsRow?>(sql, new { GameID, UserID, Key });
            }
        }

        public void UpdateSetting(SettingsRow row)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                string sql = @"update TCGCT.Settings
							   set [Value] = @Value
							   where UserID = @UserID and [Key] = @Key and [GameID] = @GameID";
                conn.Execute(sql, new { row.GameID, row.UserID, row.Key, row.Value });
            }
        }
    }
}
