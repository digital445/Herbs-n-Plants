using Services.PlantsAPI.Models;

namespace Services.PlantsAPI.Services.IServices
{
	public interface IBaseService : IDisposable
	{
		Task<T?> SendAsync<T>(ApiRequest apiRequest);
	}
}
