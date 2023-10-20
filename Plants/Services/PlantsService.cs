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

        public async Task<T?> GetAsync<T>(int id, string token)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = ApiType.GET,
                Url = PlantsAPIBaseUrl + $"/api/plants/{id}",
                AccessToken = token
            });
        }

        public async Task<T?> GetFilteredAsync<T>(FilterDto filter, int page, int pageSize, string token)
        {
            //construct URL from plantDto properties to pass them into URL as a query string
            var uriBuilder = new UriBuilder(PlantsAPIBaseUrl + "/api/plants/filter");
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query.Add("page", page.ToString());
            query.Add("pageSize", pageSize.ToString());

            List<PropertyInfo> properties = filter.GetType().GetProperties().ToList();
            properties.ForEach(pi => query.Add(pi.Name, pi.GetValue(filter)?.ToString()));

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
