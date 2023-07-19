using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG.Classes.Response
{
    internal class SetsResponse
    {
        public string @object { get; set; }
        public bool has_more { get; set; }
        public IEnumerable<SetResponse> data { get; set; }
    }
}
