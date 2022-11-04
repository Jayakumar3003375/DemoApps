using Microsoft.Extensions.Options;
using Property.Web.Models;

namespace Property.Web.Services.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IOptions<ApiUrls> apiUrls, IGetHttpClient client)
        {
            RegistrationService = new RegistrationHttpservice(apiUrls, client);
            PropertiesService = new PropertyHttpservice(apiUrls, client);
            OccupancyService = new OccupancyHttpservice(apiUrls, client);
        }
        public IApiService<RegistrationModel> RegistrationService { get; private set; }
        public IApiService<Properties> PropertiesService { get; private set; }
        public IApiService<Occupancy> OccupancyService { get; private set; }

    }
}
