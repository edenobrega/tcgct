using System.Collections.Concurrent;
using tcgct_services_framework.Identity;

namespace tcgct_mud.Data.Draft
{
	public class DraftRoom
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
        public ConcurrentBag<Guid> UserIDs { get; set; }
        public Guid DraftAdmin { get; set; }
        public DraftRoom(Guid ID, string name, Guid userName)
		{
			this.ID = ID;
			Name = name;
			DraftAdmin = userName;
			UserIDs = new ConcurrentBag<Guid>();
		}
	}
}
