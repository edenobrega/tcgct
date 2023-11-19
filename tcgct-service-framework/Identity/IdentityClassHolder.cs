using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_services_framework.Identity
{
    public class IdentityClassHolder
    {
        public Type IdentityUser { get; set; }
        public Type DataAccess { get; set; }
        public Type UserStore { get; set; }
        public Type Role { get; set; }
        public Type RoleStore { get; set; }
    }
}
