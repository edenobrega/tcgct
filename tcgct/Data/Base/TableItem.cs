namespace tcgct.Data.Base
{
    public class TableItem
    {
        public TableItem()
        {
            Display = "none";
            Visibility = "hidden";
            Filtered = false;
        }
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
