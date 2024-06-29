using System.Collections.Concurrent;
using tcgct_services_framework.Identity;

namespace tcgct_mud.Data.Draft
{
	public class DraftRoom
	{
		public Guid ID { get; set; }
		public string Name { get; set; }
        public ConcurrentBag<int> UserIDs { get; set; }
        public int DraftAdmin { get; set; }
		private string? Password = null;
		private void initiliase(Guid ID, string name, int userID, string? password = null)
		{
			this.ID = ID;
			Name = name;
			DraftAdmin = userID;
			UserIDs = new ConcurrentBag<int>();
			if (password != null)
			{
				this.Password = password;
			}
		}
        public DraftRoom(Guid ID, string name, int userID)
		{
			this.initiliase(ID, name, userID);
		}
		public DraftRoom(Guid ID, string name, int userID, string? password)
		{
			this.initiliase(ID, name, userID, password);
		}

		public EJoinAttempt Join(int User, string? password)
		{
			if(this.Password != null && this.Password != password)
			{
				return EJoinAttempt.IncorrectPassword;
			}
			if (!UserIDs.Contains(User))
			{
				try
				{
					UserIDs.Add(User);
					return EJoinAttempt.Success;
				}
				catch
				{
					return EJoinAttempt.Failed;
				}

			}
			return EJoinAttempt.AlreadyJoined;
		}
	}
}
