using System.Security.Principal;
using tcgct_services_framework.Identity.Interface;

namespace tcgct_services_framework.Identity
{
    public class TCGCTUser : IIdentity
    {
        public string? AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        public string Name { get; set; }
        public Guid ID { get; set; }
        public string Password { get; set; }
    }
}
