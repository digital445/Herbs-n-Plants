using System.Net.Http.Headers;
using Services.PlantsAPI.Services.IServices;
using Services.PlantsAPI.Models.Dto;
using static Services.PlantsAPI.Enumerations;
using Microsoft.Extensions.Configuration;

namespace Services.PlantsAPI.Services
{
	public class ImgurImageStorageService : BaseService, IImageStorageService
	{
		private readonly string ImgurAPIBaseUrl;
		private readonly string ImgurAPIAccessToken;

		public ImgurImageStorageService(
			IConfiguration configuration, 
			IHttpClientFactory httpClientFactory) : base(httpClientFactory)
		{
			ImgurAPIBaseUrl = configuration.GetValue<string>("Services:ImgurAPI:BaseUrl") ?? "";
#if DEBUG
			ImgurAPIAccessToken = Environment.GetEnvironmentVariable("ImgurAPIAccessToken") ?? "";
#else
			ImgurAPIAccessToken = configuration.GetValue<string>("Services:ImgurAPI:AccessToken") ?? "";
#endif

		}

		public async Task<T?> DeleteImageAsync<T>(string imageServiceId)
		{
			return await SendAsync<T?>(new ApiRequestDto
			{
				ApiType = ApiType.DELETE,
				Url = ImgurAPIBaseUrl + $"/image/{imageServiceId}",
				AccessToken = ImgurAPIAccessToken
			});
		}
	}
}
