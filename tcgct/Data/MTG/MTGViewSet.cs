using tcgct.Data.Base;
using tcgct_mtg.Models;

namespace tcgct.Data.MTG
{
    public class MTGViewSet : TableItem
    {
        public MTGViewSet(Set Set)
        {
            this.Set = Set;
        }
        
        public Set Set { get; set; }
    }
}
