using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_mtg.Models
{
    public class CardTypeLine
    {
        public CardTypeLine(List<TypeLine> typeLines)
        {
            TypeLines = typeLines;
        }
        
        public List<TypeLine> TypeLines { get; set; }

        public override string ToString()
        {
            string ret = string.Empty;
            foreach (var line in TypeLines)
            {
                ret += $" {line.Type.Name}";
            }
            return ret.Trim();
        }
    }
}
