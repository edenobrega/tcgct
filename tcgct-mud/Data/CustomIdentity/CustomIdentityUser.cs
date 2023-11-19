using Microsoft.AspNetCore.Identity;
using System.Security.Principal;

namespace tcgct_mud.Data.Identity
{
    public class CustomIdentityUser : IIdentity
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public string? AuthenticationType => "Custom Identity";

        public bool IsAuthenticated { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }
    }
}
