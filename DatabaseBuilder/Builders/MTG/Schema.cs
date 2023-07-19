using Dapper;
using Microsoft.Data.SqlClient;

namespace DatabaseBuilder.Builders.MTG
{
	internal static class Schema
	{
		public static void BuildSchemas(string dir, string connectionString)
		{
			foreach (var file in Directory.GetFiles(dir+"\\Schema"))
			{
				string bsql = File.ReadAllText(file);
				Console.WriteLine(bsql);
				using (var conn = new SqlConnection(connectionString))
				{
					conn.Execute(bsql);
				}
			}
		}
	}
}
