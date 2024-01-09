using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcgct_services_framework.Generic;
using tcgct_services_framework.MTG.Models;
using tcgct_services_framework.MTG.Models.Helpers;
using tcgct_services_framework.MTG.Services;

namespace tcgct_sql.Services
{
    public class MTGSetService : IMTGSetService
    {
        private readonly ConfigService configService;
        public MTGSetService(ConfigService configService)
        {
            this.configService = configService;
        }

        public async Task<IEnumerable<Set>> GetSets(IEnumerable<int> ids)
        {
            return await Task.Run(() =>
            {
                using (var conn = new SqlConnection(configService.ConnectionString))
                {
                    conn.Open();
                    string sql = @"select * from MTG.[Set] where [id] in @ids";
                    return conn.Query<Set>(sql, new { ids });
                }
            });
        }
        public IEnumerable<SetType> GetSetTypesByID(IEnumerable<int> ids)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                string sql = @"select [id], [name] from [MTG].[SetType] where [id] in @SetIds";
                return conn.Query<SetType>(sql, new { SetIds = ids });
            }
        }
        public IEnumerable<Set> GetAllSets()
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();

                var sql = "select * from [MTG].[Set]";
                var results = conn.Query<Set>(sql);
                conn.Close();

                IEnumerable<SetType> setTypes = GetAllSetTypes();

                results.ToList().ForEach(fe =>
                {
                    fe.Set_Type = setTypes.Single(s => s.ID == fe.Set_Type_id);
                });
                return results;
            }
        }
        public IEnumerable<SetType> GetAllSetTypes()
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                var sql = $@"select * from [MTG].[SetType]";
                var results = conn.Query<SetType>(sql);
                return results;
            }
        }
        public IEnumerable<Set> GetCollectingSets(Guid UserID)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                IEnumerable<Set> sets;
                string sql = @"select distinct s.*
                                   from [MTG].[Collection] as co
                                   join [MTG].[Card] as c on c.id = co.CardID
                                   join [MTG].[Set] as s on s.id = c.card_set_id
                                   where co.UserID = @UID";
                sets = conn.Query<Set>(sql, new { UID = UserID });
                IEnumerable<SetType> setTypes = GetAllSetTypes();
                sets.ToList().ForEach(fe =>
                {
                    fe.Set_Type = setTypes.Single(s => s.ID == fe.Set_Type_id);
                });

                return sets;
            }
        }
        public IEnumerable<PinnedSet> GetPinnedSets(Guid UserID)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                var sql = "select SetID, UserID from [MTG].[PinnedSet] where UserID = @UserID";
                return conn.Query<PinnedSet>(sql, new { UserID });
            }
        }
        public Set GetSet(int id)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                var sql = $@"select * from [MTG].[Set] where id = @id";
                var result = conn.QuerySingle<Set>(sql, new { id });
                conn.Close();
                result.Set_Type = GetSetType(result.Set_Type_id);
                return result;
            }
        }
        public SetType GetSetType(int id)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                var sql = $@"select * from [MTG].[SetType] where id = @id";
                var result = conn.QuerySingle<SetType>(sql, new { id });
                return result;
            }
        }
        public IEnumerable<Set> GetSetsPinned(Guid UserID)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                var sql = @"select s.* 
                            from [MTG].[Set] as s
                            join mtg.PinnedSet as ps on ps.SetID = s.id
                            where UserID = @UserID";
                var results = conn.Query<Set>(sql, new { UserID });

                var st = GetAllSetTypes();

                results.ToList().ForEach(fe =>
                {
                    fe.Set_Type = st.Single(s => s.ID == fe.Set_Type_id);
                });

                return results;
            }
        }
        public async Task<Set> GetSetAsync(int id)
        {
            return await Task.Run(() =>
            {
                return GetSet(id);
            });
        }
        public async Task<IEnumerable<Set>> GetUserPinnedSetsAsync(Guid UserID)
        {
            return await Task.Run(() =>
            {
                return GetSetsPinned(UserID);
            });
        }
        public async Task<IEnumerable<PinnedSet>> GetPinnedSetsAsync(Guid UserID)
        {
            return await Task.Run(() =>
            {
                return GetPinnedSets(UserID);
            });
        }
        public void CreatePinnedSet(Guid UserID, int SetID)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                var sql = "insert into [MTG].[PinnedSet] (SetID, UserID) values (@SetID, @UserID)";
                conn.Execute(sql, new { SetID, UserID });
            }
        }
        public int CreateSet(Set set)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                var sql = "insert into [MTG].[Set]([Name], shorthand, icon, search_uri, Source_ID, set_type_id, release_date) output inserted.id values(@NAME, @SHORTHAND, @ICON, @SEARCH_URI, @Source_ID, @SET_TYPE_ID, @release_date)";
                return conn.QuerySingle<int>(sql, new
                {
                    set.Name,
                    set.Shorthand,
                    set.Icon,
                    set.Search_Uri,
                    set.Source_id,
                    set.Set_Type_id,
                    set.Release_date
                });
            }
        }
        public int CreateSetType(string name)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                string sql = "insert into [MTG].[SetType]([Name]) output INSERTED.id values (@NAME)";
                return conn.QuerySingle<int>(sql, new { name });
            }
        }
        public void DeletePinnedSet(Guid UserID, int SetID)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                var sql = "delete from [MTG].[PinnedSet] where SetID = @SetID and UserID = @UserID";
                conn.Execute(sql, new { SetID, UserID });
            }
        }
        public async Task CreatePinnedSetAsync(Guid UserID, int SetID)
        {
            await Task.Run(() =>
            {
                CreatePinnedSet(UserID, SetID);
            });
        }
        public async Task DeletePinnedSetAsync(Guid UserID, int SetID)
        {
            await Task.Run(() =>
            {
                DeletePinnedSet(UserID, SetID);
            });
        }
        public async Task<IEnumerable<Set>> GetAllSetsAsync()
        {
            return await Task.Run(() =>
            {
                return GetAllSets();
            });
        }
        public async Task<IEnumerable<SetType>> GetAllSetTypesAsync()
        {
            return await Task.Run(() =>
            {
                return GetAllSetTypes();
            });
        }
        public async Task<IEnumerable<Set>> GetCollectingSetsAsync(Guid UserID)
        {
            return await Task.Run(() =>
            {
                return GetCollectingSets(UserID);
            });
        }
    }
}
