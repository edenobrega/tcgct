using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_services_framework.Generic
{
	public class SettingsRow
	{
        public int ID { get; set; }
        public int GameID { get; set; }
        public string UserID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
