using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseBuilder.Builders.MTG
{
	internal class Helpers
	{
		public class TableReference
		{
			public TableReference(string tableName, string reference)
			{
				this.TableName = tableName;
				this.Reference = reference;
			}
			public string TableName { get; set; }
			public string Reference { get; set; }
		}
		
		public static IEnumerable<int> GetAllIndexs(string str, string searchValue)
		{
			List<int> idx = new List<int>();
			while (true)
			{
				int loc = str.ToUpper().IndexOf(searchValue.ToUpper());
				if (loc != -1)
				{
					idx.Add(loc);
					loc += searchValue.Length;
					str = new string(' ', str.Length - str[loc..].Length) + str[loc..];
					continue;
				}
				break;
			}
			return idx;
		}

		public static IEnumerable<TableReference> GetReferences(string tableName, string fileText)
		{
			List<TableReference> ret = new List<TableReference>();
			var idxs = GetAllIndexs(fileText, "references").ToList();
			for (int i = 0; i < idxs.Count; i++)
			{
				if (i + 1 < idxs.Count)
				{
					ret.Add(new TableReference(tableName, fileText.Substring(idxs[i], idxs[i + 1] - idxs[i])));
					continue;
				}
				ret.Add(new TableReference(tableName, fileText.Substring(idxs[i])));
			}
			return ret;
		}
	}
}
