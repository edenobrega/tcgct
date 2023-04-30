using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcgct_mtg.Models;
using Dapper;

namespace tcgct_mtg.Services
{
    public class MTGService
    {
        //using (var conn = new SqlConnection(configuration.connectionString))
        //{
        //    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(configuration.connectionString);
        //    conn.Open();
        //    var sql = $@"";
        //    var results = await conn.QueryAsync<Set>(sql);
        //    conn.Close();
        //    return results;
        //}
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
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(configuration.connectionString);
                conn.Open();
                var sql = $@"select *
                              from [MTG].[Card]
                              where card_set_id = @id";
                //var results = await conn.QueryAsync<Card, Rarity, Card>(sql, (card, rarity) => { card.Rarity = rarity; return card; });

                conn.Close();
                //return results;
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
                                ,@scryfallid
                                ,@convertedcost
                                ,@image
                                ,@imageflipped
                                ,@oracleid
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
                    card.ScryfallID,
                    card.ConvertedCost,
                    card.Image,
                    card.ImageFlipped,
                    card.OracleID,
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
                var sql = "select * from [MTG].[CardFace] where ID = @ID";
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
        public async void CreateTypeLine(int type_id, int card_id)
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(configuration.connectionString);
                conn.Open();
                var sql = $@"insert into [MTG].[TypeLine] values (@CARD_ID, @TYPE_ID)";
                var results = await conn.QueryAsync<Set>(sql,new { card_id, type_id });
                conn.Close();
            }
        }
        #endregion
    }
}
