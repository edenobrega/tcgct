using System.Security.Principal;
using tcgct_services_framework.Identity.Interface;

namespace tcgct_services_framework.Identity
{
    public class TCGCTUser : IIdentity
    {
        public string? AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        private string _name;
        public string Name 
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public string Username
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public int ID { get; set; }
        public Guid UID { get; set; }
        public string Password { get; set; }
    }
}
