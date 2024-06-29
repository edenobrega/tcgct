using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcgct_services_framework.Generic.Interface
{
    public interface ISettingsService
    {
        Dictionary<string, int> GetGameIDs();
        int GetGameID(string Game);
        SettingsRow? GetSetting(string Key, int GameID, int UserID);
        void UpdateSetting(SettingsRow row);
    }
}
