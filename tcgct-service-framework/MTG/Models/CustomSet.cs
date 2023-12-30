using tcgct_services_framework.Generic.Interface;

namespace tcgct_services_framework.MTG.Models
{
    public class CustomSet
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Owner { get; set; }
        public int CollectedTarget { get; set; }
        public int CollectedCount { get; set; }
    }
}
