using tcgct.Data.Base;
using tcgct_mtg.Models;

namespace tcgct.Data.MTG
{
    public class MTGViewCard : TableItem
    {
        public MTGViewCard(Card Card)
        {
            this.Card = Card;
        }
        public Card Card { get; set; }
    }
}
