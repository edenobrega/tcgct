﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_services_framework.MTG.Models
{
	public class CustomSet
	{
        public int ID { get; set; }
        public string Name { get; set; }
        public Guid Owner { get; set; }
    }
}
