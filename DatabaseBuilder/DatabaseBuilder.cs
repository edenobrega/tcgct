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
                string sql = @"insert into MTG.[SetType] values ('Default Test Set Type')
								insert into MTG.[Set] values ('Default Test Set', 'DTS', 'https://upload.wikimedia.org/wikipedia/commons/thumb/5/57/Circled_plus.svg/1200px-Circled_plus.svg.png', 'Test search uri', 1, 'set_1')
								insert into MTG.[CardType] values ('—'), ('One'), ('Two'), ('Three'), ('//')
								insert into MTG.[Rarity] values ('Normal'), ('Uncommon')
								insert into MTG.[Card] values 
								('Card 1',          '5{B}{W}',      'This is the card text',          'This is flavour test', 'Artist',       '1', '4', '3',  1, 'card_1', 7, 'link to image here', 'link to flipped image here', 'oracle_id_1', 1, 0),
								('Card 2',          '3{R}{R}',      'This is the SECOND card text',   null,                   'Artist Two',   '2', '5', '7',  1, 'card_2', 5, 'link to image here', null,                         'oracle_id_2', 2, 0),
								('Card 3',          '1{G}',         'This is the third card text',    null,                   'Artist Two',   '3', '1', '1',  1, 'card_3', 2, 'link to image here', 'link to flipped image here', 'oracle_id_3', 1, 0),
								('Card 4',          '{U}',          'This is the card text',          'This is flavour test', 'Artist',       '4', '2', '1',  1, 'card_4', 1, 'link to image here', 'link to flipped image here', 'oracle_id_4', 2, 0),
								('Card 5',          '5{B}{W}',      'This is the card text',          'This is flavour test', 'Artist Three', '5', '4', '3',  1, 'card_5', 7, 'link to image here', 'link to flipped image here', 'oracle_id_1', 1, 0),
								('Card 6',          '{B} // {R}',   null,                              null,                   'Artist',      '6',null, null, 1, 'card_4', 1, 'link to image here', 'link to flipped image here', 'oracle_id_4', 2, 1)
								insert into MTG.[CardFace] values 
								(6,'card_face', 'Face One', null, '{B}', null, 1, null, null, null, null, 1, 1),
								(6,'card_face', 'Face Two', null, '{R}', null, 1, null, null, null, null, 2, 1)
								insert into MTG.[CardPart] values
								(1, 'related_card', 'token', 4),
								(2, 'related_card', 'token', 3),
								(2, 'related_card', 'token', 4)
								insert into MTG.[TypeLine] VALUES
								(1, 2, 1),
								(1, 1, 2),
								(1, 4, 3),
								(3, 3, 1),
								(4, 3, 1),
								(2, 1, 1),
								(2, 1, 2),
								(5, 1, 1)
								";
                conn.Execute(sql);
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