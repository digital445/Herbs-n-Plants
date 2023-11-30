using Plants.Models.Dto;

namespace Plants.Services.IServices
{
    public interface IPlantsService : IBaseService
    {
        Task<T?> GetPageAsync<T>(int page, int pageSize, string token);
        Task<T?> GetFilteredAsync<T>(FilterDto filter, int page, int pageSize, string token);
        /// <summary>
        /// Gets a Plant by Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="token">API access token</param>
        /// <returns></returns>
        Task<T?> GetAsync<T>(int id, string token);
        Task<T?> CreateUpdateAsync<T>(PlantDto plantDto, string token);
        Task<T?> DeleteAsync<T>(int id, string token);
        Task<T?> GetPaletteAsync<T>(string token);
	}
}
