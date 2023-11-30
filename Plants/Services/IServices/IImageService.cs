namespace Plants.Services.IServices
{
	public interface IImageService
	{
		Task<T?> UploadImageAsync<T>(IFormFile file, string token);
		Task<T?> DeleteImageAsync<T>(string imageServiceId, string token);
	}
}
