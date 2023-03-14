using tcgct_mtg.Models;

namespace tcgct.Data.MTG
{
    public class ViewSet 
    {
        public ViewSet(MTGSet Set)
        {
            this.Set = Set;
            Display = "none";
            Visibility = "hidden";
        }

        public MTGSet Set { get; set; }
        public string Display { get; set; }
        public string Visibility { get; set; }

        public void FlipVisibility()
        {
            Display = Display == "none" ? "table-row" : "none";
            Visibility = Visibility == "hidden" ? "visible" : "hidden";
        }
    }
}
