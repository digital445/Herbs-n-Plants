using System.Net.Http.Headers;
using Plants.Services.IServices;
using Plants.Models.Dto;
using static Plants.Enumerations;

namespace Plants.Services
{
	public class ImgurImageStorageService : BaseService, IImageStorageService
	{
		private readonly string ImgurAPIBaseUrl;
		private readonly string ImgurAPIAccessToken;
		private readonly string ImgurAPIAlbumHash;

		public ImgurImageStorageService(
			IConfiguration configuration,
			IHttpClientFactory httpClientFactory) : base(httpClientFactory)
		{
			ImgurAPIBaseUrl = configuration.GetValue<string>("Services:ImgurAPI:BaseUrl") ?? "";
#if DEBUG
			ImgurAPIAccessToken = Environment.GetEnvironmentVariable("ImgurAPIAccessToken") ?? "";
			ImgurAPIAlbumHash = Environment.GetEnvironmentVariable("ImgurAPIAlbumHash") ?? "";
#else
			ImgurAPIAccessToken = configuration.GetValue<string>("Services:ImgurAPI:AccessToken") ?? "";
			ImgurAPIAlbumHash = configuration.GetValue<string>("Services:ImgurAPI:AlbumHash") ?? "";
#endif
		}

		public async Task<T?> UploadImageAsync<T>(IFormFile file)
		{
			using var formDataContent = CreateFormData(file);

			return await SendAsync<T?>(new ApiRequestDto
			{
				ApiType = ApiType.POST,
				Url = ImgurAPIBaseUrl + "/upload",
				Data = formDataContent,
				AccessToken = ImgurAPIAccessToken
			});
		}


		public async Task<T?> DeleteImageAsync<T>(string imageId)
		{
			return await SendAsync<T?>(new ApiRequestDto
			{
				ApiType = ApiType.DELETE,
				Url = ImgurAPIBaseUrl + $"/image/{imageId}",
				AccessToken = ImgurAPIAccessToken
			});
		}
		private MultipartFormDataContent CreateFormData(IFormFile file)
		{
			var formDataContent = new MultipartFormDataContent
			{
				{ new StringContent(ImgurAPIAlbumHash), "album" },
			};

			byte[] byteArray;
			using (MemoryStream memoryStream = new())
			{
				file.OpenReadStream().CopyTo(memoryStream);
				byteArray = memoryStream.ToArray();
			}

			var imageContent = new ByteArrayContent(byteArray);
			imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
			formDataContent.Add(imageContent, "image", "image.jpg");
			return formDataContent;
		}
	}
}
