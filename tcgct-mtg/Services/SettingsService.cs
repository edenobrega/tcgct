using Dapper;
using Microsoft.Data.SqlClient;
using tcgct_services_framework.Generic;
using tcgct_services_framework.Generic.Interface;

namespace tcgct_mtg.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ConfigService configService;
        public SettingsService(ConfigService config)
        {
            configService = config;
        }

        // TODO: This should get all games
        private int GetGameID()
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                return conn.QuerySingle<int>("select [ID] from [TCGCT].[Games] where [Name] = 'MTG'");
            }
        }
        public SettingsRow? GetSetting(string Key, string UserID)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                string sql = @"select * from [TCGCT].[Settings] where GameID = @GameID and [UserID] = @UserID and [Key] = @Key";
                return conn.QuerySingleOrDefault<SettingsRow?>(sql, new { GameID = GetGameID(), UserID, Key });
            }
        }
        // todo: this should create default for all
        public async Task CreateDefaultSettings(string UserID)
        {
            await Task.Run(() =>
            {
                using (var conn = new SqlConnection(configService.ConnectionString))
                {
                    conn.Open();
                    int GameID = GetGameID();
                    string sql = @"insert into [TCGCT].[Settings]([GameID], [UserID], [Key], [Value]) VALUES 
									(@GameID, @UserID, 'FilterBySetIDs', NULL),
									(@GameID, @UserID, 'FilterBySetTypes', NULL)";
                    conn.Execute(sql, new { GameID, UserID });
                }
            });
        }
        public void UpdateSetting(SettingsRow row)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                string sql = @"update TCGCT.Settings
								set [Value] = @Value
								where UserID = @UserID and [Key] = @Key and [GameID] = @GameID";
                conn.Execute(sql, new { GameID = GetGameID(), row.UserID, row.Key, row.Value });
            }
        }
    }
}
