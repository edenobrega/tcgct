using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_mtg.Models
{
    public class CardFace 
    {
        public int ID { get; set; }
        public int CardID { get; set; }
        public Card Card { get; set; }
        public string Object { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string ManaCost { get; set; }
        public string OracleText { get; set; }
        public int ConvertedCost { get; set; }
        public string FlavourText { get; set; }
        public string Layout { get; set; }
        public string Loyalty { get; set; }
        public string OracleID { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
    }
}
