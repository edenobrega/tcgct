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
				conn.Close();
			}
			using(var conn = new SqlConnection($"Server=localhost\\SQLEXPRESS;Database={db_name};Trusted_Connection=True;"))
			{
				string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
				projectDirectory += "\\database\\mtg";
				string bsql;
				// Schemas
				bsql = File.ReadAllText(projectDirectory + "\\Schema\\MTG.sql");
				conn.Execute(bsql);
				// Tables
				bsql = File.ReadAllText(projectDirectory + "\\Table\\CardType.sql");
				conn.Execute(bsql);
				bsql = File.ReadAllText(projectDirectory + "\\Table\\Collection.sql");
				conn.Execute(bsql);
				bsql = File.ReadAllText(projectDirectory + "\\Table\\PinnedSet.sql");
				conn.Execute(bsql);
				bsql = File.ReadAllText(projectDirectory + "\\Table\\Rarity.sql");
				conn.Execute(bsql);
				bsql = File.ReadAllText(projectDirectory + "\\Table\\SetType.sql");
				conn.Execute(bsql);
				bsql = File.ReadAllText(projectDirectory + "\\Table\\Set.sql");
				conn.Execute(bsql);
				bsql = File.ReadAllText(projectDirectory + "\\Table\\Card.sql");
				conn.Execute(bsql);
				bsql = File.ReadAllText(projectDirectory + "\\Table\\CardFace.sql");
				conn.Execute(bsql);
				bsql = File.ReadAllText(projectDirectory + "\\Table\\TypeLine.sql");
				conn.Execute(bsql);
				bsql = File.ReadAllText(projectDirectory + "\\Table\\CardPart.sql");
				conn.Execute(bsql);
				// Stored Procedures
				bsql = File.ReadAllText(projectDirectory + "\\StoredProcedure\\TableCounts.sql");
				conn.Execute(bsql);
				return;
				// Data
				bsql = File.ReadAllText(projectDirectory + "\\Data\\SetType.sql");
				conn.Execute(bsql);
				bsql = File.ReadAllText(projectDirectory + "\\Data\\Set.sql");
				conn.Execute(bsql);
				bsql = File.ReadAllText(projectDirectory + "\\Data\\Rarity.sql");
				conn.Execute(bsql);


				//bsql = File.ReadAllText(projectDirectory + "\\Data\\Card.sql");
				//conn.Execute(bsql);
				int count = 0;
				string val = "SET IDENTITY_INSERT [MTG].[Card] ON;";
				foreach (var line in File.ReadAllLines(projectDirectory + "\\Data\\Card.sql").Skip(1))
				{
					val += line+";";
					count++;
					if(count == 100)
					{
						count = 0;
						conn.Execute(val);
						val = "SET IDENTITY_INSERT [MTG].[Card] ON;";
					}
				}

				bsql = File.ReadAllText(projectDirectory + "\\Data\\CardFace.sql");
				conn.Execute(bsql);
				bsql = File.ReadAllText(projectDirectory + "\\Data\\CardPart.sql");
				conn.Execute(bsql);
				bsql = File.ReadAllText(projectDirectory + "\\Data\\CardType.sql");
				conn.Execute(bsql);
				bsql = File.ReadAllText(projectDirectory + "\\Data\\TypeLine.sql");
				conn.Execute(bsql);

			}

			// SetType
			// Set
			// Rarity
			// Card

			//foreach (var item in Directory.GetFiles(projectDirectory+"\\"+dir))
			//{
			//	bsql = File.ReadAllText(item);

			//	//using (var conn = new SqlConnection(connectionString))
			//	//{
			//	//	conn.Execute(bsql);
			//	//}
			//}
		}
	}
}