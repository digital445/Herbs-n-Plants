using System.Net.Http.Headers;
using Plants.Services.IServices;
using Plants.Models;
using static Plants.StaticDetails;

namespace Plants.Services
{
	public class ImgurImageStorageService : BaseService, IImageStorageService
	{
		private const string albumHash = "piwTiWk";
		public ImgurImageStorageService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
		{
		}

		public async Task<T?> UploadImageAsync<T>(IFormFile file, string token)
		{
			using var formDataContent = GetFormData(file);

			return await SendAsync<T?>(new ApiRequest
			{
				ApiType = ApiType.POST,
				Url = "https://api.imgur.com/3/upload",
				Data = formDataContent,
				AccessToken = token
			});
		}


		public async Task<T?> DeleteImageAsync<T>(string imageServiceId, string token)
		{
			return await SendAsync<T?>(new ApiRequest
			{
				ApiType = ApiType.DELETE,
				Url = $"https://api.imgur.com/3/image/{imageServiceId}",
				AccessToken = token
			});
		}
		private MultipartFormDataContent GetFormData(IFormFile file)
		{
			var formDataContent = new MultipartFormDataContent
			{
				{ new StringContent(albumHash), "album" },
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
