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

            var parsed = JsonConvert.DeserializeObject<List<APICard>>(File.ReadAllText(@"D:\Programming\tcgct-new\tcgct\Updaters\MTG\Data\bulkcards.json"));

            MTGService mtgservice = new MTGService();
            List<Set> sets = mtgservice.GetAllSets().Result.ToList();
            List<SetType> settypes = mtgservice.GetSetTypes().Result.ToList();
            List<Card> cards = mtgservice.GetAllCards().Result.ToList();
            List<Rarity> rarities = mtgservice.GetRarities().Result.ToList();
            List<CardType> cardtypes = mtgservice.GetAllCardTypes().Result.ToList();
            List<tcgct_mtg.Models.CardFace> cardfaces = mtgservice.GetAllCardFaces().Result.ToList();

            // Update sets + set types
            foreach (var _card in parsed.DistinctBy(x => x.set_uri))
            {
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

            // Update card types
            // todo: refactor
            // perhaps first join all strings together, then split, then loop through that
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

            // Update rarity
            parsed.ToList().Select(s => s.rarity).Distinct().ToList().ForEach(fe =>
            {
                if(!rarities.Exists(e => e.Name == fe))
                {
                    int ri = mtgservice.CreateRarity(fe).Result;
                    rarities.Add(new Rarity
                    {
                        ID = ri,
                        Name = fe
                    });
                }
            });

            // Update cards
            foreach (var card in parsed)
            {
                if (cards.Exists(e => e.ScryfallID == card.card_id))
                {
                    // todo: perhaps should check if any of the properties have changed then update?
                    continue;
                }

                Set _set = sets.Single(s => s.Scryfall_id == card.set_id);
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
                    CollectorNumber = card.collector_number,
                    Power = card.power,
                    Toughness = card.toughness,
                    Card_Set_ID = _set.ID,
                    ScryfallID = card.card_id,
                    ConvertedCost = card.cmc,
                    Image = card.image_uris?.normal,
                    OracleID = card.oracle_id,
                    Rarity_ID = _rarity.ID,
                    Rarity = _rarity,
                    MultiFace = card.card_faces != null

                };

                int _ci = mtgservice.CreateCard(_card).Result;
                _card.ID = _ci;
                cards.Add(_card);

                if(_card.MultiFace)
                {
                    foreach (var face in card.card_faces)
                    {
                        // todo: Some are two of the same face? maybe just drop all if something exists
                        tcgct_mtg.Models.CardFace _cf = new tcgct_mtg.Models.CardFace
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

                        int _cfi = mtgservice.CreateCardFace(_cf).Result;
                        _cf.ID = _cfi;
                        cardfaces.Add(_cf);
                    }
                }
            }

            // Update card parts
            return;
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