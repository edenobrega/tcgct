using System.Reflection;
using tcgct_service_interfaces.Attributes;

namespace tcgct_mud.Data
{
	public class EditLog<T>
	{
		internal string GetValue(MemberInfo info)
		{
			switch (info.MemberType)
			{
				case MemberTypes.Field:
					return ((FieldInfo)info).GetValue(this.Card).ToString();
				case MemberTypes.Property:
					return ((PropertyInfo)info).GetValue(this.Card).ToString();
				default:
					throw new NotImplementedException();
			}
		}

        public T Card { get; set; }
        public string CardID 
        { 
            get 
            {
                MemberInfo? info = this.Card.GetType().GetMembers().FirstOrDefault(f => f.CustomAttributes.Any(a => a.AttributeType == typeof(CardID)))
					?? this.Card.GetType().GetMember("ID").FirstOrDefault()
					?? this.Card.GetType().GetMember("CardID").FirstOrDefault()
					?? null;

				if (info is null)
				{
					throw new Exception("");
				}

				return GetValue(info);
            } 
        }
		public string DbID
		{
			get
			{
				MemberInfo? info = this.Card.GetType().GetMembers().FirstOrDefault(f => f.CustomAttributes.Any(a => a.AttributeType == typeof(DbID)))
					?? null;

				if (info is null)
				{
					throw new Exception("");
				}

				return GetValue(info);
			}
		}
		public string CardName 
		{ 
			get 
			{
				MemberInfo? info = this.Card.GetType().GetMembers().FirstOrDefault(f => f.CustomAttributes.Any(a => a.AttributeType == typeof(CardName)))
					?? this.Card.GetType().GetMember("Name").FirstOrDefault()
					?? this.Card.GetType().GetMember("CardName").FirstOrDefault()
					?? null;

				if (info is null)
				{
					throw new Exception("");
				}

				return GetValue(info);
			} 
		}
		public bool QuickEdit { get; set; }
        public bool Increment { get; set; }
        public int ChangeAmount { get; set; }
        public TimeOnly Time { get; set; }
    }
}
