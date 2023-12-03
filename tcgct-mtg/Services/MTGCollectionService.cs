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
    public class MTGCollectionService : IMTGCollectionService
    {
        private readonly ConfigService configService;
        public MTGCollectionService(ConfigService config)
        {
            configService = config;
        }

        public void UpdateCollected(List<Collection> newCollection, string UserID, List<EditLog<Card>>? logs = null)
        {
            // todo: turn this into a merge query
            var oldCollection = GetCollectionDynamic(newCollection.Select(s => s.CardID), UserID).ToList();
            List<Collection> update = new List<Collection>();
            List<Collection> delete = new List<Collection>();
            List<Collection> insert = new List<Collection>();

            foreach (var item in newCollection)
            {
                if (item.Count == 0)
                {
                    delete.Add(item);
                }
                else if (oldCollection.Exists(oc => oc.CardID == item.CardID && oc.UserID == item.UserID))
                {
                    var sing = oldCollection.Single(oc => oc.CardID == item.CardID && oc.UserID == item.UserID);
                    if (sing.Count != item.Count)
                    {
                        update.Add(item);
                    }
                }
                else
                {
                    insert.Add(item);
                }
            }

            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                string sql;
                foreach (var item in delete)
                {
                    sql = "delete from [MTG].[Collection] where CardID = @CardID and UserID = @UserID";
                    conn.Execute(sql, new { item.CardID, UserID });
                }

                foreach (var item in update)
                {
                    sql = "update [MTG].[Collection] set [Count] = @Count where CardID = @CardID and UserID = @UserID";
                    conn.Execute(sql, new { item.Count, item.CardID, UserID });
                }

                foreach (var item in insert)
                {
                    sql = "insert into [MTG].[Collection] values (@CardID, @UserID, @Count)";
                    conn.Execute(sql, new { item.CardID, item.UserID, item.Count });
                }
                if (logs is not null)
                {
                    foreach (var log in logs)
                    {
                        sql = "insert into [MTG].[CollectionLog] values (@Time, @Change, @CardID, @UserID)";
                        conn.Execute(sql, new
                        {
                            log.Time,
                            Change = log.ChangeAmount,
                            log.CardID,
                            UserID
                        });
                    }
                }
            }
        }
        public IEnumerable<Collection> GetCollectionDynamic(IEnumerable<int> CardIDs, string UserID)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                string sql = @"select [CardID]
                                     ,[Count]
                                     ,[UserID]
                              from [MTG].[Collection]
                              where UserID = @UserID and CardID in @CardIDs";
                return conn.Query<Collection>(sql, new { UserID, CardIDs });
            }
        }
        public IEnumerable<CollectedData> GetCollectedSetData(IEnumerable<int> SetIDs, string UserID)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                var sql = @"select s.id as [SetID],
                            (select count(1)
                            from mtg.[Collection] as co
                            join mtg.[Card] as c on co.CardID = c.id
                            where c.card_set_id = s.id and co.[Count] >= 4 and co.UserID = @UserID) as [CollectedCards],
                            (select count(1) from mtg.Card as c where c.card_set_id = s.id) as [TotalCards]
                            from mtg.[Set] as s
                            where s.id in @SetIDs";
                return conn.Query<CollectedData>(sql, new { SetIDs, UserID });
            }
        }
        public IEnumerable<Set> PopulateSetCollected(IEnumerable<Set> Data, string UserID)
        {
            IEnumerable<CollectedData> csd = GetCollectedSetData(Data.Select(s => s.ID), UserID);
            csd.ToList().ForEach(fe =>
            {
                Data.Single(s => s.ID == fe.SetID).CollectedData = fe;
            });
            return Data;
        }
        public async Task<IEnumerable<Set>> PopulateSetCollectedAsync(IEnumerable<Set> Data, string UserID)
        {
            return await Task.Run(() =>
            {
                return PopulateSetCollected(Data, UserID);
            });
        }
    }
}
