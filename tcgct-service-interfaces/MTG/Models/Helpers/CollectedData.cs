using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_services_framework.MTG.Models.Helpers
{
	public class CollectedData
	{
        public int SetID { get; set; } = 0;
        public int CollectedCards { get; set; } = 0;
        public int TotalCards { get; set; } = 0;
    }
}
