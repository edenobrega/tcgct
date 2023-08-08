using tcgct_services_framework.MTG.Models;

namespace tcgct_mud.Data.MTG
{
    public class CardHTML
    {
        public CardHTML(Card card)
        {
            Card = card;
            HTMLValues = new HTMLHelperProps();
        }
        public Card Card { get; set; }
        public HTMLHelperProps HTMLValues { get; set; }
    }
}
