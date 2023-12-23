using Microsoft.AspNetCore.Components.Authorization;

namespace tcgct_mud.Helpers
{
	public static class AuthHelper
	{
		public static Guid GetUserID(AuthenticationState authState)
		{
			return Guid.Parse(authState.User.Claims.Single(s => s.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
		}
	}
}