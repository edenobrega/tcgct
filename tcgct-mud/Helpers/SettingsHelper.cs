﻿using tcgct_services_framework.Generic;
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

		private IEnumerable<int> GetInts(int userID, int GameID, string SettingName)
		{
			SettingsRow? setting = settingsService.GetSetting(SettingName, GameID, userID);
			if (setting is not null && !string.IsNullOrEmpty(setting.Value))
			{
				return setting.Value.Split(",").Select(int.Parse);
			}
			return Enumerable.Empty<int>();
		}

		public IEnumerable<int> GetFilterBySetIDs(int UserID, int GameID)
        {
			return GetInts(UserID, GameID, "FilterBySetIDs");
		}

		public IEnumerable<int> GetFilterBySetTypes(int UserID, int GameID)
		{
			return GetInts(UserID, GameID, "FilterBySetTypes");
		}
	
		public Tuple<int, bool> GetCollectingSetsConfig(int UserID, int GameID)
		{
			var setting = settingsService.GetSetting("CollectingSets", GameID, UserID);
			if (setting is null)
			{
				throw new Exception("Setting not found");
			}
			var split = setting.Value.Split(',');
			return new Tuple<int, bool>(int.Parse(split[0]), int.Parse(split[1]) >= 1);
		}
	}
}
