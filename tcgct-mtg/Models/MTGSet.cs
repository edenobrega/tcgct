using Microsoft.Data.SqlClient;
using Dapper;

namespace tcgct_mtg.Models
{
    public class MTGSet
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Shorthand { get; set; }
        public string Icon { get; set; }
        public string Search_Uri { get; set; }
        public MTGSetType Set_Type_id { get; set; }
    }
}
