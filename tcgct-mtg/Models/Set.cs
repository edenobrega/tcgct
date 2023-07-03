using Microsoft.Data.SqlClient;
using Dapper;
using tcgct_mtg.Models.Helpers;

namespace tcgct_mtg.Models
{
    public class Set
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Shorthand { get; set; }
        public string Icon { get; set; }
        public string Search_Uri { get; set; }
        public string Scryfall_id { get; set; }
        public int Set_Type_id { get; set; }
        public SetType Set_Type { get; set; }
        public CollectedData CollectedData { get; set; }
        public bool Pinned { get; set; }
    }
}
