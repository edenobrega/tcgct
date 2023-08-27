using Dapper;
using Microsoft.Data.SqlClient;
using tcgct_services_framework.Generic;
using tcgct_services_framework.MTG;
using tcgct_services_framework.MTG.Models;
using tcgct_services_framework.MTG.Models.Helpers;

namespace tcgct_mtg.Services
{
	public class MTGSqlService : IMTGService
	{
        public string ConnectionString { get; private set; }
        public MTGSqlService(string connectionString)
		{
			this.ConnectionString = connectionString;
        }

		#region async
		public async Task CreatePinnedSetAsync(string UserID, int SetID)
		{
			await Task.Run(() =>
			{
				CreatePinnedSet(UserID, SetID);
			});
		}
		public async Task DeletePinnedSetAsync(string UserID, int SetID)
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
		public async Task<IEnumerable<Set>> GetCollectingSetsAsync(string UserID)
		{
			return await Task.Run(() =>
			{
				return GetCollectingSets(UserID);
			});
		}
		public async Task<Set> GetSetAsync(int id)
		{
			return await Task.Run(() =>
			{
				return GetSet(id);
			});
		}
		public async Task<IEnumerable<Card>> GetSetCardsAsync(int id, string? user_id = null)
		{
			return await Task.Run(() =>
			{
				return GetSetCards(id, user_id);
			});
		}
		public async Task<IEnumerable<Set>> PopulateSetCollectedAsync(IEnumerable<Set> Data, string UserID)
		{
			return await Task.Run(() =>
			{
				return PopulateSetCollected(Data, UserID);
			});
		}
		public async Task<IEnumerable<Set>> GetUserPinnedSetsAsync(string UserID)
		{
			return await Task.Run(() =>
			{
				return GetSetsPinned(UserID);
			});
		}
		public async Task<IEnumerable<PinnedSet>> GetPinnedSetsAsync(string UserID)
		{
			return await Task.Run(() =>
			{
				return GetPinnedSets(UserID);
			});
		}
		#endregion

