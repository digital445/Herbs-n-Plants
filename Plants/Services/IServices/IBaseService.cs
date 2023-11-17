using Plants.Models;

namespace Plants.Services.IServices
{
	public interface IBaseService : IDisposable
	{
		Task<T?> SendAsync<T>(ApiRequest apiRequest);
	}
}
