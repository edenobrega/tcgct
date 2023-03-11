namespace tcgct_dotnet.Models.Collection.MTG
{
    public class MTGTypeLine
    {
        public int ID { get; set; }
        public MTGCard Card { get; set; }
        public MTGTypeText Type { get; set; }
    }
}
