using Microsoft.AspNetCore.Components.Authorization;

namespace tcgct_mud.Helpers
{
	public static class AuthHelper
	{
		public static int GetUserID(AuthenticationState authState)
		{
			return int.Parse(authState.User.Claims.Single(s => s.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
		}

		public static string GetUserName(AuthenticationState authState)
		{
			return authState.User.Claims.Single(s => s.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

		}
	}
}