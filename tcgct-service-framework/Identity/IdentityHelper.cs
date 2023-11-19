using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using tcgct_services_framework.Identity.Interface;

namespace tcgct_services_framework.Identity
{
    // None of this is probably even needed, but it was fun to make.
    public static class IdentityHelper
    {
        internal class ImplementedStamps
        {
            public bool ICustomDataAccess { get; set; } = false;
            public bool ICustomIdentityUser { get; set; } = false;
            public bool ICustomRole { get; set; } = true;
            public bool ICustomRoleStore { get; set; } = true;
            public bool ICustomUserStore { get; set; } = false;

            public bool CheckIfFullyImplemented()
            {
                return ICustomDataAccess &&
                    ICustomIdentityUser &&
                    ICustomRole &&
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
        public static void CheckBackendTechs()
        {
            Dictionary<string, ImplementedStamps> stamps = new Dictionary<string, ImplementedStamps>();
            var ding = Assembly.GetExecutingAssembly().GetTypes().Where(w => w.FullName != "tcgct_services_framework.Identity.Implementations" && w.FullName.Contains("tcgct_services_framework.Identity.Implementations"));

            foreach (var imps in GetImplementations())
            {
                Console.WriteLine($"Implementation for {imps} found");
                stamps[imps] = new ImplementedStamps();
            }

            foreach (var item in ding)
            {
                if (item.GetInterfaces().Length > 0)
                {
                    foreach (var i in item.GetInterfaces())
                    {
                        switch (i.Name)
                        {
                            case "ICustomDataAccess`1":
                                stamps[item.FullName.Split('.').SkipLast(1).TakeLast(1).First()].ICustomDataAccess = true;
                                break;
                            case "ICustomIdentityUser":
                                stamps[item.FullName.Split('.').SkipLast(1).TakeLast(1).First()].ICustomIdentityUser = true;
                                break;
                            case "ICustomRole`1":
                                stamps[item.FullName.Split('.').SkipLast(1).TakeLast(1).First()].ICustomRole = true;
                                break;
                            case "ICustomRoleStore`1":
                                stamps[item.FullName.Split('.').SkipLast(1).TakeLast(1).First()].ICustomRoleStore = true;
                                break;
                            case "ICustomUserStore`1":
                                stamps[item.FullName.Split('.').SkipLast(1).TakeLast(1).First()].ICustomUserStore = true;
                                break;
                        }
                    }
                }
            }

            foreach (var stamp in stamps)
            {
                string str = stamp.Value.CheckIfFullyImplemented() ? "fully implemented" : "not fully implemented";
                Console.WriteLine($"Implementation for {stamp.Key} is {str}.");
            }
        }
        public static IdentityClassHolder GetClasses(string tech)
        {
            IdentityClassHolder ich = new IdentityClassHolder();

            return ich;
        }
    }
}
