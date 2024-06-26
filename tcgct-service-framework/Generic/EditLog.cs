﻿using System.Reflection;
using tcgct_services_framework.Attributes;

namespace tcgct_services_framework.Generic
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
				if (this.Card is null)
				{
					throw new Exception("EditLog::CardID, Card cannot be null");
				}

				MemberInfo? info = this.Card.GetType().GetMembers().FirstOrDefault(f => f.CustomAttributes.Any(a => a.AttributeType == typeof(CardID)))
				?? this.Card.GetType().GetMember("ID").FirstOrDefault()
				?? this.Card.GetType().GetMember("CardID").FirstOrDefault()
				?? null;

				if (info is null)
				{
					throw new Exception("EditLog::CardID, Info cannot be null");
				}

				return GetValue(info);
            } 
        }
		public string DbID
		{
			get
			{
				if (this.Card is null)
				{
					throw new Exception("EditLog::DbID, Card cannot be null");
				}
				MemberInfo? info = this.Card.GetType().GetMembers().FirstOrDefault(f => f.CustomAttributes.Any(a => a.AttributeType == typeof(DbID)))
					?? null;

				if (info is null)
				{
					throw new Exception("EditLog::DbID, info cannot be null");
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
        public int ChangeAmount { get; set; }
        public DateTime Time { get; set; }
    }
}
