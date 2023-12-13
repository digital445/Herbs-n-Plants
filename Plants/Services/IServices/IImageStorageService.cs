namespace Plants.Services.IServices
{
	public interface IImageStorageService
	{
		Task<T?> UploadImageAsync<T>(IFormFile file, string token);
		Task<T?> DeleteImageAsync<T>(string imageServiceId, string token);
	}
}
