using Plants.Models.Dto;

namespace Plants.Services.IServices
{
    public interface IPlantsService : IBaseService
    {
        Task<T?> GetAllAsync<T>(string token);
        Task<T?> GetPageAsync<T>(int page, int pageSize, string token);
        Task<T?> GetFilteredAsync<T>(PlantDto plantDto, string token);
        Task<T?> GetAsync<T>(int id, string token);
        Task<T?> CreateAsync<T>(PlantDto plantDto, string token);
        Task<T?> UpdateAsync<T>(PlantDto plantDto, string token);
        Task<T?> DeleteAsync<T>(int id, string token);
    }
}
