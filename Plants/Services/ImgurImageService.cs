using System.Net.Http.Headers;
using Plants.Services.IServices;
using Plants.Models;
using static Plants.StaticDetails;

namespace Plants.Services
{
	public class ImgurImageService : BaseService, IImageService
	{
		private const string albumHash = "piwTiWk";
		public ImgurImageService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
		{
		}

		public async Task<T?> UploadImageAsync<T>(IFormFile file, string token)
		{
			using var formData = new MultipartFormDataContent
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
			formData.Add(imageContent, "image", "image.jpg");

			return await SendAsync<T?>(new ApiRequest
			{
				ApiType = ApiType.POST,
				Url = "https://api.imgur.com/3/upload",
				Data = formData,
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


	}
}
