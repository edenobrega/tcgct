using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcgct_services_framework.Attributes;
using tcgct_services_framework.Generic;
using tcgct_services_framework.MTG.Models;
using tcgct_services_framework.MTG.Services;

namespace tcgct_sql.Services
{
	public class MTGCustomSetService : IMTGCustomSetService
	{
		private readonly ConfigService configService;
		public MTGCustomSetService(ConfigService configService)
		{
			this.configService = configService;
		}

		public int CreateSet(string Name, string Description, Guid Owner, int CollectedTarget)
		{
            using (var conn = new SqlConnection(configService.ConnectionString))
			{
				string sql = @"insert into [MTG].[CustomSet]([Name], [Description], [Owner], [CollectedTarget])
							   output inserted.Id
							   values (@Name, @Description, @Owner, @CollectedTarget)";
				var result = conn.QuerySingle<int>(sql, new { Name, Description, Owner, CollectedTarget });
				return result;
			}
		}

		public void DeleteSet(int ID)
		{
			using (var conn = new SqlConnection(configService.ConnectionString))
			{
				string sql = "delete from mtg.CustomSet where ID = @ID";
				 conn.Execute(sql, new { ID });
			}
		}

		public void UpdateCards(int ID, IEnumerable<int> CardIDs)
		{
			using (var conn = new SqlConnection(configService.ConnectionString))
			{
				string sql = "select SetID, CardID from [MTG].[CustomSetCard] where SetID = @SetID";
				var csc = conn.Query<CustomSetCard>(sql, new { ID });
				Stack<int> remove = new Stack<int>();
				Stack<int> add = new Stack<int>();
				foreach (var item in csc)
				{
					if (CardIDs.Any(a => a == item.CardID))
					{
						remove.Push(item.CardID);
						continue;
					}
					add.Push(item.CardID);
				}
				sql = "delete from [MTG].[CustomSetCard] where SetID = @SetID and CardID in @CardIDs";
				conn.Execute(sql, new { SetID = ID,CardIDs });
				sql = "insert into [MTG].[CustomSetCard] values (@SetID, @CardID)";
				conn.Execute(sql, add);
			}
		}

		public async Task<IEnumerable<Card>> GetCards(int ID, Guid Owner)
		{
			using (var conn = new SqlConnection(configService.ConnectionString))
			{
				string sql = @"select c.*, co.[Count] as [Collected]
						from mtg.Card as c
						join mtg.CustomSetCard as csc on csc.CardID = c.ID
						left join mtg.Collection as co on co.CardID = csc.CardID and co.UserID = @UserID
						where csc.SetID = @ID";

				return await conn.QueryAsync<Card>(sql, new { ID, UserID = Owner });
			}
		}

		public IEnumerable<CustomSet> GetSets(Guid Owner)
		{
			using (var conn = new SqlConnection(configService.ConnectionString))
			{
				string sql = @"SELECT * FROM [MTG].[CustomSet] where [Owner] = @Owner";
				return conn.Query<CustomSet>(sql, new { Owner });
			}
		}

        public int GetCollectionCount(int SetID)
        {
			//select cs.*
			//,(select count(1)
			//from mtg.CustomSetCard as csc
			//left join mtg.Collection as co on co.CardID = csc.CardID
			//where csc.SetID = cs.Id and co.[Count] >= (select top(1) CollectedTarget from mtg.CustomSet where Id = cs.Id)) as [CollectedCards]
			//,(select count(1) from mtg.CustomSetCard as csc where csc.SetID = cs.Id ) as [TotalCards]
			//from mtg.CustomSet as cs
			//where cs.Id in @SetIDs
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                string sql = @"select count(1)
							  from mtg.CustomSetCard as csc
							  left join mtg.Collection as co on co.CardID = csc.CardID
							  where csc.SetID = @SetID and co.[Count] >= (select top(1) CollectedTarget from mtg.CustomSet where Id = @SetID)";
                return conn.QuerySingle<int>(sql, new { SetID });
            }
        }
    }
}
