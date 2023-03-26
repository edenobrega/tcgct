using tcgct.Data.Interface;
using tcgct_mtg.Models;

namespace tcgct.Data.MTG
{
    public class MTGViewSet : ITableItem
    {
        public MTGViewSet(MTGSet Set)
        {
            this.Set = Set;
            Visible = false;
            Display = "none";
            Visibility = "hidden";
        }

        public MTGSet Set { get; set; }
        public string Display { get; set; }
        public string Visibility { get; set; }
        public bool Filtered { get; set; }

        /// <summary>
        /// If visible on table, not css value
        /// </summary>
        public bool Visible { get; private set; }
        public void FlipVisibility()
        {
            Visible = !Visible;
            Display = Display == "none" ? "table-row" : "none";
            Visibility = Visibility == "hidden" ? "visible" : "hidden";
        }
    }
}
