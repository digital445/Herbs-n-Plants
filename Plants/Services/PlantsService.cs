using Plants.Models;
using Plants.Models.Dto;
using Plants.Services.IServices;
using System.Reflection;
using System.Web;
using static Plants.StaticDetails;

namespace Plants.Services
{
    public class PlantsService : BaseService, IPlantsService
    {
        public PlantsService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async Task<T?> CreateAsync<T>(PlantDto plantDto, string token)
        {
            return await SendAsync<T>(new ApiRequest {
                ApiType = ApiType.POST,
                Url = PlantsAPIBaseUrl + "/api/plants",
                Data = plantDto,
                AccessToken = token
            });
        }
        public async Task<T?> UpdateAsync<T>(PlantDto plantDto, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.PUT,
                Url = PlantsAPIBaseUrl + "/api/plants",
                Data = plantDto,
                AccessToken = token
            });
        }

        public async Task<T?> DeleteAsync<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.DELETE,
                Url = PlantsAPIBaseUrl + $"/api/plants/{id}",
                AccessToken = token
            });
        }

        public async Task<T?> GetAllAsync<T>(string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.GET,
                Url = PlantsAPIBaseUrl + "/api/plants",
                AccessToken = token
            });
        }

        public async Task<T?> GetAsync<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.GET,
                Url = PlantsAPIBaseUrl + $"/api/plants/{id}",
                AccessToken = token
            });
        }

        public async Task<T?> GetFilteredAsync<T>(PlantDto plantDto, string token)
        {
            //construct URL from plantDto properties to pass them in URL as query string
            var uriBuilder = new UriBuilder(PlantsAPIBaseUrl + "/api/plants/filter");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            List<PropertyInfo> properties = plantDto.GetType().GetProperties().ToList();
            properties.RemoveAll(pi => pi.GetCustomAttribute(typeof(SkipFilteringAttribute)) != null); //skip properties marked with SkipFilter attribute
            properties.ForEach(pi => query.Add(pi.Name, pi.GetValue(plantDto)?.ToString()));

            string? nameToFind = plantDto.Names?.FirstOrDefault()?.Name;
            if (!string.IsNullOrEmpty(nameToFind))
            {
                query.Add("Names[0].Name", nameToFind);
            }            

            uriBuilder.Query = query.ToString();
            string finalUrl = uriBuilder.ToString();

            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.GET,
                Url = finalUrl,
                AccessToken = token
            });
        }

        public async Task<T?> GetPageAsync<T>(int page, int pageSize, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.GET,
                Url = PlantsAPIBaseUrl + $"/api/plants?page={page}&pageSize={pageSize}",
                AccessToken = token
            });
        }
    }
}
