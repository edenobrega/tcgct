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
        public string Display { get; private set; }
        public string Visibility { get; private set; }
        public bool Filtered { get; set; }

        private bool visible;
        /// <summary>
        /// If visible on table, not css value
        /// </summary>
        public bool Visible 
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
                if (visible)
                {
                    Display = "table-row";
                    Visibility = "visible";
                }
                else
                {
                    Display = "none";
                    Visibility= "hidden";
                }
            } 
        }
    }
}
