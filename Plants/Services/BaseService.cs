using Plants.Models.Dto;
using Plants.Services.IServices;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Plants.Services
{
    public class BaseService : IBaseService
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public BaseService(IHttpClientFactory httpClientFactory) //is called while Dependency Injection is used
		{
            _httpClientFactory = httpClientFactory;
		}

		public async Task<T?> SendAsync<T>(ApiRequestDto apiRequest)
		{
			try
			{
				var client = _httpClientFactory.CreateClient("API");
				HttpRequestMessage message = new();
				message.Headers.Add("Accept", "application/json");
				message.RequestUri = new Uri(apiRequest.Url);
				client.DefaultRequestHeaders.Clear();
				if (apiRequest.Data != null)
				{
					message.Content = apiRequest.Data switch
					{
						MultipartFormDataContent formData => formData,
						_ => new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json")
					};
				}

				if (!string.IsNullOrEmpty(apiRequest.AccessToken))
				{
					//helps to send a token with each request
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);
				}
				HttpResponseMessage? apiResponse = null;
				message.Method = apiRequest.ApiType switch
				{
					StaticDetails.ApiType.POST => HttpMethod.Post,
					StaticDetails.ApiType.PUT => HttpMethod.Put,
					StaticDetails.ApiType.DELETE => HttpMethod.Delete,
					_ => HttpMethod.Get
				};

				apiResponse = await client.SendAsync(message);
				string apiContent = await apiResponse.Content.ReadAsStringAsync();
				if (string.IsNullOrEmpty(apiContent))
				{
					throw new Exception("HTTP-response: " + (int)apiResponse.StatusCode + " -- " + apiResponse.ReasonPhrase);
				}

				var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
				return apiResponseDto;
			}
			catch (Exception ex)
			{
				var response = new ResponseDto()
				{
					IsSuccess = false,
					ErrorMessages = new List<string>() { ex.Message }
				};
				var res = JsonConvert.SerializeObject(response);
				var apiResponseDto = JsonConvert.DeserializeObject<T>(res); //we use serialization/deserialization for returning an object of T type
				return apiResponseDto;
			}
		}

		public void Dispose()
		{
			GC.SuppressFinalize(true);
		}

	}
}
