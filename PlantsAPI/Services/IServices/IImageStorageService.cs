namespace Services.PlantsAPI.Services.IServices
{
	public interface IImageStorageService
	{
		Task<T?> DeleteImageAsync<T>(string imageServiceId, string token);
	}
}
