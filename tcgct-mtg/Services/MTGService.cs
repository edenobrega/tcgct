﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcgct_mtg.Models;
using Dapper;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using tcgct_mtg.Models.Helpers;

namespace tcgct_mtg.Services
{
    // investigate/todo: move away from having async and sync methods, or just fix the async methods to not hand the app when not using task.run
    public class MTGService
    {
        #region Set
        #region async
        public async Task<Set> GetSetAsync(int id)
        {
            return await Task.Run(() =>
            {
                return GetSet(id);
            });

        }
        public async Task<Set> GetSetAsync(string scryfall_id)
        {
            return await Task.Run(() => 
            {
                return GetSet(scryfall_id);
            });

        }
        public async Task<IEnumerable<Set>> GetAllSetsAsync()
        {
            return await Task.Run(() =>
            {
                return GetAllSets();
            });
        }        
        public async Task<IEnumerable<Set>> GetAllSetsAsync(string UserID)
        {
            return await Task.Run(() =>
            {
                return GetAllSets(UserID);
            });
        }
        public async Task<int> CreateSetAsync(Set set)
        {
            return await Task.Run(() =>
            {
                return CreateSet(set);
            });

        }
        public async Task<IEnumerable<Set>> GetCollectingSetsAsync(string UserID)
        {
            return await Task.Run(() => 
            {
                return GetCollectingSets(UserID);
            });
        }
		public async Task<IEnumerable<Set>> GetUserPinnedSetsAsync(string UserID)
		{
            return await Task.Run(() =>
            {
                return GetUserPinnedSets(UserID);
            });
		}

		#endregion

