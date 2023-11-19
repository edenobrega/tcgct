using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tcgct_services_framework.Identity.Interface;

namespace tcgct_services_framework.Identity.Implementations.MongoDB
{
    public class MongoIdentityUser : ICustomIdentityUser
    {
        public string ID { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Password { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string? AuthenticationType => throw new NotImplementedException();

        public bool IsAuthenticated => throw new NotImplementedException();

        public string? Name => throw new NotImplementedException();
    }
}
