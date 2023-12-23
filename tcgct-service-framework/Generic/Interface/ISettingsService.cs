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
		Task CreateDefaultSettings(Guid UserID);
        SettingsRow? GetSetting(string Key, int GameID, Guid UserID);
        void UpdateSetting(SettingsRow row);
    }
}
