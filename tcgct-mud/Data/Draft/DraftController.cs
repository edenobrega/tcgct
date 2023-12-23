using System.Collections.Concurrent;

namespace tcgct_mud.Data.Draft
{
    public class DraftController
    {
        public DraftController()
        {
            Console.WriteLine("Draft Controller being created");
            listeners = new ConcurrentDictionary<Guid, Action>();
            Rooms = new ConcurrentDictionary<Guid, DraftRoom>();
        }

        public ConcurrentDictionary<Guid, Action> listeners;
        public ConcurrentDictionary<Guid, DraftRoom> Rooms;
        public void CreateRoom(string roomName, Guid userID)
        {
            Guid newID = Guid.NewGuid();
            //Rooms.TryAdd(newID, new DraftRoom(newID, roomName, userID));
            foreach (var item in listeners)
            {
                item.Value.Invoke();
            }
        }

		public void RerenderAll()
		{
			foreach (var item in listeners)
			{
				item.Value.Invoke();
			}
		}
	}
}
