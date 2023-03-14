using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcgct_mtg.Models;
using Dapper;

namespace tcgct_mtg.Services
{
    public class MTGSetService
    {
        public async Task<IEnumerable<MTGSet>> GetAll()
        {
            using (var conn = new SqlConnection(configuration.connectionString))
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(configuration.connectionString);
                conn.Open();
                var sql = $@"
                    SELECT *
                    FROM [{builder.InitialCatalog}].[dbo].[MTG_Set] as ms
                    left join [{builder.InitialCatalog}].[dbo].[MTG_SetType] as mst on set_type_id = mst.id 
                ";
                var results = await conn.QueryAsync<MTGSet, MTGSetType, MTGSet>(sql, (set, settype) => { set.Set_Type_id = settype; return set; }, splitOn: "set_type_id");
                conn.Close();
                return results;
            }
        }
    }
}
