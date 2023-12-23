using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_services_framework.MTG.Models
{
    public class Collection
    {
        public int CardID { get; set; }
        public Guid UserID { get; set; }
        public int Count { get; set; }
    }
}
