using System.Reflection;
using tcgct_services_framework.Identity.Implementations.MSSQL;

namespace tcgct_services_framework.Identity
{
    // None of this is probably even needed, but it was fun to make.
    public static class IdentityHelper
    {
        internal class ImplementedStamps
        {
            public bool ICustomDataAccess { get; set; } = false;
            public bool ICustomRoleStore { get; set; } = false;
            public bool ICustomUserStore { get; set; } = false;

            public bool CheckIfFullyImplemented()
            {
                return ICustomDataAccess &&
                    ICustomRoleStore &&
                    ICustomUserStore;
            }
        }
        public static IEnumerable<string> GetImplementations()
        {
            List<string> Implementations = new List<string>();
            var Assemblies = Assembly.GetExecutingAssembly().GetTypes().Where(w => w.FullName != "tcgct_services_framework.Identity.Implementations" && w.FullName.Contains("tcgct_services_framework.Identity.Implementations"));
            foreach (var item in Assemblies)
            {
                Implementations.Add(item.FullName.Split('.').SkipLast(1).TakeLast(1).First());
            }
            return Implementations.Distinct();
        }
        public static IEnumerable<string> GetValidImplementations()
        {
            Dictionary<string, ImplementedStamps> stamps = new Dictionary<string, ImplementedStamps>();
            var imps = Assembly.GetExecutingAssembly().GetTypes().Where(w => w.FullName != "tcgct_services_framework.Identity.Implementations" && w.FullName.Contains("tcgct_services_framework.Identity.Implementations"));

            foreach (var imp in GetImplementations())
            {
                Console.WriteLine($"Implementation for {imp} found");
                stamps[imp] = new ImplementedStamps();
            }

            foreach (var item in imps)
            {
                if (item.GetInterfaces().Length > 0)
                {
                    foreach (var i in item.GetInterfaces())
                    {
                        switch (i.Name)
                        {
                            case "ICustomDataAccess":
                                stamps[item.FullName.Split('.').SkipLast(1).TakeLast(1).First()].ICustomDataAccess = true;
                                break;
                            case "IRoleStore`1":
                                stamps[item.FullName.Split('.').SkipLast(1).TakeLast(1).First()].ICustomRoleStore = true;
                                break;
                            case "ICustomUserStore":
                                stamps[item.FullName.Split('.').SkipLast(1).TakeLast(1).First()].ICustomUserStore = true;
                                break;
                        }
                    }
                }
            }
            var fic = stamps.Where(w => w.Value.CheckIfFullyImplemented()).Select(s => s.Key).ToArray();

            foreach (var stamp in stamps)
            {
                string str = stamp.Value.CheckIfFullyImplemented() ? "fully implemented" : "not fully implemented";
                Console.WriteLine($"Implementation for {stamp.Key} is {str}.");
            }

            return fic;
        }
        public static IdentityClassHolder GetClasses(string tech)
        {
            IdentityClassHolder ich = new IdentityClassHolder();
            var imps = Assembly.GetExecutingAssembly().GetTypes().Where(w => w.FullName != "tcgct_services_framework.Identity.Implementations" && w.FullName.Contains("tcgct_services_framework.Identity.Implementations."+tech));
            foreach (var item in imps)
            {
                if (item.GetInterfaces().Length > 0)
                {
                    foreach (var i in item.GetInterfaces())
                    {
                        switch (i.Name)
                        {
                            case "ICustomDataAccess":
                                ich.DataAccess = item;
                                break;
                            case "IRoleStore`1":
                                ich.RoleStore = item;
                                break;
                            case "ICustomUserStore":
                                ich.UserStore = item;
                                break;
                        }
                    }
                }
            }
            return ich;
        }
    }
}
