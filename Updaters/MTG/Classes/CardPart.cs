using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG.Classes
{
    internal class CardPart
    {
        public string @object { get; set; }
        public string id { get; set; }
        public string component { get; set; }
        public string name { get; set; }
        public string type_line { get; set; }
        public Uri uri { get; set; }
    }
}
