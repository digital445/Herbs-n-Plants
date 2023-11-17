namespace Plants.Services.IServices
{
	public interface IImageService
	{
		Task<T?> UploadImageAsync<T>(IFormFile file, string token);
	}
}
