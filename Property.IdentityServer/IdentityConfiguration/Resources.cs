using IdentityServer4.Models;

namespace Property.IdentityServer.IdentityConfiguration
{
    public class Resources
    {

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> {"role"}
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource
                {
                    Name = "PropertyService",
                    DisplayName = "Weather Api",
                    Description = "Allow the application to access Land & Property Service",
                    Scopes = new List<string> { "read", "write","read.write"},
                    ApiSecrets = new List<Secret> {new Secret("Equiniti".Sha256())},
                    UserClaims = new List<string> {"role"}
                }
            };
        }

    }
}
