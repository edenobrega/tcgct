using MTG.Classes;
using MTG.Classes.Base;
using MTG.Classes.Response;
using Newtonsoft.Json;
using RestSharp;
using tcgct_mtg.Services;
using tcgct_services_framework.MTG.Models;
using tcgct_services_interfaces.MTG;

namespace MTG
{
    internal class Program
    {
        static IMTGService mtgservice;
        static Dictionary<string, Set> sets;
        static List<SetType> settypes;
        static Dictionary<string, Card> cards;
        static List<Rarity> rarities;
        static List<CardType> cardtypes;
        static List<tcgct_services_framework.MTG.Models.CardFace> cardfaces;
        static List<tcgct_services_framework.MTG.Models.CardPart> cardparts;
        // TODO: replace above with dictionary <IDENTIFYABLE INFO, OBJ> and see if its faster
        static void Main(string[] args)
        {
            Logger.ShouldLog = true;

            mtgservice = new MTGSqlService();
            tcgct_mtg.configuration.ConfigureConnectionString("Server=localhost\\SQLEXPRESS;Database=tcgct_load_test;Trusted_Connection=True;");
			Logger.Log("Info", "Loading cards from json");
			var parsed = JsonConvert.DeserializeObject<APICard[]>(File.ReadAllText(@"D:\Programming\tcgct-new\tcgct\Updaters\MTG\Data\default-cards-20230722091336.json"))
                    .Where(w => w.lang == "en").ToArray();

            Logger.Log("Info", "Getting existing sets");
            sets = mtgservice.GetAllSets().ToDictionary(k => k.Source_id, v => v);
			Logger.Log("Info", "Getting existing set types");
			settypes = mtgservice.GetAllSetTypes().ToList();
            Logger.Log("Info", "Getting existing cards");
			cards = mtgservice.GetAllCards().ToDictionary(k => k.Source_ID, v => v);
            Logger.Log("Info", "Getting existing rarities");
			rarities = mtgservice.GetRarities().ToList();
            Logger.Log("Info", "Getting existing card types");
			cardtypes = mtgservice.GetAllCardTypes().ToList();
            Logger.Log("Info", "Getting exisiting card faces");
			cardfaces = mtgservice.GetAllCardFaces().ToList();
            Logger.Log("Info", "Getting existing card parts");
			cardparts = mtgservice.GetAllCardParts().ToList();

			Logger.Log("Info", "Getting all sets from scryfall");
			var all_sets = HttpHelpers.GetAllSets();
            if (all_sets.has_more)
            {
                Logger.Log("Error", "Extra pages, not accounted for in loader");
                throw new Exception("extra pages, not accounted for in loader");
            }
			Logger.Log("Info", "Request successful");
			Logger.Log("Info", "Populating Sets table");
			foreach (var _set in all_sets.data)
            {
				int _si = -1;   // set id
				int _sti = -1;  // set type id
				if (sets.ContainsKey(_set.id))
                {
                    continue;
                }
				if (!settypes.Exists(x => x.Name == _set.set_type))
				{
					_sti = mtgservice.CreateSetType(_set.set_type);
					settypes.Add(new SetType { ID = _sti, Name = _set.set_type });
					Set set = new Set
					{
						Name = _set.name,
						Shorthand = _set.code,
						Icon = _set.icon_svg_uri,
						Search_Uri = _set.search_uri,
						Source_id = _set.id,
						Set_Type_id = _sti

					};
					_si = mtgservice.CreateSet(set);
                    set.ID = _si;
                    sets[set.Source_id] = set;
				}
				else
				{
					_sti = settypes.Single(x => x.Name == _set.set_type).ID;
					Set set = new Set
					{
						Name = _set.name,
						Shorthand = _set.code,
						Icon = _set.icon_svg_uri,
						Search_Uri = _set.search_uri,
						Source_id = _set.id,
						Set_Type_id = _sti

					};
					_si = mtgservice.CreateSet(set);
					set.ID = _si;
                    sets[set.Source_id] = set;
				}
			}
			
            Logger.Log("Info", "Populating Card Types table");
			// Update card types
			// todo: refactor
			// perhaps first join all strings together, then split, then loop through that
			HashSet<string> _tempCardTypes = new HashSet<string>();
			parsed.ToList().ForEach(fe =>
			{
				if (fe.type_line != null)
				{
					_tempCardTypes.UnionWith(fe.type_line.ToUpper().Split(' '));
				}
			});
            
			var missing_card_types = _tempCardTypes.Except(cardtypes.Select(s => s.Name.ToUpper()));

            foreach (var item in missing_card_types)
            {
				int ct = mtgservice.CreateCardType(item.Trim());
				cardtypes.Add(new CardType
				{
					ID = ct,
					Name = item.Trim()
				});
			}

			Logger.Log("Info", "Populating Rarity table");
			// Update rarity
			parsed.ToList().Select(s => s.rarity).Distinct().ToList().ForEach(fe =>
            {
                if(!rarities.Exists(e => e.Name == fe))
                {
                    int ri = mtgservice.CreateRarity(fe);
                    rarities.Add(new Rarity
                    {
                        ID = ri,
                        Name = fe
                    });
                }
            });

			Logger.Log("Info", "Populating Cards table");
			foreach (var card in parsed)
            {
                if(card.lang != "en")
                {
					Logger.Log("Warning", $"Non english card skipped Name:{card.card_name}, ID:{card.card_id}");
                    continue;
                }

                if (cards.ContainsKey(card.card_id))
                {
                    // todo: perhaps should check if any of the properties have changed then update?
                    continue;
                }

                //Set _set = sets.Single(s => s.Scryfall_id == card.set_id);
                Set _set = sets[card.set_id];
                Rarity _rarity = rarities.Single(r => r.Name == card.rarity);
                if(card.oracle_id == null)
                {
                    card.oracle_id = card.card_faces[0].oracle_id;
                }
                Card _card = new Card
                {
                    Name = card.card_name,
                    ManaCost = card.mana_cost,
                    Text = card.oracle_text,
                    Flavor = card.flavor_text,
                    Artist = card.artist,
                    Collector_Number = card.collector_number,
                    Power = card.power,
                    Toughness = card.toughness,
                    Card_Set_ID = _set.ID,
                    Source_ID = card.card_id,
                    ConvertedCost = card.cmc,
                    Image = card.image_uris?.normal,
                    Oracle_ID = card.oracle_id,
                    Rarity_ID = _rarity.ID,
                    Rarity = _rarity,
                    MultiFace = card.card_faces != null

                };

                int _ci = mtgservice.CreateCard(_card);
                _card.ID = _ci;
                cards[_card.Source_ID] = _card;

                if(_card.MultiFace)
                {
                    foreach (var face in card.card_faces)
                    {
						// todo: Some are two of the same face? maybe just drop all if something exists
						tcgct_services_framework.MTG.Models.CardFace _cf = new tcgct_services_framework.MTG.Models.CardFace
                        {
                            CardID = _ci,
                            Card = _card,
                            Object = face.@object,
                            Name = face.name,
                            Image = face.image_uris?.normal,
                            ManaCost = face.mana_cost,
                            OracleText = face.oracle_text,
                            ConvertedCost = Convert.ToInt32(face.cmc),
                            FlavourText = face.flavor_text,
                            Layout = face.layout,
                            Loyalty = face.loyalty,
                            OracleID = face.oracle_id,
                            Power = face.power,
                            Toughness = face.toughness
                        };

                        int _cfi = mtgservice.CreateCardFace(_cf);
                        _cf.ID = _cfi;
                        cardfaces.Add(_cf);
                    }
                }
            }
			Logger.Log("Info", "Populating Card Parts table");
			// Update card parts
			foreach (var card in parsed.Where(w => w.all_parts != null).ToList())
            {
                Card? _c = null;
                foreach (var part in card.all_parts)
                {
                    if (cardparts.Exists(e => e.Card?.Source_ID == card.card_id))
                    {
                        continue;
                    }

                    if(_c == null)
                    {
                        _c = cards[card.card_id];
                    }
					tcgct_services_framework.MTG.Models.CardPart cpa;
                    try
                    {
                        cpa = new tcgct_services_framework.MTG.Models.CardPart
                        {
                            Object = part.@object,
                            Component = part.component,
                            Name = part.name,
                            //RelatedCardID = cards.Single(s => s.Scryfall_ID == part.uri.Segments[2]).ID,
                            RelatedCardID = cards[part.uri.Segments[2]].ID,
                            CardID = _c.ID
                        };
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine($"Error: ID:{card.card_id} Name:{card.card_name}");
                        continue;
                    }


                    int _cpi = mtgservice.CreateCardPart(cpa);
                    cpa.ID = _cpi;
                    cardparts.Add(cpa);
                }
            }
			
            Logger.Log("Info", "Updating Type Line table");
            // Update type lines
			foreach (var card in parsed)
            {
                if (string.IsNullOrEmpty(card.type_line))
                {
                    continue;
                }

                int _ci = cards[card.card_id].ID;
                List<TypeLine> _ts = mtgservice.GetCardTypeLine(_ci).TypeLines;
                int _order = 1;
                var types_split = card.type_line.Split(' ');
                foreach (var _type in types_split)
                {
                    if (_ts.Exists(e => e.Type.Name == _type))
                    {
                        continue;
                    }
                    int _cti = cardtypes.Single(s => s.Name == _type.ToUpper()).ID;
                    mtgservice.CreateTypeLine(_cti, _ci, _order);
                    _order++;
                }
            }
        }
    }

    internal static class HttpHelpers
    {
        internal static SetResponse GetSet(string url)
        {
            Uri uri = new Uri(url);
            var options = new RestClientOptions($"{uri.Scheme}://{uri.Authority}");
            var client = new RestClient(options);
            var request = new RestRequest(uri.AbsoluteUri);
            // The cancellation token comes from the caller. You can still make a call without it.
            var response = client.Get(request);
            return JsonConvert.DeserializeObject<SetResponse>(response.Content);
        }

        internal static SetsResponse GetAllSets()
        {
			Uri uri = new Uri("https://api.scryfall.com/sets");
			var options = new RestClientOptions($"{uri.Scheme}://{uri.Authority}");
			var client = new RestClient(options);
			var request = new RestRequest(uri.AbsoluteUri);
			// The cancellation token comes from the caller. You can still make a call without it.
			var response = client.Get(request);
			return JsonConvert.DeserializeObject<SetsResponse>(response.Content);
		}
    }
}