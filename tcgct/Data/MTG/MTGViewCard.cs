using tcgct.Data.Base;
using tcgct_mtg.Models;

namespace tcgct.Data.MTG
{
    public class MTGViewCard : TableItem
    {
        public MTGViewCard(Card card, Collection collection)
        {
            this.Card = card;
            Collection = collection;
        }
        public Card Card { get; set; }
        public Collection Collection { get; set; }
    }
}
