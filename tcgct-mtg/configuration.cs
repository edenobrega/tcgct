using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_mtg
{
    static public class configuration
    {
        public static void ConfigureConnectionString(string ConnectionString)
        {
            connectionString = ConnectionString;
        }
        internal static string connectionString = "Server=localhost\\SQLEXPRESS;Database=tcgct;Trusted_Connection=True;";
    }
}
