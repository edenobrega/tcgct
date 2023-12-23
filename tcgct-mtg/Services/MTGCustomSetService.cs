using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public int CreateSet(string Name, Guid Owner)
		{
            using (var conn = new SqlConnection(configService.ConnectionString))
			{
				string sql = @"insert into [MTG].[CustomSet]
							   output inserted.Id
							   values (@Name, @Owner)";
				var result = conn.QuerySingle<int>(sql, new { Name, Owner });
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

		public IEnumerable<Card> GetCards(int ID)
		{
			using (var conn = new SqlConnection(configService.ConnectionString))
			{
				string sql = @"select c.*
							from mtg.Card as c
							join mtg.CustomSetCard as csc on csc.CardID = c.ID
							where csc.SetID = @ID";
				return conn.Query<Card>(sql, new { ID });
			}
		}

		public IEnumerable<CustomSet> GetSets(Guid Owner)
		{
			using (var conn = new SqlConnection(configService.ConnectionString))
			{
				string sql = @"SELECT [ID], [Name], [Owner] FROM [tcgct-dev].[MTG].[CustomSet] where [Owner] = @Owner";
				return conn.Query<CustomSet>(sql, new { Owner });
			}
		}
	}
}