		#region sync
		public IEnumerable<Set> GetCollectingSets(string UserID)
        {
			using (var conn = new SqlConnection(configuration.connectionString))
			{
                IEnumerable<Set> sets;
				string sql = @"select distinct s.*
                                   from [tcgct].[MTG].[Collection] as co
                                   join [MTG].[Card] as c on c.id = co.CardID
                                   join [MTG].[Set] as s on s.id = c.card_set_id
                                   where co.UserID = @UID";
                sets = conn.Query<Set>(sql, new { UID = UserID });
                IEnumerable<SetType> setTypes = GetSetTypes();
                sets.ToList().ForEach(fe =>
                {
                    fe.Set_Type = setTypes.Single(s => s.ID == fe.Set_Type_id);
                });

                return sets;
			}
		}
        public Set GetSet(int id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"select * from [MTG].[Set] where id = @id";
                var result = conn.QuerySingle<Set>(sql, new { id });
                conn.Close();
                result.Set_Type = GetSetType(result.Set_Type_id);
				return result;
            }
        }
        public Set GetSet(string scryfall_id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"select * from [MTG].[Set] where scryfall_id = @scryfall_id";
                var result = conn.QuerySingle<Set>(sql, new { scryfall_id });
                return result;
            }
        }
        public IEnumerable<Set> GetAllSets(string? UserID = null)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();

                var sql = "select * from [MTG].[Set]";
                var results = conn.Query<Set>(sql);
                conn.Close();

                IEnumerable<SetType> setTypes = GetSetTypes();

				results.ToList().ForEach(fe =>
				{
					fe.Set_Type = setTypes.Single(s => s.ID == fe.Set_Type_id);
				});
				return results;
            }
        }
        public IEnumerable<Set> GetUserPinnedSets(string UserID)
        {
			using (var conn = new SqlConnection(configuration.connectionString))
			{
				conn.Open();
				var sql = @"select s.* 
                            from [MTG].[Set] as s
                            join mtg.PinnedSet as ps on ps.SetID = s.id
                            where UserID = @UserID";
                var results = conn.Query<Set>(sql, new{ UserID });

                var st = GetSetTypes();

                results.ToList().ForEach(fe => 
                {
                    fe.Set_Type = st.Single(s => s.ID == fe.Set_Type_id);
                });

                return results;
			}
		}
        public int CreateSet(Set set)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = "insert into [MTG].[Set]([Name], shorthand, icon, search_uri, scryfall_id, set_type_id) output inserted.id values(@NAME, @SHORTHAND, @ICON, @SEARCH_URI, @SCRYFALL_ID, @SET_TYPE_ID)";
                return conn.QuerySingle<int>(sql, new
                {
                    set.Name,
                    set.Shorthand,
                    set.Icon,
                    set.Search_Uri,
                    set.Scryfall_id,
                    set.Set_Type_id
                });
            }
        }       
        #endregion
		#endregion

        #region Pinned Sets
        #region async
        public async Task<IEnumerable<PinnedSet>> GetPinnedSetsAsync(string UserID)
        {
            return await Task.Run(() =>
            {
                return GetPinnedSets(UserID);
            });
        }
		public async Task DeletePinnedSetAsync(string UserID, int SetID)
		{
            await Task.Run(() => 
            {
                DeletePinnedSet(UserID, SetID);
            });
		}
		public async Task CreatePinnedSetAsync(string UserID, int SetID)
		{
            await Task.Run(() =>
            {
                CreatePinnedSet(UserID, SetID);
            });
		}
		#endregion

		#region sync
		public void DeletePinnedSet(string UserID, int SetID)
        {
			using (var conn = new SqlConnection(configuration.connectionString))
			{
				conn.Open();
				var sql = "delete from [MTG].[PinnedSet] where SetID = @SetID and UserID = @UserID";
				conn.Execute(sql, new { SetID, UserID });
			}
		}
		public void CreatePinnedSet(string UserID, int SetID)
		{
			using (var conn = new SqlConnection(configuration.connectionString))
			{
				conn.Open();
				var sql = "insert into [MTG].[PinnedSet] (SetID, UserID) values (@SetID, @UserID)";
                conn.Execute(sql, new { SetID, UserID });
			}
		}
		public IEnumerable<PinnedSet> GetPinnedSets(string UserID)
        {
			using (var conn = new SqlConnection(configuration.connectionString))
			{
				conn.Open();
				var sql = "select SetID from [MTG].[PinnedSet] where UserID = @UserID";
                return conn.Query<PinnedSet>(sql, new { UserID });
			}
		}
        #endregion
        #endregion

		#region Cards
		#region async
		public async Task<IEnumerable<Card>> GetAllCardsAsync()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = "select * from [MTG].[Card]";
                return await conn.QueryAsync<Card>(sql);
            }
        }
        public async Task<IEnumerable<Card>> GetSetCardsAsync(int id)
        {
            return await Task.Run(() => 
            { 
                List<Rarity> rarities = GetRarities().ToList();
                Set set = GetSet(id);
                Card[] cards;
				using (var conn = new SqlConnection(configuration.connectionString))
				{
					conn.Open();
					var sql = @"select c.[id]
	                               ,co.[Count] as [Collected]
                                  ,c.[name]
                                  ,c.[mana_cost]
                                  ,c.[text]
                                  ,c.[flavor]
                                  ,c.[artist]
                                  ,c.[collector_number]
                                  ,c.[power]
                                  ,c.[toughness]
                                  ,c.[card_set_id]
                                  ,c.[scryfall_id]
                                  ,c.[converted_cost]
                                  ,c.[image]
                                  ,c.[image_flipped]
                                  ,c.[oracle_id]
                                  ,c.[rarity_id]
                                  ,c.[multi_face] as [MultiFace]
                              from [tcgct].[MTG].[Card] as c
                              full outer join mtg.[Collection] as co on c.id = co.CardID
                              where c.card_set_id = @id";
					cards = conn.Query<Card>(sql, new { id }).ToArray();

                    int[] multiface_ids = cards.Where(s => s.MultiFace).Select(s => s.ID).ToArray();
                    sql = @"SELECT [ID]
                              ,[CardID]
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
                              ,[Toughness]
                          FROM [tcgct].[MTG].[CardFace]
                          where CardID in @multiface_ids";

                    foreach (var card in cards)
                    {
                        card.Set = set;
                        card.Rarity = rarities.Single(s => s.ID == card.Rarity_ID);
                        if (card.MultiFace)
                        {
                            card.Faces = GetCardFaces(card.ID).ToArray();
                        }
                    }           
                    
					return cards;
				}
			});
        } 
        public async Task<Card> GetCardAsync(int id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"select *
                              from [MTG].[Card]
                              where [id] = @id";
                var result = await conn.QuerySingleAsync<Card>(sql, new { id });
                conn.Close();

                result.Rarity = await this.GetRarityAsync(result.Rarity_ID);
                result.Set = await this.GetSetAsync(result.Card_Set_ID);
                result.TypeLine = await this.GetCardTypeLineAsync(id);
                return result;
            }
        }
        public async Task<int> CreateCardAsync(Card card)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
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
                                ,[scryfall_id]
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
                                ,@scryfall_id
                                ,@convertedcost
                                ,@image
                                ,@imageflipped
                                ,@oracle_id
                                ,@rarity_id
                                ,@multiface)";
                return await conn.QuerySingleAsync<int>(sql, new
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
                    card.Scryfall_ID,
                    card.ConvertedCost,
                    card.Image,
                    card.ImageFlipped,
                    card.Oracle_ID,
                    card.Rarity_ID,
                    card.MultiFace
                });
            }
        }
        #endregion

        #region sync
        public IEnumerable<Card> GetAllCards()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = "select * from [MTG].[Card]";
                return conn.Query<Card>(sql);
            }
        }
        public IEnumerable<Card> GetSetCards(int id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = "select * from [MTG].[Card] where card_set_id=@id";
                var results = conn.Query<Card>(sql, new { id }).ToList();
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
        public Card GetCard(int id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"select *
                              from [MTG].[Card]
                              where [id] = @id";
                var result = conn.QuerySingle<Card>(sql, new { id });
                conn.Close();

                result.Rarity = this.GetRarity(result.Rarity_ID);
                result.Set = this.GetSet(result.Card_Set_ID);
                result.TypeLine = this.GetCardTypeLine(id);
                return result;
            }
        }
        public int CreateCard(Card card)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
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
                                ,[scryfall_id]
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
                                ,@scryfall_id
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
                    card.Scryfall_ID,
                    card.ConvertedCost,
                    card.Image,
                    card.ImageFlipped,
                    card.Oracle_ID,
                    card.Rarity_ID,
                    card.MultiFace
                });
            }
        }
        #endregion
        #endregion

        #region Card Face
        #region async
        public async Task<IEnumerable<CardFace>> GetAllCardFacesAsync()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = "select * from [MTG].[CardFace]";
                return await conn.QueryAsync<CardFace>(sql);
            }
        }
        public async Task<IEnumerable<CardFace>> GetCardFacesAsync(int id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = "select * from [MTG].[CardFace] where CardID = @ID";
                return await conn.QueryAsync<CardFace>(sql, new { ID = id });
            }
        }
        public async Task<int> CreateCardFaceAsync(CardFace card)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
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

                return await conn.QuerySingleAsync<int>(sql, new
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
        #endregion

        #region sync
        public IEnumerable<CardFace> GetAllCardFaces()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = "select * from [MTG].[CardFace]";
                return conn.Query<CardFace>(sql);
            }
        }
        public IEnumerable<CardFace> GetCardFaces(int id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = @"SELECT [ID]
                              ,[CardID]
                              ,[Object]
                              ,[Name]
                              ,[Image]
                              ,[Mana_Cost] as [ManaCost]
                              ,[Oracle_Text] as [OracleText]
                              ,[ConvertedCost]
                              ,[FlavourText]
                              ,[Layout]
                              ,[Loyalty]
                              ,[OracleID]
                              ,[Power]
                              ,[Toughness]
                          FROM [tcgct].[MTG].[CardFace] where CardID = @ID";
                return conn.Query<CardFace>(sql, new { ID = id });
            }
        }
        public int CreateCardFace(CardFace card)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
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
        #endregion
        #endregion

        #region Card Parts
        #region async
        public async Task<IEnumerable<CardPart>> GetAllCardPartsAsync()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = "select * from [MTG].[CardPart]";
                var results = await conn.QueryAsync<CardPart>(sql);
                return results;
            }
        }

        public async Task<int> CreateCardPartAsync(CardPart cardPart)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = @"INSERT INTO [MTG].[CardPart]
                                   ([CardID]
                                   ,[Object]
                                   ,[Component]
                                   ,[Name]
                                   ,[RelatedCardID])
                             OUTPUT inserted.ID
                             VALUES
                                   (@CardID,
                                   @Object,
                                   @Component,
                                   @Name,
                                   @RelatedCardID)";
                var result = await conn.QuerySingleAsync<int>(sql, new
                {
                    cardPart.CardID,
                    cardPart.Object,
                    cardPart.Component,
                    cardPart.Name,
                    cardPart.RelatedCardID
                });
                return result;
            }
        }
        #endregion

        #region sync
        public IEnumerable<CardPart> GetAllCardParts()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = "select * from [MTG].[CardPart]";
                var results = conn.Query<CardPart>(sql);
                return results;
            }
        }

        public int CreateCardPart(CardPart cardPart)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = @"INSERT INTO [MTG].[CardPart]
                                   ([CardID]
                                   ,[Object]
                                   ,[Component]
                                   ,[Name]
                                   ,[RelatedCardID])
                             OUTPUT inserted.ID
                             VALUES
                                   (@CardID,
                                   @Object,
                                   @Component,
                                   @Name,
                                   @RelatedCardID)";
                var result = conn.QuerySingle<int>(sql, new
                {
                    cardPart.CardID,
                    cardPart.Object,
                    cardPart.Component,
                    cardPart.Name,
                    cardPart.RelatedCardID
                });
                return result;
            }
        }
        #endregion
        #endregion

        #region Rarity
        #region async
        public async Task<IEnumerable<Rarity>> GetRaritiesAsync()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"select [id], [name] from [MTG].[Rarity]";
                var results = await conn.QueryAsync<Rarity>(sql);
                conn.Close();
                return results;
            }

        }
        public async Task<Rarity> GetRarityAsync(int id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"select id, name from [MTG].[Rarity] where id = @id";
                var result = await conn.QuerySingleAsync<Rarity>(sql, new { id });
                conn.Close();
                return result;
            }
        }
        public async Task<int> CreateRarityAsync(string name)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                string sql = "insert into [MTG].[Rarity]([Name]) output INSERTED.id values (@NAME)";
                return await conn.QuerySingleAsync<int>(sql, new { name });
            }
        }
        #endregion

        #region sync
        public  IEnumerable<Rarity> GetRarities()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"select [id], [name] from [MTG].[Rarity]";
                var results = conn.Query<Rarity>(sql);
                return results;
            }

        }
        public Rarity GetRarity(int id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"select id, name from [MTG].[Rarity] where id = @id";
                var result = conn.QuerySingle<Rarity>(sql, new { id });
                return result;
            }
        }
        public int CreateRarity(string name)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                string sql = "insert into [MTG].[Rarity]([Name]) output INSERTED.id values (@NAME)";
                return conn.QuerySingle<int>(sql, new { name });
            }
        }
        #endregion
        #endregion

        #region Set Types
        #region async
        public async Task<IEnumerable<SetType>> GetAllSetTypesAsync()
		{
            return await Task.Run(() => 
            { 
			    using (var conn = new SqlConnection(configuration.connectionString))
                {
                    conn.Open();
                    var sql = $@"select * from [MTG].[SetType]";
                    var results = conn.Query<SetType>(sql);
                    return results;
                }            
            });


        }
        public async Task<int> CreateSetTypeAsync(string name)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                string sql = "insert into [MTG].[SetType]([Name]) output INSERTED.id values (@NAME)";
                return await conn.QuerySingleAsync<int>(sql, new { name });
            }
        }
        #endregion

        #region sync
        public SetType GetSetType(int id)
        {
			using (var conn = new SqlConnection(configuration.connectionString))
			{
				conn.Open();
				var sql = $@"select * from [MTG].[SetType] where id = @id";
				var result = conn.QuerySingle<SetType>(sql, new { id });
				return result;
			}
		}
        public IEnumerable<SetType> GetSetTypes()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"select * from [MTG].[SetType]";
                var results = conn.Query<SetType>(sql);
                return results;
            }
        }
        public int CreateSetType(string name)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                string sql = "insert into [MTG].[SetType]([Name]) output INSERTED.id values (@NAME)";
                return conn.QuerySingle<int>(sql, new { name });
            }
        }
        #endregion
        #endregion

        #region CardType
        #region async
        public async Task<CardType> GetCardTypeAsync(int type_id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"select [id], [name] from [MTG].[CardType] where [id] = @type_id";
                var result = await conn.QuerySingleAsync<CardType>(sql, new { type_id });
                return result;
            }
        }

        public async Task<CardType> GetCardTypeAsync(string type_name)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"select [id], [name] from [MTG].[CardType] where [name] = @type_name";
                var result = await conn.QuerySingleAsync<CardType>(sql, new { type_name });
                return result;
            }
        }

        public async Task<IEnumerable<CardType>> GetAllCardTypesAsync()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"select * from [MTG].[CardType]";
                var results = await conn.QueryAsync<CardType>(sql);
                return results;
            }
        }

        public async Task<int> CreateCardTypeAsync(string name)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"insert into [MTG].[CardType] output inserted.id values (@NAME)";
                return await conn.QuerySingleAsync<int>(sql, new { name });
            }
        }
        #endregion

        #region sync
        public CardType GetCardType(int type_id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"select [id], [name] from [MTG].[CardType] where [id] = @type_id";
                var result = conn.QuerySingle<CardType>(sql, new { type_id });
                return result;
            }
        }

        public CardType GetCardType(string type_name)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"select [id], [name] from [MTG].[CardType] where [name] = @type_name";
                var result = conn.QuerySingle<CardType>(sql, new { type_name });
                return result;
            }
        }

        public IEnumerable<CardType> GetAllCardTypes()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"select * from [MTG].[CardType]";
                var results = conn.Query<CardType>(sql);
                return results;
            }
        }

        public int CreateCardType(string name)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"insert into [MTG].[CardType] output inserted.id values (@NAME)";
                return conn.QuerySingle<int>(sql, new { name });
            }
        }
        #endregion
        #endregion

        #region Type Line
        #region Internal Classes
        internal class TypeLineSQL
        {
            public int id { get; set; }
            public int type_id { get; set; }
            public string name { get; set; }
        }
        #endregion

        #region async
        public async void CreateTypeLineAsync(int type_id, int card_id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"insert into [MTG].[TypeLine] values (@CARD_ID, @TYPE_ID)";
                await conn.ExecuteAsync(sql,new { card_id, type_id });
                conn.Close();
            }
        }

        public async Task<CardTypeLine> GetCardTypeLineAsync(int card_id)
        {
            return await Task.Run<CardTypeLine>(() => 
            { 
                using (var conn = new SqlConnection(configuration.connectionString))
                {
                    conn.Open();
                    var sql = $@"select tl.id as [ID], tl.[type_id] as [TypeID], ct.[name] 
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
                            ID = item.id,
                            CardID = card_id,
                            TypeID = item.type_id,
                            Type = new CardType 
                            { 
                                ID = item.type_id,
                                Name = item.name 
                            }
                        });
                    }

                    return new CardTypeLine(typeLines);
                }            
            });

        }
        #endregion

        #region sync
        public void CreateTypeLine(int type_id, int card_id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"insert into [MTG].[TypeLine] values (@CARD_ID, @TYPE_ID)";
                conn.ExecuteAsync(sql, new { card_id, type_id });
                conn.Close();
            }
        }

        public CardTypeLine GetCardTypeLine(int card_id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"select tl.id as [ID], tl.[type_id] as [TypeID], ct.[name] 
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
                        ID = item.id,
                        CardID = card_id,
                        TypeID = item.type_id,
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
		#endregion
		#endregion

		#region Collected
		#region async
		public async Task<IEnumerable<Set>> PopulateSetCollectedAsync(IEnumerable<Set> Data, string UserID)
        {
            return await Task.Run(() => 
            {
                return PopulateSetCollected(Data, UserID);
            });
        }
		#endregion

		#region sync 
		public void UpdateCollected(List<Collection> newCollection, string UserID)
        {
            var oldCollection = GetCollectionDynamic(newCollection.Select(s => s.CardID), UserID).ToList();
            List<Collection> update = new List<Collection>();
            List<Collection> delete = new List<Collection>();
            List<Collection> insert = new List<Collection>();

            foreach (var item in newCollection)
            {
                if(item.Count == 0)
                {
                    delete.Add(item);
                }
                else if(oldCollection.Exists(oc => oc.CardID == item.CardID && oc.UserID == item.UserID))
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
            using (var conn = new SqlConnection(configuration.connectionString))
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
            }

        }
        public IEnumerable<Collection> GetCollectedSet(string UserID, int SetID)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                string sql = @"select col.CardID
	                                  ,s.id as SetID
	                                  ,col.[Count]
                                      ,col.UserID
                                from [tcgct].[MTG].[Collection] col
                                inner join MTG.[Card] c on c.id = col.CardID
                                inner join MTG.[Set] s on s.id = c.card_set_id
                                where col.UserID = @UserID and c.card_set_id = @SetID";
                return conn.Query<Collection>(sql, new { UserID, SetID });
            }

        }
        public IEnumerable<Collection> GetCollectionDynamic(IEnumerable<int> CardIDs, string UserID)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                string sql = @"select [CardID]
                                     ,[Count]
                                     ,[UserID]
                              from [tcgct].[MTG].[Collection]
                              where UserID = @UserID and CardID in @CardIDs";
                return conn.Query<Collection>(sql, new { UserID ,CardIDs });
            }
        }
		public IEnumerable<CollectedData> GetCollectedSetData(IEnumerable<int> SetIDs, string UserID)
        {
			using (var conn = new SqlConnection(configuration.connectionString))
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
		public IEnumerable<CollectedData> GetCollectedSetData(string UserID)
		{
			using (var conn = new SqlConnection(configuration.connectionString))
			{
				conn.Open();
                var sql = @"select s.id as [SetID],
                            (select count(1)
                            from mtg.[Collection] as co
                            join mtg.[Card] as c on co.CardID = c.id
                            where c.card_set_id = s.id and co.[Count] >= 4 and co.UserID = @UserID),
                            (select count(1) from mtg.Card as c where c.card_set_id = s.id)
                            from mtg.[Set] as s";
				return conn.Query<CollectedData>(sql, new { UserID });
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
        #endregion
		#endregion

	}
}