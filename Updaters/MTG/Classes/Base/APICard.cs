using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcgct_mtg.Models;

namespace MTG.Classes.Base
{
    internal class APICard
    {
        public string artist { get; set; }
        public List<string> artist_ids { get; set; }
        public bool booster { get; set; }
        public string border_color { get; set; }
        public string card_back_id { get; set; }
        public int cardmarket_id { get; set; }
        public float cmc { get; set; }
        public string collector_number { get; set; }
        public List<string> color_identity { get; set; }
        public List<string> colors { get; set; }
        public bool digital { get; set; }
        public int edhrec_rank { get; set; }
        public List<string> finishes { get; set; }
        public string flavor_text { get; set; }
        public bool foil { get; set; }
        public string frame { get; set; }
        public bool full_art { get; set; }
        public List<string> games { get; set; }
        public bool highres_image { get; set; }
        [JsonProperty("id")]
        public string card_id { get; set; }
        public string illustration_id { get; set; }
        public string image_status { get; set; }
        public ImageURIs image_uris { get; set; }
        public List<string> keywords { get; set; }


        public List<CardFace> card_faces { get; set; }
        public List<CardPart> all_parts { get; set; }


        public string lang { get; set; }
        public string layout { get; set; }
        public Legalities legalities { get; set; }
        public string mana_cost { get; set; }
        public int mtgo_foil_id { get; set; }
        public int mtgo_id { get; set; }
        public List<int> multiverse_ids { get; set; }
        [JsonProperty("name")]
        public string card_name { get; set; }
        public bool nonfoil { get; set; }
        public string @object { get; set; }
        public string oracle_id { get; set; }
        public string oracle_text { get; set; }
        public bool oversized { get; set; }
        public int penny_rank { get; set; }
        public string power { get; set; }
        public Prices prices { get; set; }
        public string prints_search_uri { get; set; }
        public bool promo { get; set; }
        public string rarity { get; set; }
        public RelatedURIs related_uris { get; set; }
        public string released_at { get; set; }
        public bool reprint { get; set; }
        public bool reserved { get; set; }
        public string rulings_uri { get; set; }
        public string scryfall_set_uri { get; set; }
        public string scryfall_uri { get; set; }
        public string set { get; set; }
        public string set_id { get; set; }
        public string set_name { get; set; }
        public string set_search_uri { get; set; }
        public string set_type { get; set; }
        public string set_uri { get; set; }
        public bool story_spotlight { get; set; }
        public int tcgplayer_id { get; set; }
        public bool textless { get; set; }
        public string toughness { get; set; }
        public string type_line { get; set; }
        [JsonProperty("uri")]
        public string card_uri { get; set; }
        public string variation { get; set; }

    }
}
