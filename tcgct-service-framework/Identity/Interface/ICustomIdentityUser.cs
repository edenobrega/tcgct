using System.Security.Principal;

namespace tcgct_services_framework.Identity.Interface
{
    public interface ICustomIdentityUser : IIdentity
    {
        public string ID { get; set; }
        public string Password { get; set; }
    }
}
