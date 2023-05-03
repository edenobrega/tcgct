using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcgct_mtg.Models;
using Dapper;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace tcgct_mtg.Services
{
    public class MTGService
    {
        #region Set
        public async Task<Set> GetSet(int id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(configuration.connectionString);
                conn.Open();
                var sql = $@"select * from [MTG].[Set] where id = @id";
                var result = await conn.QuerySingleAsync<Set>(sql, new { id });
                conn.Close();
                return result;
            }
        }
        public async Task<Set> GetSet(string scryfall_id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(configuration.connectionString);
                conn.Open();
                var sql = $@"select * from [MTG].[Set] where scryfall_id = @scryfall_id";
                var result = await conn.QuerySingleAsync<Set>(sql, new { scryfall_id });
                conn.Close();
                return result;
            }
        }
        public async Task<IEnumerable<Set>> GetAllSets()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();

                var sql = "SELECT * FROM [MTG].[Set]";
                var results = await conn.QueryAsync<Set>(sql);
                conn.Close();

                this.GetSetTypes().Result.ToList().ForEach(fe => results.Where(r => r.Set_Type_id ==  fe.ID).ToList().ForEach(r2 => r2.Set_Type = fe));

                return results;
            }
        }
        public async Task<int> CreateSet(Set set)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = "insert into [MTG].[Set]([Name], shorthand, icon, search_uri, scryfall_id, set_type_id) output inserted.id values(@NAME, @SHORTHAND, @ICON, @SEARCH_URI, @SCRYFALL_ID, @SET_TYPE_ID)";
                return await conn.QuerySingleAsync<int>(sql, new
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

        #region Cards
        public async Task<IEnumerable<Card>> GetAllCards()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(configuration.connectionString);
                conn.Open();
                var sql = "select * from [MTG].[Card]";
                return await conn.QueryAsync<Card>(sql);
            }
        }
        public async Task<IEnumerable<Card>> GetSetCards(int id)
        {
            throw new NotImplementedException();
            List<Rarity> rarities = GetRarities().Result.ToList();
            Set set = GetSet(id).Result;
        } 
        public async Task<Card> GetCard(int id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(configuration.connectionString);
                conn.Open();
                var sql = $@"select *
                              from [MTG].[Card]
                              where [id] = @id";
                var result = await conn.QuerySingleAsync<Card>(sql, new { id });
                conn.Close();

                result.Rarity = await this.GetRarity(result.Rarity_ID);
                result.Set = await this.GetSet(result.Card_Set_ID);
                result.TypeLine = await this.GetCardTypeLine(id);
                return result;
            }
        }
        public async Task<int> CreateCard(Card card)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(configuration.connectionString);
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
                                ,@collectornumber
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
                    card.CollectorNumber,
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

        #region Card Face
        public async Task<IEnumerable<CardFace>> GetAllCardFaces()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = "select * from [MTG].[CardFace]";
                return await conn.QueryAsync<CardFace>(sql);
            }
        }
        public async Task<IEnumerable<CardFace>> GetCardFaces(int id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = "select * from [MTG].[CardFace] where CardID = @ID";
                return await conn.QueryAsync<CardFace>(sql, new { ID = id });
            }
        }
        public async Task<int> CreateCardFace(CardFace card)
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

        #region Card Parts
        public async Task<IEnumerable<CardPart>> GetAllCardParts()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = "select * from [MTG].[CardPart]";
                var results = await conn.QueryAsync<CardPart>(sql);
                return results;
            }
        }

        public async Task<int> CreateCardPart(CardPart cardPart)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = @"INSERT INTO [MTG].[CardPart]
                                   ([CardID]
                                   ,[Object]
                                   ,[Component]
                                   ,[Name])
                             OUTPUT inserted.ID
                             VALUES
                                   (@CardID,
                                   @Object,
                                   @Component,
                                   @Name)";
                var result = await conn.QuerySingleAsync<int>(sql, new
                {
                    cardPart.CardID,
                    cardPart.Object,
                    cardPart.Component,
                    cardPart.Name
                });
                return result;
            }
        }
        #endregion

        #region Rarity
        public async Task<IEnumerable<Rarity>> GetRarities()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(configuration.connectionString);
                conn.Open();
                var sql = $@"select [id], [name] from [{builder.InitialCatalog}].[MTG].[Rarity]";
                var results = await conn.QueryAsync<Rarity>(sql);
                conn.Close();
                return results;
            }

        }
        public async Task<Rarity> GetRarity(int id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(configuration.connectionString);
                conn.Open();
                var sql = $@"select id, name from [MTG].[Rarity] where id = @id";
                var result = await conn.QuerySingleAsync<Rarity>(sql, new { id });
                conn.Close();
                return result;
            }
        }
        public async Task<int> CreateRarity(string name)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                string sql = "insert into [MTG].[Rarity]([Name]) output INSERTED.id values (@NAME)";
                return await conn.QuerySingleAsync<int>(sql, new { name });
            }
        }
        #endregion

        #region Set Types
        public async Task<IEnumerable<SetType>> GetSetTypes()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(configuration.connectionString);
                conn.Open();
                var sql = $@"select * from [MTG].[SetType]";
                var results = await conn.QueryAsync<SetType>(sql);
                conn.Close();
                return results;
            }
        }
        public async Task<int> CreateSetType(string name)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                string sql = "insert into [MTG].[SetType]([Name]) output INSERTED.id values (@NAME)";
                return await conn.QuerySingleAsync<int>(sql, new { name });
            }
        }
        #endregion

        #region CardType
        public async Task<CardType> GetCardType(int type_id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(configuration.connectionString);
                conn.Open();
                var sql = $@"select [id], [name] from [MTG].[CardType] where [id] = @type_id";
                var result = await conn.QuerySingleAsync<CardType>(sql, new { type_id });
                return result;
            }
        }

        public async Task<CardType> GetCardType(string type_name)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(configuration.connectionString);
                conn.Open();
                var sql = $@"select [id], [name] from [MTG].[CardType] where [name] = @type_name";
                var result = await conn.QuerySingleAsync<CardType>(sql, new { type_name });
                return result;
            }
        }

        public async Task<IEnumerable<CardType>> GetAllCardTypes()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"select * from [MTG].[CardType]";
                var results = await conn.QueryAsync<CardType>(sql);
                return results;
            }
        }

        public async Task<int> CreateCardType(string name)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                conn.Open();
                var sql = $@"insert into [MTG].[CardType] output inserted.id values (@NAME)";
                return await conn.QuerySingleAsync<int>(sql, new { name });
            }
        }
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
        public async void CreateTypeLine(int type_id, int card_id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(configuration.connectionString);
                conn.Open();
                var sql = $@"insert into [MTG].[TypeLine] values (@CARD_ID, @TYPE_ID)";
                await conn.ExecuteAsync(sql,new { card_id, type_id });
                conn.Close();
            }
        }

        public async Task<CardTypeLine> GetCardTypeLine(int card_id)
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
    }
}
