namespace tcgct_dotnet.Models.Collection.MTG
{
    public class MTGCollected
    {
        public int ID { get; set; }
        public MTGCard Card { get; set; }
        public int Foil { get; set; }
        public int Normal { get; set; }
        //public int Owner { get; set; }
    }
}
