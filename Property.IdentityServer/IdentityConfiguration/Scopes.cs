using IdentityServer4.Models;

namespace Property.IdentityServer.IdentityConfiguration
{
    public class Scopes
    {
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
                 new ApiScope("read.write", "Read & Write Access to Land & Property Service"),
            };
        }
    }
}
