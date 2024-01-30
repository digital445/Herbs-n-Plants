using Plants.Models;
using Plants.Models.Dto;
using Plants.Services.IServices;
using System.Reflection;
using System.Web;
using static Plants.Enumerations;

namespace Plants.Services
{
    public class PlantsService : BaseService, IPlantsService
    {
        private readonly string PlantsAPIBaseUrl;
		private readonly string PlantsAPIAccessToken;

		public PlantsService(
            IConfiguration configuration, 
            IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            PlantsAPIBaseUrl = configuration.GetValue<string>("Services:PlantsAPI:BaseUrl") ?? "";
#if DEBUG
            PlantsAPIAccessToken = Environment.GetEnvironmentVariable("PlantsAPIAccessToken") ?? "";
#else
            PlantsAPIAccessToken = configuration.GetValue<string>("Services:PlantsAPI:AccessToken") ?? "";
#endif
		}

		public async Task<T?> CreateUpdateAsync<T>(PlantDto plantDto)
        {
            return await SendAsync<T>(new ApiRequestDto {
                ApiType = ApiType.POST,
                Url = PlantsAPIBaseUrl + "/api/plants",
                Data = plantDto,
                AccessToken = PlantsAPIAccessToken
			});
        }
        public async Task<T?> DeleteAsync<T>(int id)
        {
            return await SendAsync<T>(new ApiRequestDto
            {
                ApiType = ApiType.DELETE,
                Url = PlantsAPIBaseUrl + $"/api/plants/{id}",
                AccessToken = PlantsAPIAccessToken
			});
        }

        public async Task<T?> GetAsync<T>(int id)
        {
            return await SendAsync<T>(new ApiRequestDto
            {
                ApiType = ApiType.GET,
                Url = PlantsAPIBaseUrl + $"/api/plants/{id}",
                AccessToken = PlantsAPIAccessToken
			});
        }

        public async Task<T?> GetFilteredAsync<T>(FilterDto filter, int page, int pageSize)
		{
            string baseUrl = PlantsAPIBaseUrl + "/api/plants/filter";

			return await SendAsync<T>(new ApiRequestDto
			{
				ApiType = ApiType.GET,
				Url = ConstructFilterUrl(baseUrl, filter, page, pageSize),
				AccessToken = PlantsAPIAccessToken
			});
		}


		public async Task<T?> GetPageAsync<T>(int page, int pageSize)
        {
            return await SendAsync<T>(new ApiRequestDto
            {
                ApiType = ApiType.GET,
                Url = PlantsAPIBaseUrl + $"/api/plants?page={page}&pageSize={pageSize}",
                AccessToken = PlantsAPIAccessToken
			});
        }
        public async Task<T?> GetPaletteAsync<T>()
        {
            return await SendAsync<T>(new ApiRequestDto
            {
                ApiType = ApiType.GET,
                Url = PlantsAPIBaseUrl + "/api/plants/palette",
                AccessToken = PlantsAPIAccessToken
			});
        }

		/// <summary>
		/// Constructs the URL from FilterDto properties
		/// </summary>
		private string ConstructFilterUrl(string baseUrl, FilterDto filter, int page, int pageSize)
		{
			var uriBuilder = new UriBuilder(baseUrl);
			var query = HttpUtility.ParseQueryString(uriBuilder.Query);
			query.Add("page", page.ToString());
			query.Add("pageSize", pageSize.ToString());

			List<PropertyInfo> properties = filter.GetType().GetProperties().ToList();
			properties.ForEach(pi => query.Add(pi.Name, pi.GetValue(filter)?.ToString()));

			uriBuilder.Query = query.ToString();
			return uriBuilder.ToString();
		}

	}
}
