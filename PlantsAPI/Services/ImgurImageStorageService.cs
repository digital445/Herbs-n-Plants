using System.Net.Http.Headers;
using Services.PlantsAPI.Services.IServices;
using Services.PlantsAPI.Models.Dto;
using static Services.PlantsAPI.StaticDetails;

namespace Services.PlantsAPI.Services
{
	public class ImgurImageStorageService : BaseService, IImageStorageService
	{
		private const string albumHash = "piwTiWk";
		public ImgurImageStorageService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
		{
		}

		public async Task<T?> DeleteImageAsync<T>(string imageServiceId, string token)
		{
			return await SendAsync<T?>(new ApiRequestDto
			{
				ApiType = ApiType.DELETE,
				Url = $"https://api.imgur.com/3/image/{imageServiceId}",
				AccessToken = token
			});
		}
	}
}
