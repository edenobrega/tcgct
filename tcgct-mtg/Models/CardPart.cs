using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_mtg.Models
{
    public class CardPart
    {
        public string @Object { get; set; }
        public int ID { get; set; }
        public int CardID { get; set; }
        public Card Card { get; set; }
        public int RelatedCardID { get; set; }
        public Card RelatedCard { get; set; }
        public string Component { get; set; }
        public string Name { get; set; }
        public Uri Uri { get; set; }
    }
}
