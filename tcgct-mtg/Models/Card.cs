using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_mtg.Models
{
    public class Card
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ManaCost { get; set; }
        public string Text { get; set; }
        public string Flavor { get; set; }
        public string Artist { get; set; }
        public string CollectorNumber { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
        public int Card_Set_ID { get; set; }
        public Set Set { get; set; }
        public string ScryfallID { get; set; }
        public float ConvertedCost { get; set; }
        public string Image { get; set; }
        public string ImageFlipped { get; set; }
        public string OracleID { get; set; }
        public int Rarity_ID { get; set; }
        public Rarity Rarity { get; set; }
    }
}
