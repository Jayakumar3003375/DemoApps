using IdentityModel.Client;
namespace Property.Web.Services
{
    public class IdentityService : IIdentityService
    {
        private DiscoveryDocumentResponse _discoveryDocument { get; set; }
        IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        public IdentityService()
        {
            using (var client = new HttpClient())
            {
                _discoveryDocument = client.GetDiscoveryDocumentAsync(configuration["Identity:Url"] + "/.well-known/openid-configuration").Result;
            }
        }
        public async Task<TokenResponse> GetToken(string apiScope)
        {
            using (var client = new HttpClient())
            {
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = _discoveryDocument.TokenEndpoint,
                    ClientId = configuration["Identity:ClientId"],
                    Scope = apiScope,
                    ClientSecret = configuration["Identity:ClientSecret"]
                });
                if (tokenResponse.IsError)
                {
                    throw new Exception("Token Error");
                }
                return tokenResponse;
            }
        }
    }
}
