using Microsoft.Data.SqlClient;
using Dapper;

namespace DatabaseBuilder
{
    public class DatabaseBuilder
    {
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
            using (var conn = new SqlConnection($"Server=localhost\\SQLEXPRESS;Database={db_name};Trusted_Connection=True;"))
            {
                string bsql;
                // Schemas
                bsql = File.ReadAllText(".\\database\\mtg\\Schema\\MTG.sql");
                conn.Execute(bsql);
                // Tables
                bsql = File.ReadAllText(".\\database\\mtg\\Table\\CardType.sql");
                conn.Execute(bsql);
                bsql = File.ReadAllText(".\\database\\mtg\\Table\\Collection.sql");
                conn.Execute(bsql);
                bsql = File.ReadAllText(".\\database\\mtg\\Table\\PinnedSet.sql");
                conn.Execute(bsql);
                bsql = File.ReadAllText(".\\database\\mtg\\Table\\Rarity.sql");
                conn.Execute(bsql);
                bsql = File.ReadAllText(".\\database\\mtg\\Table\\SetType.sql");
                conn.Execute(bsql);
                bsql = File.ReadAllText(".\\database\\mtg\\Table\\Set.sql");
                conn.Execute(bsql);
                bsql = File.ReadAllText(".\\database\\mtg\\Table\\Card.sql");
                conn.Execute(bsql);
                bsql = File.ReadAllText(".\\database\\mtg\\Table\\CardFace.sql");
                conn.Execute(bsql);
                bsql = File.ReadAllText(".\\database\\mtg\\Table\\TypeLine.sql");
                conn.Execute(bsql);
                bsql = File.ReadAllText(".\\database\\mtg\\Table\\CardPart.sql");
                conn.Execute(bsql);
				bsql = File.ReadAllText(".\\database\\mtg\\Table\\Information.sql");
				conn.Execute(bsql);
				// Stored Procedures
				bsql = File.ReadAllText(".\\database\\mtg\\StoredProcedure\\TableCounts.sql");
                conn.Execute(bsql);
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