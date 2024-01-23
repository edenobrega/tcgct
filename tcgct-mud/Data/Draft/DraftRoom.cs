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
		private string? Password = null;
		private void initiliase(Guid ID, string name, Guid userName, string? password = null)
		{
			this.ID = ID;
			Name = name;
			DraftAdmin = userName;
			UserIDs = new ConcurrentBag<Guid>();
			if (password != null)
			{
				this.Password = password;
			}
		}
        public DraftRoom(Guid ID, string name, Guid userName)
		{
			this.initiliase(ID, name, userName);
		}
		public DraftRoom(Guid ID, string name, Guid userName, string? password)
		{
			this.initiliase(ID, name, userName, password);
		}

		public EJoinAttempt Join(Guid User, string? password)
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
