using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_services_framework.Generic.Interface
{
    public interface ISettingsService
    {
        Task CreateDefaultSettings(string UserID);
        SettingsRow? GetSetting(string Key, string UserID);
        void UpdateSetting(SettingsRow row);
    }
}
