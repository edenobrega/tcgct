namespace tcgct_dotnet.Models.Collection.MTG
{
    public class MTGCard
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ManaCost { get; set; }
        public string Flavor { get; set; }
        public string Artist { get; set; }
        public string CollectorNumber { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
        public MTGSet Set { get; set; }
        public string ScryfallID { get; set; }
        public int ConvertedCost { get; set; }
        public string Image { get; set; }
        public string ImageFlipped { get; set; }
        public string OracleID { get; set; }
        public MTGRarity Rarity { get; set; }
    }
}
