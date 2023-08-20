using Microsoft.Data.SqlClient;
using Dapper;
using System.Runtime.CompilerServices;

namespace DatabaseBuilder
{
    public class DatabaseBuilder
    {
        class BuildOrder
        {
            public BuildOrder()
            {
                Dependencies = new List<string>();
				Order = Random.Shared.Next(1, 1000);
            }

            public int Order;
            public string TableName;
            public string Query;
            public List<string> Dependencies;
        }

        public static void Build(string db_name)
        {
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


			// Prepare build order for tables
			List<BuildOrder> tables = new List<BuildOrder>();
			foreach (var item in Directory.GetFiles(".\\Database\\MTG\\Table"))
			{
				var query = File.ReadAllText(item);

				BuildOrder tableOrder = new BuildOrder
				{
					TableName = query.Split("CREATE TABLE")[1].Trim().Split(" ")[0].Split("(")[0],
					Query = query
				};

				var dependencies = query.Split("REFERENCES").Skip(1);
				foreach (var table in dependencies)
				{
					tableOrder.Dependencies.Add(table.Split(" ")[1]);
				}

				tables.Add(tableOrder);
			}
			int changes = 0;
			while (true)
			{
				foreach (var twd in tables)
				{
					foreach (var dep in twd.Dependencies)
					{
						var v = tables.Single(s => s.TableName == dep).Order;
						if (twd.Order > v)
						{
							twd.Order = v - 1;
							changes++;
						}
					}
				}
				if(changes == 0)
				{
					break;
				}
				changes = 0;
			}

			tables = tables.OrderByDescending(o => o.Order).ThenByDescending(t => t.Dependencies.Count).ToList();

            string bsql;
			using (var conn = new SqlConnection(connectionString))
            {
				foreach (var item in Directory.GetFiles(".\\Database\\MTG\\Schema"))
				{
					bsql = File.ReadAllText(item);
					conn.Execute(bsql);
				}

				foreach (var item in tables)
				{
                    Console.WriteLine(item.TableName);
                    conn.Execute(item.Query);
				}

				foreach (var item in Directory.GetFiles(".\\Database\\MTG\\StoredProcedure"))
				{
					bsql = File.ReadAllText(item);
					conn.Execute(bsql);
				}
			}
        }
        public static void BuildTestDB(string db_name)
        {
            Build(db_name);
            string connectionString = $"Server=localhost\\SQLEXPRESS;Database={db_name};Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
				string bsql = File.ReadAllText(".\\database\\mtg\\TestData.sql");
				conn.Execute(bsql);
            }
        }
        public static void DropTestDB(string db_name)
        {
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
                conn.Close();
            }
        }
    }
}