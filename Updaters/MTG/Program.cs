﻿using MTG.Classes;
using MTG.Classes.Base;
using MTG.Classes.Response;
using Newtonsoft.Json;
using RestSharp;

using tcgct_mtg.Models;
using tcgct_mtg.Services;

namespace MTG
{
    internal class Program
    {
        static MTGService mtgservice;
        static List<Set> sets;
        static List<SetType> settypes;
        static List<Card> cards;
        static List<Rarity> rarities;
        static List<CardType> cardtypes;
        static List<tcgct_mtg.Models.CardFace> cardfaces;
        static List<tcgct_mtg.Models.CardPart> cardparts;
        // TODO: replace above with dictionary <IDENTIFYABLE INFO, OBJ> and see if its faster
        static void Main(string[] args)
        {
            mtgservice = new MTGService();
            tcgct_mtg.configuration.ConfigureConnectionString("Server=localhost\\SQLEXPRESS;Database=tcgct_test;Trusted_Connection=True;");

			List<APICard> bad_cards = new List<APICard>();
            var parsed = JsonConvert.DeserializeObject<List<APICard>>(File.ReadAllText(@"D:\Programming\tcgct-new\tcgct\Updaters\MTG\Data\bulkcards.json"));
            foreach (var item in parsed)
            {
                if(item.lang != "en")
                {
                    bad_cards.Add(item);
                }
                
            }
            bad_cards.ForEach(fe => { parsed.Remove(fe); });

            sets = mtgservice.GetAllSets().ToList();
            settypes = mtgservice.GetAllSetTypes().ToList();
            cards = mtgservice.GetAllCards().ToList();
            rarities = mtgservice.GetRarities().ToList();
            cardtypes = mtgservice.GetAllCardTypes().ToList();
            cardfaces = mtgservice.GetAllCardFaces().ToList();
            cardparts = mtgservice.GetAllCardParts().ToList();

            var all_sets = HttpHelpers.GetAllSets();
            if (all_sets.has_more)
            {
                throw new Exception("extra pages, not accounted for in loader");
            }
            foreach (var _set in all_sets.data)
            {
				int _si = -1;   // set id
				int _sti = -1;  // set type id
				if (sets.Exists(e => e.Shorthand == _set.code))
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
						Scryfall_id = _set.id,
						Set_Type_id = _sti

					};
					_si = mtgservice.CreateSet(set);
                    set.ID = _si;
					sets.Add(set);
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
						Scryfall_id = _set.id,
						Set_Type_id = _sti

					};
					_si = mtgservice.CreateSet(set);
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
                    int ct = mtgservice.CreateCardType(t);
                    cardtypes.Add(new CardType
                    {
                        ID = ct,
                        Name = t.Trim()
                    });
                }
            }));

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



            foreach (var card in parsed)
            {
                if(card.lang != "en")
                {
                    Console.WriteLine($"None english card skipped Name:{card.card_name}, ID:{card.card_id}");
                    continue;
                }
                if (cards.Exists(e => e.Scryfall_ID == card.card_id))
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
                    Collector_Number = card.collector_number,
                    Power = card.power,
                    Toughness = card.toughness,
                    Card_Set_ID = _set.ID,
                    Scryfall_ID = card.card_id,
                    ConvertedCost = card.cmc,
                    Image = card.image_uris?.normal,
                    Oracle_ID = card.oracle_id,
                    Rarity_ID = _rarity.ID,
                    Rarity = _rarity,
                    MultiFace = card.card_faces != null

                };

                int _ci = mtgservice.CreateCard(_card);
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

                        int _cfi = mtgservice.CreateCardFace(_cf);
                        _cf.ID = _cfi;
                        cardfaces.Add(_cf);
                    }
                }
            }

            // Update card parts
            foreach (var card in parsed.Where(w => w.all_parts != null).ToList())
            {
                Card? _c = null;
                foreach (var part in card.all_parts)
                {
                    if (cardparts.Exists(e => e.Card?.Scryfall_ID == card.card_id))
                    {
                        continue;
                    }

                    if(_c == null)
                    {
                        _c = cards.Single(s => s.Scryfall_ID == card.card_id);
                    }
                    tcgct_mtg.Models.CardPart cpa;
                    try
                    {
                        cpa = new tcgct_mtg.Models.CardPart
                        {
                            Object = part.@object,
                            Component = part.component,
                            Name = part.name,
                            RelatedCardID = cards.Single(s => s.Scryfall_ID == part.uri.Segments[2]).ID,
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

            // Update type lines
            foreach (var card in parsed)
            {
                if (string.IsNullOrEmpty(card.type_line))
                {
                    continue;
                }

                int _ci = cards.Single(s => s.Scryfall_ID == card.card_id).ID;
                List<TypeLine> _ts = mtgservice.GetCardTypeLine(_ci).TypeLines;
                foreach (var _type in card.type_line.Split(' '))
                {
                    if (_ts.Exists(e => e.Type.Name == _type))
                    {
                        continue;
                    }
                    int _cti = cardtypes.Single(s => s.Name == _type).ID;
                    mtgservice.CreateTypeLine(_cti, _ci);
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