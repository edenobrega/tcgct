using System.Security.Principal;
using tcgct_services_framework.Identity.Interface;

namespace tcgct_services_framework.Identity
{
    public class TCGCTUser : IIdentity
    {
        public string? AuthenticationType => throw new NotImplementedException();

        public bool IsAuthenticated => throw new NotImplementedException();

        public string Name { get; set; }
        public string ID { get; set; }
        public string Password { get; set; }
    }
}
