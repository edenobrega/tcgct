using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_mtg.Models
{
    public class TypeLine
    {
        public int CardID { get; set; }
        public int TypeID { get; set; }
        public int Order { get; set; }
        public CardType Type { get; set; }
    }
}
