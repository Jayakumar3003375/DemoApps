namespace Property.Web.Services
{
    public interface IGetHttpClient
    {
        Task<HttpClient> GetAsync();
    }
}