		#region sync
		public int CreateCard(Card card)
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var sql = @"INSERT INTO [MTG].[Card]
                                ([name]
                                ,[mana_cost]
                                ,[text]
                                ,[flavor]
                                ,[artist]
                                ,[collector_number]
                                ,[power]
                                ,[toughness]
                                ,[card_set_id]
                                ,[Source_ID]
                                ,[converted_cost]
                                ,[image]
                                ,[image_flipped]
                                ,[oracle_id]
                                ,[rarity_id]
                                ,[multi_face])
	                        OUTPUT inserted.id
                            VALUES
                                (@name
                                ,@manacost
                                ,@text
                                ,@flavor
                                ,@artist
                                ,@collector_number
                                ,@power
                                ,@toughness
                                ,@card_set_id
                                ,@Source_ID
                                ,@convertedcost
                                ,@image
                                ,@imageflipped
                                ,@oracle_id
                                ,@rarity_id
                                ,@multiface)";
				return conn.QuerySingle<int>(sql, new
				{
					card.Name,
					card.ManaCost,
					card.Text,
					card.Flavor,
					card.Artist,
					card.Collector_Number,
					card.Power,
					card.Toughness,
					card.Card_Set_ID,
					card.Source_ID,
					card.ConvertedCost,
					card.Image,
					card.ImageFlipped,
					card.Oracle_ID,
					card.Rarity_ID,
					card.MultiFace
				});
			}
		}
		public int CreateCardFace(CardFace card)
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				string sql = @"INSERT INTO [MTG].[CardFace]([CardID]
                              ,[Object]
                              ,[Name]
                              ,[Image]
                              ,[Mana_Cost]
                              ,[Oracle_Text]
                              ,[ConvertedCost]
                              ,[FlavourText]
                              ,[Layout]
                              ,[Loyalty]
                              ,[OracleID]
                              ,[Power]
                              ,[Toughness])
                        OUTPUT inserted.ID
                        VALUES(
                               @CardID
                              ,@Object
                              ,@Name
                              ,@Image
                              ,@ManaCost
                              ,@OracleText
                              ,@ConvertedCost
                              ,@FlavourText
                              ,@Layout
                              ,@Loyalty
                              ,@OracleID
                              ,@Power
                              ,@Toughness)";

				return conn.QuerySingle<int>(sql, new
				{
					card.CardID,
					card.Object,
					card.Name,
					card.Image,
					card.ManaCost,
					card.OracleText,
					card.ConvertedCost,
					card.FlavourText,
					card.Layout,
					card.Loyalty,
					card.OracleID,
					card.Power,
					card.Toughness
				});
			}
		}
		public int CreateCardPart(CardPart cardPart)
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var sql = @"INSERT INTO [MTG].[CardPart]
                                   ([CardID]
                                   ,[Object]
                                   ,[Component]
                                   ,[RelatedCardID])
                             OUTPUT inserted.ID
                             VALUES
                                   (@CardID,
                                   @Object,
                                   @Component,
                                   @RelatedCardID)";
				var result = conn.QuerySingle<int>(sql, new
				{
					cardPart.CardID,
					cardPart.Object,
					cardPart.Component,
					cardPart.RelatedCardID
				});
				return result;
			}
		}
		public int CreateCardType(string name)
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var sql = $@"insert into [MTG].[CardType] output inserted.id values (@NAME)";
				return conn.QuerySingle<int>(sql, new { name });
			}
		}
		public void CreatePinnedSet(string UserID, int SetID)
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var sql = "insert into [MTG].[PinnedSet] (SetID, UserID) values (@SetID, @UserID)";
				conn.Execute(sql, new { SetID, UserID });
			}
		}
		public int CreateRarity(string name)
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				string sql = "insert into [MTG].[Rarity]([Name]) output INSERTED.id values (@NAME)";
				return conn.QuerySingle<int>(sql, new { name });
			}
		}
		public int CreateSet(Set set)
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var sql = "insert into [MTG].[Set]([Name], shorthand, icon, search_uri, Source_ID, set_type_id) output inserted.id values(@NAME, @SHORTHAND, @ICON, @SEARCH_URI, @Source_ID, @SET_TYPE_ID)";
				return conn.QuerySingle<int>(sql, new
				{
					set.Name,
					set.Shorthand,
					set.Icon,
					set.Search_Uri,
					set.Source_id,
					set.Set_Type_id
				});
			}
		}
		public int CreateSetType(string name)
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				string sql = "insert into [MTG].[SetType]([Name]) output INSERTED.id values (@NAME)";
				return conn.QuerySingle<int>(sql, new { name });
			}
		}
		public void CreateTypeLine(int type_id, int card_id, int order)
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var sql = $@"insert into [MTG].[TypeLine] values (@CARD_ID, @TYPE_ID, @ORDER)";
				conn.Execute(sql, new { card_id, type_id, order });
				conn.Close();
			}
		}		
		public void DeletePinnedSet(string UserID, int SetID)
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var sql = "delete from [MTG].[PinnedSet] where SetID = @SetID and UserID = @UserID";
				conn.Execute(sql, new { SetID, UserID });
			}
		}
		public IEnumerable<CardFace> GetAllCardFaces()
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var sql = "select * from [MTG].[CardFace]";
				return conn.Query<CardFace>(sql);
			}
		}
		public IEnumerable<CardPart> GetAllCardParts()
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var sql = "select * from [MTG].[CardPart]";
				var results = conn.Query<CardPart>(sql);
				return results;
			}
		}
		public IEnumerable<Card> GetAllCards()
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var sql = "select * from [MTG].[Card]";
				return conn.Query<Card>(sql);
			}
		}
		public IEnumerable<CardType> GetAllCardTypes()
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var sql = $@"select * from [MTG].[CardType]";
				var results = conn.Query<CardType>(sql);
				return results;
			}
		}
		public IEnumerable<Set> GetAllSets()
		{
			using (var conn = new SqlConnection(ConnectionString))
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
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var sql = $@"select * from [MTG].[SetType]";
				var results = conn.Query<SetType>(sql);
				return results;
			}
		}
		internal class TypeLineSQL
		{
			public int id { get; set; }
			public int type_id { get; set; }
			public string name { get; set; }
			public int order { get; set; }
		}
		public CardTypeLine GetCardTypeLine(int card_id)
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var sql = $@"select tl.[type_id] as [TypeID], ct.[name], tl.[order] 
                                from [MTG].[TypeLine] as tl
                                inner join [MTG].[CardType] as ct on ct.id = tl.[type_id]
                                where card_id = @card_id";
				var results = conn.Query<TypeLineSQL>(sql, new { card_id });

				conn.Close();
				List<TypeLine> typeLines = new List<TypeLine>();
				foreach (var item in results)
				{
					typeLines.Add(new TypeLine
					{
						CardID = card_id,
						TypeID = item.type_id,
						Order = item.order,
						Type = new CardType
						{
							ID = item.type_id,
							Name = item.name
						}
					});
				}

				return new CardTypeLine(typeLines);
			}
		}
		public IEnumerable<Set> GetCollectingSets(string UserID)
		{
			using (var conn = new SqlConnection(ConnectionString))
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
		public IEnumerable<PinnedSet> GetPinnedSets(string UserID)
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var sql = "select SetID, UserID from [MTG].[PinnedSet] where UserID = @UserID";
				return conn.Query<PinnedSet>(sql, new { UserID });
			}
		}
		public IEnumerable<Rarity> GetRarities()
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var sql = $@"select [id], [name] from [MTG].[Rarity]";
				var results = conn.Query<Rarity>(sql);
				return results;
			}
		}
		public Set GetSet(int id)
		{
			using (var conn = new SqlConnection(ConnectionString))
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
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var sql = $@"select * from [MTG].[SetType] where id = @id";
				var result = conn.QuerySingle<SetType>(sql, new { id });
				return result;
			}
		}
		public IEnumerable<Card> GetSetCards(int id, string? user_id)
		{
			using (var conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				var sql = @"select c.*, co.[Count] as Collected
							from mtg.Card as c
							left join mtg.Collection as co on co.CardID = c.id and co.UserID = @user_id
							where c.card_set_id = @id";
				var results = conn.Query<Card>(sql, new { id, user_id }).ToList();
				List<Rarity> rarities = GetRarities().ToList();
				Set set = GetSet(id);
				results.ForEach(fe =>
				{
					fe.Set = set;
					fe.Rarity = rarities.Single(s => s.ID == fe.Rarity_ID);
					fe.TypeLine = GetCardTypeLine(fe.ID);
				});
				return results;
			}
		}
		public IEnumerable<Set> GetSetsPinned(string UserID)
		{
			using (var conn = new SqlConnection(ConnectionString))
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
		public IEnumerable<Set> PopulateSetCollected(IEnumerable<Set> Data, string UserID)
		{
			IEnumerable<CollectedData> csd = GetCollectedSetData(Data.Select(s => s.ID), UserID);
			csd.ToList().ForEach(fe =>
			{
				Data.Single(s => s.ID == fe.SetID).CollectedData = fe;
			});
			return Data;
		}
		public IEnumerable<CollectedData> GetCollectedSetData(IEnumerable<int> SetIDs, string UserID)
		{
			using (var conn = new SqlConnection(ConnectionString))
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

			using (var conn = new SqlConnection(ConnectionString))
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
				if(logs is not null)
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
			using (var conn = new SqlConnection(ConnectionString))
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
		#endregion
	}
}
