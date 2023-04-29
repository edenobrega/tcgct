using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG.Classes
{
    internal class CardFace
    {
        public string @object { get; set; }
        public string name { get; set; }
        public string mana_cost { get; set; }
        public string type_line { get; set; }
        public string oracle_text { get; set; }
        public List<string> colors { get; set; }
        public string defense { get; set; }
        public string artist { get; set; }
        public string artist_id { get; set; }
        public string illustration_id { get; set; }
        public ImageURIs image_uris { get; set; }
        public decimal cmc { get; set; }
        public List<string> color_indicator { get; set; }
        public string flavor_text { get; set; }
        public string layout { get; set; }
        public string loyalty { get; set; }
        public string oracle_id { get; set; }
        public string power { get; set; }
        public string toughness { get; set; }
        public string printed_name { get; set; }
        public string printed_text { get; set; }
    }
}
