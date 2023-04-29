using MTG.Classes;
using MTG.Classes.Base;
using MTG.Classes.Response;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Channels;
using tcgct_mtg.Models;
using tcgct_mtg.Services;

namespace MTG
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //BulkResponse br = new BulkResponse();
            //br.Get();
            //var h = br.data.Single(x => x.type == "default_cards").download_uri;
            //string url = $"{h.Scheme}://{h.Host}";
            //var options = new RestClientOptions(url);
            //var client = new RestClient(options);
            //var request = new RestRequest(h.AbsoluteUri);
            //var response = client.Get(request);
            //var parsed = JsonConvert.DeserializeObject<List<APICard>>(response.Content);

            var parsed = JsonConvert.DeserializeObject<List<APICard>>(File.ReadAllText(@"D:\Programming\tcgct-new\tcgct\MTG\Data\bulkcards.json"));
            MTGService mtgservice = new MTGService();
            List<Set> sets = mtgservice.GetAllSets().Result.ToList();
            List<SetType> settypes = mtgservice.GetSetTypes().Result.ToList();
            List<Card> cards = mtgservice.GetAllCards().Result.ToList();
            List<Rarity> rarities = mtgservice.GetRarities().Result.ToList();
            List<CardType> cardtypes = mtgservice.GetAllCardTypes().Result.ToList();

            // Update sets + set types
            foreach (var _card in parsed.DistinctBy(x => x.set_uri))
            {
                break;
                int _si = -1;   // set id
                int _sti = -1;  // set type id
                if (sets.Exists(s => s.Name == _card.set_name))
                {
                    continue;
                }
                // Rate limit on api
                var _s = HttpHelpers.GetSet(_card.set_uri);
                Thread.Sleep(400);
                if (!settypes.Exists(x => x.Name == _s.set_type))
                {
                    _sti = mtgservice.CreateSetType(_card.set_type).Result;
                    settypes.Add(new SetType { ID = _sti, Name = _s.set_type });
                    Set set = new Set
                    {
                        Name = _s.name,
                        Shorthand = _s.code,
                        Icon = _s.icon_svg_uri,
                        Search_Uri = _s.search_uri,
                        Scryfall_id = _s.id,
                        Set_Type_id = _sti

                    };
                    _si = mtgservice.CreateSet(set).Result;
                    sets.Add(set);
                }
                else
                {
                    _sti = settypes.Single(x => x.Name == _s.set_type).ID;
                    Set set = new Set
                    {
                        Name = _s.name,
                        Shorthand = _s.code,
                        Icon = _s.icon_svg_uri,
                        Search_Uri = _s.search_uri,
                        Scryfall_id = _s.id,
                        Set_Type_id = _sti

                    };
                    _si = mtgservice.CreateSet(set).Result;
                    set.ID = _si;
                    sets.Add(set);
                }

            }

            parsed.ToList().ForEach(fe =>fe.type_line?.Split(' ').ToList().ForEach(t =>
            {
                if (!cardtypes.Exists(e => e.Name == t))
                {
                    int ct = mtgservice.CreateCardType(t).Result;
                    cardtypes.Add(new CardType
                    {
                        ID = ct,
                        Name = t
                    });
                }
            }));

            return;
            foreach (var _card in parsed)
            {
                int _si = -1;   // set id
                int _sti = -1;  // set type id
                int _ri = -1;   // rarity id
                Console.WriteLine(_card.set_name);
                Console.WriteLine();
                
                // check card exists
                if(cards.Exists(x => x.ScryfallID == _card.card_id))
                {
                    continue;
                }

                // check set exists
                if (!sets.Exists(x => x.Name == _card.set_name))
                {
                    Console.WriteLine("Set does not exist, checking if set type exists");
                    var _s = HttpHelpers.GetSet(_card.set_uri);
                    Thread.Sleep(400);
                    if(!settypes.Exists(x => x.Name == _s.set_type))
                    {
                        Console.WriteLine("Set type does not exist, adding");
                        _sti = mtgservice.CreateSetType(_card.set_type).Result;
                        settypes.Add(new SetType { ID = _sti, Name = _s.set_type });
                        Set set = new Set
                        {
                            Name = _s.name,
                            Shorthand = _s.code,
                            Icon = _s.icon_svg_uri,
                            Search_Uri = _s.search_uri,
                            Scryfall_id = _s.id,
                            Set_Type_id = _sti

                        };
                        _si = mtgservice.CreateSet(set).Result;
                        sets.Add(set);
                    }
                    else
                    {
                        _sti = settypes.Single(x => x.Name == _s.set_type).ID;
                        Set set = new Set
                        {
                            Name = _s.name,
                            Shorthand = _s.code,
                            Icon = _s.icon_svg_uri,
                            Search_Uri = _s.search_uri,
                            Scryfall_id = _s.id,
                            Set_Type_id = _sti

                        };
                        _si = mtgservice.CreateSet(set).Result;
                        set.ID = _si;
                        sets.Add(set);
                    }
                }
                else
                {
                    _si = sets.Single(x => x.Name == _card.set_name).ID;
                }
                
                // check rarity exists
                if(!rarities.Exists(x => x.Name == _card.rarity))
                {
                    Console.WriteLine("Rarity does not exist, adding");
                    _ri = mtgservice.CreateRarity(_card.rarity).Result;
                    rarities.Add(new Rarity
                    {
                        ID = _ri,
                        Name = _card.rarity
                    });
                }
                else
                {
                    _ri = rarities.Single(x => x.Name == _card.rarity).ID;
                }

                Card card = new Card
                {
                    Name = _card.card_name,
                    ManaCost = _card.mana_cost,
                    Text = _card.oracle_text,
                    Flavor = _card.flavor_text,
                    Artist = _card.artist,
                    CollectorNumber = _card.collector_number,
                    Power = _card.power,
                    Toughness = _card.toughness,
                    Card_Set_ID = _si,
                    ScryfallID = _card.card_id,
                    ConvertedCost = _card.cmc,
                    Image = _card.image_uris.normal,
                    ImageFlipped = null,
                    OracleID = _card.oracle_id,
                    Rarity_ID = _ri
                };
                int card_id = mtgservice.CreateCard(card).Result;
                card.ID = card_id;
                cards.Add(card);

                foreach (var type in _card.type_line.Split(' '))
                {
                    int _cti = -1;
                    if (!cardtypes.Exists(x => x.Name == type))
                    {
                        _cti = mtgservice.CreateCardType(type).Result;
                        cardtypes.Add(new CardType
                        {
                            ID = _cti,
                            Name = type
                        });
                    }
                    else
                    {
                        _cti = cardtypes.Single(x => x.Name == type).ID;
                    }
                    mtgservice.CreateTypeLine(_cti, card_id);
                }
            }
        }
    }

    internal static class HttpHelpers
    {
        internal static SetResponse GetSet(string url)
        {
            Console.WriteLine(url);
            Uri uri = new Uri(url);
            var options = new RestClientOptions($"{uri.Scheme}://{uri.Authority}");
            var client = new RestClient(options);
            var request = new RestRequest(uri.AbsoluteUri);
            // The cancellation token comes from the caller. You can still make a call without it.
            var response = client.Get(request);
            return JsonConvert.DeserializeObject<SetResponse>(response.Content);
        }
    }
}