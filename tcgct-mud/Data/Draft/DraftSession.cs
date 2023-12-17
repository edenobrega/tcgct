namespace tcgct_mud.Data.Draft
{
    public class DraftSession : IDisposable
    {
        public Guid SessionID { get; private set; }
        public DraftController DraftHub { get; private set; }
        public DraftSession(DraftController dh)
        {
            Console.WriteLine("Session created");
            SessionID = Guid.NewGuid();
            DraftHub = dh;
        }

        public void Subscribe(Action action)
        {
            DraftHub.listeners.TryAdd(SessionID, action);
        }

        public void Dispose()
        {
            DraftHub.listeners.Remove(SessionID, out _);
            DraftHub.RerenderAll();
        }
    }
}
