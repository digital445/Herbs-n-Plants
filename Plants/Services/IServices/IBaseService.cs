using Plants.Models.Dto;

namespace Plants.Services.IServices
{
    public interface IBaseService : IDisposable
	{
		Task<T?> SendAsync<T>(ApiRequestDto apiRequest);
	}
}
