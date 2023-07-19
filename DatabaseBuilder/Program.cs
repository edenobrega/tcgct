using Microsoft.Data.SqlClient;
using Dapper;
using System.Reflection;
using DatabaseBuilder.Builders.MTG;

namespace DatabaseBuilder
{
	internal class Program
	{



		static void Main(string[] args)
		{

			string db_name = "tcgct_test";
			string connectionString = $"Server=localhost\\SQLEXPRESS;Database={db_name};Trusted_Connection=True;";
			using (var conn = new SqlConnection("Server=localhost\\SQLEXPRESS;Database=master;Trusted_Connection=True;"))
			{
				string sql = $@"select convert (smallint, req_spid) As spid
									from master.dbo.syslockinfo l, 
										master.dbo.spt_values v,
										master.dbo.spt_values x, 
										master.dbo.spt_values u, 
										master.dbo.sysdatabases d
									where   l.rsc_type = v.number 
									and v.type = 'LR' 
									and l.req_status = x.number 
									and x.type = 'LS' 
									and l.req_mode + 1 = u.number
									and u.type = 'L' 
									and l.rsc_dbid = d.dbid 
									and rsc_dbid = (select top 1 dbid from 
													master..sysdatabases 
													where name like '%{db_name}%')";

				foreach (var item in conn.Query<int>(sql))
				{
					conn.Execute($"kill {item}");
				}
				sql = $@"IF EXISTS(SELECT [Name] FROM master.sys.databases WHERE [name] = '{db_name}')
						BEGIN
							DROP DATABASE {db_name}
						END";
				conn.Execute(sql);
				conn.Execute($"create database {db_name}");
				string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
				projectDirectory += "\\database\\mtg";


			}



			//return;
			//List<string> build_order = new List<string>
			//{
			//	"Schema",
			//	"Table",
			//	"StoredProcedure",
			//	"Data"
			//};

			//string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
			//projectDirectory += "\\database\\mtg";

			//string bsql;
			//foreach (var dir in build_order)
			//{
			//             switch (dir)
			//	{
			//		case "Schema":
			//			//Schema.BuildSchemas(projectDirectory, connectionString);
			//			break;
			//		case "Table":
			//			Table.BuildTables(projectDirectory, connectionString);
			//			break;
			//		case "StoredProcedure":
			//			break;
			//		case "Data":
			//			break;
			//		default:
			//			throw new Exception("Unknown folder passed: "+dir);
			//	}
			//foreach (var item in Directory.GetFiles(projectDirectory+"\\"+dir))
			//{
			//	bsql = File.ReadAllText(item);
			//                Console.WriteLine(bsql);
			//	//using (var conn = new SqlConnection(connectionString))
			//	//{
			//	//	conn.Execute(bsql);
			//	//}
			//}
		}


			//string fc = File.ReadAllText("D:\\Programming\\tcgct-new\\tcgct\\DatabaseBuilder\\Database\\MTG\\Table\\Card.sql");
			//var idxs = GetAllIndexs(fc, "references").ToList();
			//var r = GetReferences()


			//using (var conn = new SqlConnection(connectionString))
			//{

			//}
		}
	}
}