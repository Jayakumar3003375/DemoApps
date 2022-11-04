using IdentityServer4;
using IdentityServer4.Models;

namespace Property.IdentityServer.IdentityConfiguration
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "PropertyService",
                    ClientName = "Land & Property Service",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret> {new Secret("Equiniti".Sha256())},
                    AllowedScopes = new List<string> {"read.write"}
                }
            };
        }
    }
}
