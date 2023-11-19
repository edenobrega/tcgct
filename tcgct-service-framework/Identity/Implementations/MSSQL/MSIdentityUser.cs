using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcgct_services_framework.Identity.Interface;

namespace tcgct_services_framework.Identity.Implementations.MSSQL
{
    public class MSIdentityUser : ICustomIdentityUser
    {
        public string? AuthenticationType => throw new NotImplementedException();

        public bool IsAuthenticated => throw new NotImplementedException();

        public string Name { get; set; }
        public string ID { get; set; }
        public string Password { get; set; }
    }
}
