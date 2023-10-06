using Plants.Models;
using Plants.Models.Dto;

namespace Plants.Services.IServices
{
	public interface IBaseService : IDisposable
	{
		//ResponseDto Response { get; set; }
		Task<T?> SendAsync<T>(ApiRequest apiRequest);
	}
}
