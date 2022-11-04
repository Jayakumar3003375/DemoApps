using IdentityModel.Client;

namespace Property.Web.Services
{
    public class GetHttpClient: IGetHttpClient
    {
        private IIdentityService _identityServerService = null;
        public GetHttpClient(IIdentityService identityServerService)
        {
            _identityServerService = identityServerService;
        }
        public async Task<HttpClient> GetAsync()
        {
            try
            {
                HttpClient client = new HttpClient();
                var OAuth2Token = await _identityServerService.GetToken("read.write");
                client.SetBearerToken(OAuth2Token.AccessToken);
                return client;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
