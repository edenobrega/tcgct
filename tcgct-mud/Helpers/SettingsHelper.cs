using tcgct_services_framework.Generic;
using tcgct_services_framework.Generic.Interface;
using tcgct_sql.Services;

namespace tcgct_mud.Helpers
{
	public class SettingsHelper
	{
		readonly ISettingsService settingsService;
        public SettingsHelper(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
		}

		private IEnumerable<int> GetInts(Guid userID, int GameID, string SettingName)
		{
			SettingsRow? setting = settingsService.GetSetting(SettingName, GameID, userID);
			if (setting is not null && !string.IsNullOrEmpty(setting.Value))
			{
				return setting.Value.Split(",").Select(int.Parse);
			}
			return Enumerable.Empty<int>();
		}

		public IEnumerable<int> GetFilterBySetIDs(Guid UserID, int GameID)
        {
			return GetInts(UserID, GameID, "FilterBySetIDs");
		}

		public IEnumerable<int> GetFilterBySetTypes(Guid UserID, int GameID)
		{
			return GetInts(UserID, GameID, "FilterBySetTypes");
		}
	}
}
