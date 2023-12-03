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
    public class MTGCardService : IMTGCardService
    {
        private readonly ConfigService configService;
        private readonly IMTGSetService setService;
        public MTGCardService(ConfigService configService, IMTGSetService setService)
        {
            this.configService = configService;
            this.setService = setService;
        }

        public int CreateCard(Card card)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
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
            using (var conn = new SqlConnection(configService.ConnectionString))
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
            using (var conn = new SqlConnection(configService.ConnectionString))
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
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                var sql = $@"insert into [MTG].[CardType] output inserted.id values (@NAME)";
                return conn.QuerySingle<int>(sql, new { name });
            }
        }

        public int CreateRarity(string name)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                string sql = "insert into [MTG].[Rarity]([Name]) output INSERTED.id values (@NAME)";
                return conn.QuerySingle<int>(sql, new { name });
            }
        }

        public void CreateTypeLine(int type_id, int card_id, int order)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                var sql = $@"insert into [MTG].[TypeLine] values (@CARD_ID, @TYPE_ID, @ORDER)";
                conn.Execute(sql, new { card_id, type_id, order });
                conn.Close();
            }
        }

        public IEnumerable<CardFace> GetAllCardFaces()
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                var sql = "select * from [MTG].[CardFace]";
                return conn.Query<CardFace>(sql);
            }
        }

        public IEnumerable<CardPart> GetAllCardParts()
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                var sql = "select * from [MTG].[CardPart]";
                var results = conn.Query<CardPart>(sql);
                return results;
            }
        }

        public IEnumerable<Card> GetAllCards()
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                var sql = "select * from [MTG].[Card]";
                return conn.Query<Card>(sql);
            }
        }

        public IEnumerable<CardType> GetAllCardTypes()
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                var sql = $@"select * from [MTG].[CardType]";
                var results = conn.Query<CardType>(sql);
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
            using (var conn = new SqlConnection(configService.ConnectionString))
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

        public IEnumerable<Rarity> GetRarities()
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                var sql = $@"select [id], [name] from [MTG].[Rarity]";
                var results = conn.Query<Rarity>(sql);
                return results;
            }
        }

        public async Task<IEnumerable<Card>> GetSetCardsAsync(int id, string? user_id = null)
        {
            using (var conn = new SqlConnection(configService.ConnectionString))
            {
                conn.Open();
                var sql = @"select c.*, co.[Count] as Collected
							from mtg.Card as c
							left join mtg.Collection as co on co.CardID = c.id and co.UserID = @user_id
							where c.card_set_id = @id";
                var results = await conn.QueryAsync<Card>(sql, new { id, user_id });
                List<Rarity> rarities = GetRarities().ToList();
                Set set = setService.GetSet(id);
                results.ToList().ForEach(fe =>
                {
                    fe.Set = set;
                    fe.Rarity = rarities.Single(s => s.ID == fe.Rarity_ID);
                    fe.TypeLine = GetCardTypeLine(fe.ID);
                });
                return results;
            }
        }
    }
}
