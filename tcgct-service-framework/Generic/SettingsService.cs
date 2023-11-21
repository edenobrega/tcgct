using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_services_framework.Generic
{
    public class SettingsService
    {
        public SettingsService(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public string ConnectionString { get; set; }
    }
}
