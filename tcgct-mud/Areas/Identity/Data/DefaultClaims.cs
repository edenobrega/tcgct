using System.Security.Claims;

namespace tcgct_mud.Areas.Identity.Data
{
	public static class DefaultClaims
	{
		public static readonly List<Claim> claims = new List<Claim>
		{
			new Claim(type:"mtg_show_sets_ids", value:""),
			// Format "TO | FROM"
			new Claim(type:"mtg_show_sets_date", value:""),
			new Claim(type:"mtg_show_sets_types", value:"")
		};
	}
}
