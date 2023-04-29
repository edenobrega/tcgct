using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_mtg.Models
{
    internal class TypeLine
    {
        public int ID { get; set; }
        public int CardID { get; set; }
        public int TypeID { get; set; }
        public CardType Type { get; set; }
    }
}
