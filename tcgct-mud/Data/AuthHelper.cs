using Microsoft.AspNetCore.Components.Authorization;

public static class AuthHelper
{
	public static string GetUserID(AuthenticationState authState)
	{
		var uhoh = authState.User.Claims.Single(s => s.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
		return uhoh;
	}
}
