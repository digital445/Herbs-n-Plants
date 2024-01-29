namespace Plants.Services.IServices
{
	public interface IImageStorageService
	{
		Task<T?> UploadImageAsync<T>(IFormFile file);
		Task<T?> DeleteImageAsync<T>(string imageId);
	}
}
