using tcgct_services_framework.Attributes;

namespace tcgct_services_framework.MTG.Models
{
    public class Card
    {
        [DbID]
        public int ID { get; set; }
        [CardName]
        public string Name { get; set; }
        public string ManaCost { get; set; }
        public string Text { get; set; }
        public string Flavor { get; set; }
        public string Artist { get; set; }
        [CardID]
        public string Collector_Number { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
        public int Card_Set_ID { get; set; }
        public Set Set { get; set; }
        public string Source_ID { get; set; }
        public float ConvertedCost { get; set; }
        public string Image { get; set; }
        public string ImageFlipped { get; set; }
        public string Oracle_ID { get; set; }
        public int Rarity_ID { get; set; }
        public Rarity Rarity { get; set; }
        public bool MultiFace { get; set; }
        public CardFace[] Faces { get; set; }
        public CardTypeLine TypeLine { get; set; }
        public int Collected { get; set; }
    }
}
