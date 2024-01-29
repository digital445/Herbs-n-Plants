using Services.PlantsAPI.Models.Dto;

namespace Services.PlantsAPI.Services.IServices
{
    public interface IBaseService : IDisposable
	{
		Task<T?> SendAsync<T>(ApiRequestDto apiRequest);
	}
}
