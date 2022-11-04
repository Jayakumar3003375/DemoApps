﻿using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Property.Web.Models;
using Property.Web.Services.Base;
using System.Net;

namespace Property.Web.Services
{
    public class PropertyHttpservice : IApiService<Properties>
    {
        private readonly IOptions<ApiUrls> apiUrls;
        private readonly string propertyApiUrl;
        private readonly IGetHttpClient _client;
        
        public PropertyHttpservice(IOptions<ApiUrls> apiUrls, IGetHttpClient client)
        {
            this.apiUrls = apiUrls;
            propertyApiUrl = apiUrls.Value.PropertiesApiUrl;
            _client = client;
        }
        public async Task<ApiResponseModel<Properties>> AddAsync(Properties properties)
        {
            ApiResponseModel<Properties> response = null;

            var client = await _client.GetAsync();
            var httpResponse = await client.PostAsJsonAsync<Properties>(propertyApiUrl, properties);
            if (httpResponse.StatusCode == HttpStatusCode.Created)
            {
                var result = await httpResponse.Content.ReadAsStringAsync();
                var reponseData = JsonConvert.DeserializeObject<Properties>(result);
                response = new ApiResponseModel<Properties> { ResponseCode = HttpStatusCode.Created, Message = properties.Type + " created successfully", Record = reponseData };
            }
            return response;
        }

        public async Task<ApiResponseModel<Properties>> DeleteAsync(int? id)
        {
            var url = $"{propertyApiUrl}/{id}";
            ApiResponseModel<Properties> response = null;
            var client = await _client.GetAsync();
            var httpResponse = await client.DeleteAsync(url);
            var result = await httpResponse.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<Properties>(result);
            response = new ApiResponseModel<Properties> { ResponseCode = HttpStatusCode.OK, Message = "Property deleted successfully", Record = responseData };

            return response;
        }

        public async Task<ApiResponseModel<IEnumerable<Properties>>> GetAllAsync()
        {
            ApiResponseModel<IEnumerable<Properties>> response = null;
            var client = await _client.GetAsync();
            var httpResponse = await client.GetAsync(propertyApiUrl);
            var result = await httpResponse.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Properties>>(result);
            if (data != null && data.Count > 0)
            {
                response = new ApiResponseModel<IEnumerable<Properties>> { ResponseCode = HttpStatusCode.Found, Message = "Records found", Record = data.ToList<Properties>() };
            }
            return response;
        }
        public async Task<ApiResponseModel<Properties>> GetAsync(int? id)
        {
            var url = $"{propertyApiUrl}/{id}";
            ApiResponseModel<Properties> response = null;
            var client = await _client.GetAsync();
            var httpResponse = await client.GetAsync(url);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var result = await httpResponse.Content.ReadAsStringAsync();
                var reponseData = JsonConvert.DeserializeObject<Properties>(result);
                response = new ApiResponseModel<Properties> { ResponseCode = HttpStatusCode.Found, Message = "Data was found.", Record = reponseData };
            }
            else
            {
                response = new ApiResponseModel<Properties> { ResponseCode = HttpStatusCode.NotFound, Message = "No record found", Record = new Properties() };
            }

            return response;
        }

        public async Task<ApiResponseModel<Properties>> UpdateAsync(Properties properties)
        {
            ApiResponseModel<Properties> response = null;
            var client = await _client.GetAsync();
            //var res = JsonConvert.SerializeObject(properties);
            var httpResponse = await client.PutAsJsonAsync<Properties>(propertyApiUrl, properties);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var result = await httpResponse.Content.ReadAsStringAsync();
                response = new ApiResponseModel<Properties> { ResponseCode = HttpStatusCode.OK, Message = "Property -" + properties.PropertyNumber + " updated successfully", Record = properties };
            }

            return response;
        }
    }
}
