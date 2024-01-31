using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Services.Identity
{
	public static class StaticDetails
	{
		//constants for roles
		public const string Admin = "Admin";
		public const string Customer = "Customer";

		public static IEnumerable<IdentityResource> IdentityResources => //here we use an EXPRESSION BODY to create a read-only property.

			new List<IdentityResource>
			{
				new IdentityResources.OpenId(), //IdentityResource is a named group of claims that can be requested using a scope parameter
                new IdentityResources.Email(),
				new IdentityResources.Profile()
			};

		public static IEnumerable<ApiScope> ApiScopes =>
			new List<ApiScope>
			{
				new ApiScope("ApiAccess", "Access to APIs"), 
                new ApiScope(name: "read", displayName: "Read your data."),
				new ApiScope(name: "write", displayName: "Write your data."),
				new ApiScope(name: "delete", displayName: "Delete your data."),
			};
		public static IEnumerable<Client> Clients =>
			new List<Client>
			{
				new Client
				{
					ClientId  = "Plants.Web",
					ClientSecrets = {new Secret("secret".Sha256())},
					AllowedGrantTypes = GrantTypes.Code, //The Authorization Code grant type is used by confidential and public clients
                                                         //to exchange an authorization code for an access token.
                    RedirectUris = {"https://localhost:7174/signin-oidc"}, //URL for successful login
                                                                           //Parent address is taken from Mango.Web\Properties\launchSettings.json  https section
                    PostLogoutRedirectUris = {"https://localhost:7174/signout-callback-oidc"}, //URL  for successful logout
					RequirePkce = true,
                    AllowedScopes = new List<string>
					{
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Email,
						IdentityServerConstants.StandardScopes.Profile,
						"ApiAccess"
					}
				},
			};
	}
}
