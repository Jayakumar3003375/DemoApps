using IdentityModel.Client;

namespace Property.Web.Services
{
    public interface IIdentityService
    {
        Task<TokenResponse> GetToken(string apiScope);
    }
}
